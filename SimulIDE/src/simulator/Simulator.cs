﻿using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    public class Simulator
    {

        public Simulator()
        {

            mself = this;

            isrunning = false;
            debugging = false;
            paused = false;
            runMcu = false;

            step = 0;
            numEnodes = 0;
            timerId = 0;
            lastStep = 0;
            lastRefTime = 0;
            timerSc = 1;
            timerTick = 50;
            stepsPrea = 50;
            simuRate = 1000000;
            noLinAcc = 5; // Non-Linear accuracy

            stepsPerus = 1;

            //TYV   refTimer.Start();
        }
        ~Simulator()
        {
//TYV            CircuitFuture.WaitForFinished();
        }

        protected void SolveMatrix()
        {
            foreach (eNode node in eChangedNodeList) node.StampMatrix();
            eChangedNodeList.Clear();

            if (!matrix.SolveMatrix())                // Try to solve matrix,
            {                                         // if fail stop simulation
                Console.WriteLine("Simulator::solveMatrix(), Failed to solve Matrix");
                error = true;
            }                                // m_matrix sets the eNode voltages
        }

        //TYV        protected void TimerEvent(TimerEvent e)  //update at m_timerTick rate (50 ms, 20 Hz max)
        //TYV        {
        //TYV        e.Aaccept();

        //TYV            if (!isrunning) return;
        //TYV            if (error)
        //TYV            {
        //TYV      CircuitWidget.Self().PowerCircOff();
        //TYV      CircuitWidget.Self().SetRate(-1);
        //TYV                return;
        //TYV}
        //TYV            if (!CircuitFuture.isFinished()) // Stop remaining parallel thread
        //TYV            {
        //TYV                isrunning = false;
        //TYV                CircuitFuture.WaitForFinished();
        //TYV                isrunning = true;
        //TYV                //return;
        //TYV            }
        // Get Real Simulation Speed
        //TYVrefTime = RefTimer.nsecsElapsed();
        //TYVtStep = step;

        //TYVRunGraphicStep1();
        // Run Circuit in parallel thread
        //TYV            CircuitFuture = QtConcurrent::run(this, Simulator.RunCircuit); // Run Circuit in a parallel thread

        //TYVRunGraphicStep2();
        //TYV}

        public void RunGraphicStep()
        {
            RunGraphicStep1();
            RunGraphicStep2();
        }

        public void RunGraphicStep1()
        {
            foreach (eElement el in  updateList) el.UpdateStep();
            //emit stepTerms();
            //TerminalWidget::self()->step();
        }

        public void RunGraphicStep2()
        {
            //qDebug() <<"Simulator::runGraphicStep";
            if (debugging) tStep = step;        // Debugger driving, not timer

            //TYV        if (Circuit.Self().Animate())
            //TYV        {
            //TYV            Circuit.Self().UpdateConnectors();
            //TYV            foreach (eNode enode in eNodeList) enode.SetVoltChanged(false);
            //TYV        }

            UInt64 deltaRefTime = refTime - lastRefTime;
            if (deltaRefTime >= 1e9)               // We want steps per Sec = 1e9 ns
            {
                stepsPerSec = (ulong)((tStep - lastStep) * 1e9 / deltaRefTime);
           //TYV     CircuitWidget.Self().SetRate((stepsPerSec * 100) / (1e6 * stepsPerus) /*m_simuRate*/ );
                lastStep = tStep;
                lastRefTime = refTime;
            }

            //TYV  CircuitView.Self().SetCircTime(tStep / stepsPerus);
            //TYV PlotterWidget.Self().Step();
        }

        public void RunCircuit()
        {
            /*uint64_t time0 = m_RefTimer.nsecsElapsed();
            static uint64_t count;
            static uint64_t circTime;
            if( m_step == 0 )
            {
                count = 0;
                circTime = 0;
            }*/

            for (int i = 0; i < circuitRate; i++)
            {
                circTime =(ulong) (step * stepNS);         // Circuit Time in nanoseconds

                if (runMcu) BaseProcessor.Self().Step();             // Run MCU

                if (simuClock.Count!=0)     // Run elements at Simulation Clock
                {
                    foreach (eElement el in simuClock) el.SimuClockStep();
                }
                if (changedFast.Count()!=0)                  // Run Fast elements
                {
                    foreach (eElement el in changedFast) el.SetVChanged();
                    changedFast.Clear();
                }

                if (step >= reacCounter)               // Run Reactive Elements
                {
                    reacCounter +=(ulong) (stepsPrea * stepsPerus);
                    if (reactive.Count!=0)
                    {
                        foreach (eElement el in reactive) el.SetVChanged();
                        reactive.Clear();
                    }
                }

                if (nonLinear.Count!=0)              // Run Non-Linear elements
                {
                    int counter = 0;
                    int limit = (noLinAcc - 2) * 100;

                    while (nonLinear.Count()!=0)      // Run untill all converged
                    {
                        foreach (eElement el in nonLinear) el.SetVChanged();
                        nonLinear.Clear();

                        if (eChangedNodeList.Count()!=0) SolveMatrix();

                        if (++counter > limit) break; // Limit the number of loops
                    }
                    //if( counter > 0 ) qDebug() << "\nSimulator::runCircuit  Non-Linear Solved in steps:"<<counter;
                }
                if (eChangedNodeList.Count()!=0) SolveMatrix();

                step++;
                if (!isrunning) break; // Keep this at the end for debugger to run 1 step
            }
            /*uint64_t time = m_RefTimer.nsecsElapsed()-time0;
            count++;
            circTime += time;*/
            //if( count == 1000 )
            //qDebug() << "Simulator::runCircuit Time nS:"<<time<<circTime<<count << circTime/count;
        }

        public void RunExtraStep(uint cycle)
        {
            //if( !m_isrunning ) return;

            //qDebug() <<"\nSimulator::runExtraStep"<<m_circTime<<m_step<<m_stepNS;
            circTime =(ulong) (cycle * mcuStepNS);
            //qDebug() <<"Simulator::runExtraStep"<<m_circTime<<cycle<<m_mcuStepNS;

            if (eChangedNodeList.Count!=0)
            {
                SolveMatrix();
                //if( !m_isrunning ) return;
            }

            // Run Fast elements
            foreach (eElement el in changedFast) el.SetVChanged();
            changedFast.Clear();
        }

        public void RunContinuous()
        {
            if (debugging)                     // Debugger Controllig Simulation
            {
                Debug(false);
                OnResumeDebug?.Invoke(); 
                return;
            }
            SimuRateChanged(simuRate);
            StartSim();

            Console.WriteLine("\n    Simulation Running... \n");
            //timerId = this.StartTimer(timerTick, Qt::PreciseTimer);
        }

        public void Debug(bool run)
        {
            if (run)
            {
                debugging = false;
                RunContinuous();
            }
            else
            {
                StartSim();
                isrunning = false;
                debugging = true;
                Console.WriteLine("\n    Debugger Controllig Simulation... \n");
            }
            runMcu = false;
        }

        public void StartSim()
        {
            Console.WriteLine("\nStarting Circuit Simulation...\n");

            foreach (eNode busNode in eNodeBusList) busNode.Initialize(); // Clear Buses

            Console.WriteLine("  Initializing " + elementList.Count().ToString() + "\teElements");
            foreach (eElement el in elementList)    // Initialize all Elements
            {
                Console.WriteLine("initializing  "+el.GetId());
                if (!paused) el.ResetState();
                el.Initialize();
            }
            Console.WriteLine("  Initializing " + eNodeBusList.Count().ToString() + "\tBuses");
            foreach (eNode busNode in eNodeBusList) busNode.CreateBus(); // Create Buses

            nonLinear.Clear();
            changedFast.Clear();
            reactive.Clear();
            eChangedNodeList.Clear();

            // Connect Elements with internal circuits.
            foreach (eElement el in elementList) el.Attach();

//TYV            if (McuComponent.Self()!=null && !paused) McuComponent.Self().RunAutoLoad();


            numEnodes = eNodeList.Count();
            Console.WriteLine("  Initializing " + eNodeList.Count().ToString() + "\teNodes");
            for (int i = 0; i < numEnodes; i++)
            {
                eNode enode = eNodeList[i];
                enode.SetNodeNumber(i + 1);
                enode.Initialize();
            }
            foreach (eElement el in elementList) el.Stamp();

            // Initialize Matrix
            matrix.CreateMatrix(eNodeList);

            // Try to solve matrix, if fails stop simulation
            ////// m_matrix.printMatrix();
            if (!matrix.SolveMatrix())
            {
                Console.WriteLine("Simulator::startSim, Failed to solve Matrix");
//TYV                CircuitWidget.Self().PowerCircOff();
//TYV                CircuitWidget.Self().SetRate(-1);
                return;
            }
            //////for( eElement* el : m_elementList ) el->setVChanged();
            Console.WriteLine("\nCircuit Matrix looks good");
            if (!paused)
            {
                lastStep = 0;
                lastRefTime = 0;
                updtCounter = (ulong) circuitRate;
                reacCounter = (ulong) stepsPrea;
            }
            isrunning = true;
            paused = false;
            error = false;
//TYV            CircuitView.Self().SetCircTime(0);
        }

        public void StopDebug()
        {
            debugging = false;
            StopSim();
        }

        public void StopSim()
        {
            ////if( !m_isrunning ) return;

            StopTimer();
            ////emit pauseDebug();
            paused = false;
            circTime = 0;
            step = 0;

            foreach (eNode node  in eNodeList) node.SetVolt(0);
            foreach (eElement el in elementList) el.ResetState();
            foreach (eElement el in updateList) el.UpdateStep();

//TYV            if (McuComponent.Self()!=null) McuComponent.Self().Reset();

//TYV            CircuitWidget.Self().SetRate(0);
//TYV            Circuit.Self().Update();
            Console.WriteLine("\n    Simulation Stopped \n");
        }

        public void PauseSim()
        {
            OnPauseDebug?.Invoke();
            paused = true;
            StopTimer();
            Console.WriteLine("\n    Simulation Paused \n");
        }

        public void ResumeSim()
        {
            isrunning = true;
            paused = false;

            OnResumeDebug?.Invoke();
            if (debugging) return;
            Console.WriteLine("\n    Resuming Simulation\n");
            //timerId = this->startTimer(m_timerTick, Qt::PreciseTimer);
        }

        public void StopTimer()
        {
            if (timerId != 0)
            {
                isrunning = false;
                //KillTimer(timerId); 
                timerId = 0;
              //TYV  CircuitFuture.WaitForFinished();
            }
        }

        public void ResumeTimer()
        {
            if (timerId == 0)
            {
                isrunning = true;
               //TYV timerId = this->startTimer(m_timerTick, Qt::PreciseTimer);
            }
        }

        protected int SimuRateChanged(int rate)
        {
            //if( rate > 1e6 ) rate = 1e6;
            if (rate < 1) rate = 1;

            stepsPerus = rate / 1e6;
            if (stepsPerus < 1) stepsPerus = 1;
            stepNS = 1000 / stepsPerus;

            runMcu = false;

            if (BaseProcessor.Self()!=null)
            {
                //double mcuSteps = McuComponent::self()->freq()*1e6;
                //if( mcuSteps > rate ) m_stepsPerus = (int)

//TYV                double mcuSteps = McuComponent.Self().Freq() / stepsPerus;
//TYV                BaseProcessor.Self().SetSteps(mcuSteps);
//TYV                mcuStepNS = 1000 / McuComponent.Self().Freq();
                runMcu = true;
                //qDebug() <<"Simulator::simuRateChanged mcuSteps"<<mcuSteps<<m_mcuStepNS;
            }

            timerTick = 50 / timerSc;
            int fps = 1000 / timerTick;
            int mult = 20;
            if (fps == 40) mult = 40;

            circuitRate = (rate / fps);

            if (rate < fps)
            {
                fps = rate;
                circuitRate = 1;
                timerTick = 1000 / rate;
            }

            if (isrunning)
            {
                PauseSim();
                OnRateChanged?.Invoke();
                ResumeSim();
            }

            ////PlotterWidget::self()->setPlotterTick( m_circuitRate*mult );
//TYV            PlotterWidget.Self().SetPlotterTick(circuitRate * mult / stepsPerus));

            simuRate = (int)(circuitRate * fps);
            Console.WriteLine("\nFPS:              " + fps.ToString()
                      + "\nCircuit Rate:     " + circuitRate.ToString()
                      + "\nSimulation Speed: " + simuRate.ToString()
                      + "\nReactive SubRate: " + stepsPrea.ToString());
            return simuRate;
        }

        public double StepsPerus()
        {
            return stepsPerus;
        }

        public bool IsRunning()
        {
            return isrunning;
        }

        public bool IsPaused()
        {
            return paused;
        }

        public int ReaClock()
        {
            return stepsPrea;
        }

        public void SetReaClock(int value)
        {
            bool running = isrunning;
            if (running) StopSim();

            if (value < 1) value = 1;
            else if (value > 100) value = 100;

            stepsPrea = value;

            if (running) RunContinuous();
        }

        public int NoLinAcc() { return noLinAcc; }
        public void SetNoLinAcc(int ac)
        {
            bool running = isrunning;
            if (running) StopSim();

            if (ac < 3) ac = 3;
            else if (ac > 14) ac = 14;
            noLinAcc = ac;

            if (running) RunContinuous();
        }
        public double NLaccuracy()
        {
            return 1 / Math.Pow(10, noLinAcc) / 2;
        }

        public UInt64 Step()
        {
            return step;
        }

        public UInt64 StepAnimate()
        {
            return step;
        }

        UInt64 CircTime()
        {
            return circTime;
        }

        public void AddToEnodeBusList(eNode nod)
        {
            if (!eNodeBusList.Contains(nod)) eNodeBusList.Add(nod);
        }

        public void RemFromEnodeBusList(eNode nod, bool del)
        {
            if (eNodeBusList.Contains(nod)) eNodeBusList.Remove(nod);
  //          if (del) { delete nod; }
        }

        public void AddToEnodeList(eNode nod)
        {
            if (!eNodeList.Contains(nod)) eNodeList.Add(nod);
        }

        public void RemFromEnodeList(eNode nod, bool del)
        {
            if (eNodeList.Contains(nod))
            {
                eNodeList.Remove(nod);
//                if (del) delete nod;
            }
        }

        public void AddToChangedNodeList(eNode nod)
        {
            if (!eChangedNodeList.Contains(nod)) eChangedNodeList.Add(nod);
        }
        public void RemFromChangedNodeList(eNode nod)
        {
            eChangedNodeList.Remove(nod);
        }

        public void AddToElementList(eElement el)
        {
            if (!elementList.Contains(el)) elementList.Add(el);
        }

        public void RemFromElementList(eElement el)
        {
            if (elementList.Contains(el)) elementList.Remove(el);
        }

        public void AddToUpdateList(eElement el)
        {
            if (!updateList.Contains(el)) updateList.Add(el);
        }

        public void RemFromUpdateList(eElement el)
        {
            updateList.Remove(el);
        }

        public void AddToSimuClockList(eElement el)
        {
            if (!simuClock.Contains(el)) simuClock.Add(el);
        }

        public void RemFromSimuClockList(eElement el)
        {
            simuClock.Remove(el);
        }

        public void AddToChangedFast(eElement el)
        {
            if (!changedFast.Contains(el)) changedFast.Add(el);
        }

        public void RemFromChangedFast(eElement el)
        {
            changedFast.Remove(el);
        }

        public void AddToReactiveList(eElement el)
        {
            if (!reactive.Contains(el)) reactive.Add(el);
        }

        public void RemFromReactiveList(eElement el)
        {
            reactive.Remove(el);
        }

        public void AddToNoLinList(eElement el)
        {
            if (!nonLinear.Contains(el)) nonLinear.Add(el);
        }

        public void RemFromNoLinList(eElement el)
        {
            nonLinear.Remove(el);
        }

        public void AddToMcuList(BaseProcessor proc)
        {
            if (!mcuList.Contains(proc)) mcuList.Add(proc);
        }
        public void RemFromMcuList(BaseProcessor proc)
        {
            mcuList.Remove(proc);
        }

        // Header
        public static Simulator Self() { return mself; }
        
        public int CircuitRate() { return (int)circuitRate; }
        public int SimuRate() { return simuRate; }
        
        public void SetTimerScale(int ts) { timerSc = ts; }
        public List<eNode> GeteNodes() { return eNodeList; }

        
        public UInt64 stepsPerSec;

        public UInt64 mS() { return (UInt64) Stopwatch.GetTimestamp(); /* refTimer.Elapsed();*/ }

        public event Action OnPauseDebug;
        public event Action OnResumeDebug;
        public event Action OnRateChanged;

        private static Simulator mself=null;

 //TYV       private QFuture<void> CircuitFuture;

        private CircMatrix matrix;

        private List<eNode> eNodeList = new List<eNode>();
        private List<eNode> eChangedNodeList = new List<eNode>();
        private List<eNode> eNodeBusList = new List<eNode>();

        private List<eElement> elementList = new List<eElement>();
        private List<eElement> updateList = new List<eElement>();

        private List<eElement> changedFast = new List<eElement>();
        private List<eElement> reactive = new List<eElement>();
        private List<eElement> nonLinear = new List<eElement>();
        private List<eElement> simuClock = new List<eElement>();
        private List<BaseProcessor> mcuList = new List<BaseProcessor>();

        private bool isrunning;
        private bool debugging;
        private bool runMcu;
        private bool paused;
        private bool error;

        private int timerId;
        private int timerTick;
        private int timerSc;
        private int numEnodes;
        private int stepsPrea;

        private int noLinAcc;
        private int simuRate;
        private double stepsPerus;
        private double stepNS;
        private double mcuStepNS;

        private int circuitRate;
        private UInt64 circTime;
        private UInt64 reacCounter;
        private UInt64 updtCounter;
        private UInt64 step;
        private UInt64 tStep;
        private UInt64 lastStep;
        private UInt64 refTime;
        private UInt64 lastRefTime;
        private Timer refTimer;

        public object CircuitWidget { get; private set; }
    }
}
