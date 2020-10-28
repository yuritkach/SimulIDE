using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulIDE.src.simulator
{
    class Simulator
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

            refTimer.start();
        }
        ~Simulator()
        {
            CircuitFuture.WaitForFinished();
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

        protected void TimerEvent(TimerEvent e)  //update at m_timerTick rate (50 ms, 20 Hz max)
        {
            e.Aaccept();

            if (!isrunning) return;
            if (error)
            {
                CircuitWidget.Self().PowerCircOff();
                CircuitWidget.Self().SetRate(-1);
                return;
            }
            if (!CircuitFuture.isFinished()) // Stop remaining parallel thread
            {
                isrunning = false;
                CircuitFuture.WaitForFinished();
                isrunning = true;
                //return;
            }
            // Get Real Simulation Speed
            refTime = RefTimer.nsecsElapsed();
            tStep = step;

            RunGraphicStep1();
            // Run Circuit in parallel thread
            CircuitFuture = QtConcurrent::run(this, &Simulator::runCircuit); // Run Circuit in a parallel thread

            RunGraphicStep2();
        }

        protected void RunGraphicStep()
        {
            RunGraphicStep1();
            RunGraphicStep2();
        }

        protected void RunGraphicStep1()
        {
            foreach (eElement el in  updateList) el.UpdateStep();
            //emit stepTerms();
            //TerminalWidget::self()->step();
        }

        protected void RunGraphicStep2()
        {
            //qDebug() <<"Simulator::runGraphicStep";
            if (debugging) tStep = step;        // Debugger driving, not timer

            if (Circuit.Self().Animate())
            {
                Circuit.Self().UpdateConnectors();
                foreach (eNode enode in eNodeList) enode.SetVoltChanged(false);
            }

            UInt64 deltaRefTime = refTime - lastRefTime;
            if (deltaRefTime >= 1e9)               // We want steps per Sec = 1e9 ns
            {
                stepsPerSec = (tStep - lastStep) * 1e9 / deltaRefTime;
                CircuitWidget.Self().SetRate((stepsPerSec * 100) / (1e6 * stepsPerus) /*m_simuRate*/ );
                lastStep = tStep;
                lastRefTime = refTime;
            }

            CircuitView.Self().SetCircTime(tStep / stepsPerus);
            PlotterWidget.Self().Step();
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

            for (UInt64 i = 0; i < circuitRate; i++)
            {
                circTime = step * stepNS;         // Circuit Time in nanoseconds

                if (runMcu) BaseProcessor.Self().Step();             // Run MCU

                if (!simuClock.isEmpty())     // Run elements at Simulation Clock
                {
                    foreach (eElement el in simuClock) el.SimuClockStep();
                }
                if (!changedFast.isEmpty())                  // Run Fast elements
                {
                    foreach (eElement el in changedFast) el.SetVChanged();
                    changedFast.Clear();
                }

                if (step >= reacCounter)               // Run Reactive Elements
                {
                    reacCounter += stepsPrea * stepsPerus;
                    if (!reactive.IsEmpty())
                    {
                        foreach (eElement el in reactive) el.SetVChanged();
                        reactive.Clear();
                    }
                }

                if (!nonLinear.IsEmpty())              // Run Non-Linear elements
                {
                    int counter = 0;
                    int limit = (noLinAcc - 2) * 100;

                    while (!nonLinear.isEmpty())      // Run untill all converged
                    {
                        foreach (eElement el in nonLinear) el.SetVChanged();
                        nonLinear.Clear();

                        if (!eChangedNodeList.IsEmpty()) SolveMatrix();

                        if (++counter > limit) break; // Limit the number of loops
                    }
                    //if( counter > 0 ) qDebug() << "\nSimulator::runCircuit  Non-Linear Solved in steps:"<<counter;
                }
                if (!eChangedNodeList.IsEmpty()) SolveMatrix();

                step++;
                if (!isrunning) break; // Keep this at the end for debugger to run 1 step
            }
            /*uint64_t time = m_RefTimer.nsecsElapsed()-time0;
            count++;
            circTime += time;*/
            //if( count == 1000 )
            //qDebug() << "Simulator::runCircuit Time nS:"<<time<<circTime<<count << circTime/count;
        }

        public void RunExtraStep(UInt64 cycle)
        {
            //if( !m_isrunning ) return;

            //qDebug() <<"\nSimulator::runExtraStep"<<m_circTime<<m_step<<m_stepNS;
            circTime = cycle * mcuStepNS;
            //qDebug() <<"Simulator::runExtraStep"<<m_circTime<<cycle<<m_mcuStepNS;

            if (!eChangedNodeList.IsEmpty())
            {
                solveMatrix();
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
                debug(false);
                OnResumeDebug?.Invoke(); 
                return;
            }
            simuRateChanged(m_simuRate);
            startSim();

            std::cout << "\n    Simulation Running... \n" << std::endl;
            m_timerId = this->startTimer(m_timerTick, Qt::PreciseTimer);
        }

        void Simulator::debug(bool run)
        {
            if (run)
            {
                m_debugging = false;
                runContinuous();
            }
            else
            {
                startSim();
                m_isrunning = false;
                m_debugging = true;
                std::cout << "\n    Debugger Controllig Simulation... \n" << std::endl;
            }
            m_runMcu = false;
        }

        void Simulator::startSim()
        {
            std::cout << "\nStarting Circuit Simulation...\n" << std::endl;

            for (eNode* busNode : m_eNodeBusList) busNode->initialize(); // Clear Buses

            std::cout << "  Initializing " << m_elementList.size() << "\teElements" << std::endl;
            for (eElement* el : m_elementList)    // Initialize all Elements
            {
                //std::cout << "initializing  "<< el->getId()<<  std::endl;
                if (!m_paused) el->resetState();
                el->initialize();
            }

            std::cout << "  Initializing " << m_eNodeBusList.size() << "\tBuses" << std::endl;
            for (eNode* busNode : m_eNodeBusList) busNode->createBus(); // Create Buses

            m_nonLinear.clear();
            m_changedFast.clear();
            m_reactive.clear();
            m_eChangedNodeList.clear();

            // Connect Elements with internal circuits.
            for (eElement* el : m_elementList) el->attach();

            if (McuComponent::self() && !m_paused) McuComponent::self()->runAutoLoad();


            m_numEnodes = m_eNodeList.size();
            std::cout << "  Initializing " << m_eNodeList.size() << "\teNodes" << std::endl;
            for (int i = 0; i < m_numEnodes; i++)
            {
                eNode* enode = m_eNodeList.at(i);
                enode->setNodeNumber(i + 1);
                enode->initialize();
            }
            for (eElement* el : m_elementList) el->stamp();

            // Initialize Matrix
            m_matrix.createMatrix(m_eNodeList);

            // Try to solve matrix, if fails stop simulation
            // m_matrix.printMatrix();
            if (!m_matrix.solveMatrix())
            {
                std::cout << "Simulator::startSim, Failed to solve Matrix"
                          << std::endl;
                CircuitWidget::self()->powerCircOff();
                CircuitWidget::self()->setRate(-1);
                return;
            }
            //for( eElement* el : m_elementList ) el->setVChanged();
            std::cout << "\nCircuit Matrix looks good" << std::endl;
            if (!m_paused)
            {
                m_lastStep = 0;
                m_lastRefTime = 0;
                m_updtCounter = m_circuitRate;
                m_reacCounter = m_stepsPrea;
            }
            m_isrunning = true;
            m_paused = false;
            m_error = false;
            CircuitView::self()->setCircTime(0);
        }

        void Simulator::stopDebug()
        {
            m_debugging = false;
            stopSim();
        }

        void Simulator::stopSim()
        {
            //if( !m_isrunning ) return;

            stopTimer();
            //emit pauseDebug();
            m_paused = false;
            m_circTime = 0;
            m_step = 0;

            for (eNode* node  : m_eNodeList) node->setVolt(0);
            for (eElement* el : m_elementList) el->resetState();
            for (eElement* el : m_updateList) el->updateStep();

            if (McuComponent::self()) McuComponent::self()->reset();

            CircuitWidget::self()->setRate(0);
            Circuit::self()->update();

            std::cout << "\n    Simulation Stopped \n" << std::endl;
        }

        void Simulator::pauseSim()
        {
            emit pauseDebug();
            m_paused = true;
            stopTimer();

            std::cout << "\n    Simulation Paused \n" << std::endl;
        }

        void Simulator::resumeSim()
        {
            m_isrunning = true;
            m_paused = false;

            emit resumeDebug();

            if (m_debugging) return;

            std::cout << "\n    Resuming Simulation\n" << std::endl;
            m_timerId = this->startTimer(m_timerTick, Qt::PreciseTimer);
        }

        void Simulator::stopTimer()
        {
            if (m_timerId != 0)
            {
                m_isrunning = false;
                this->killTimer(m_timerId);
                m_timerId = 0;
                m_CircuitFuture.waitForFinished();
            }
        }

        void Simulator::resumeTimer()
        {
            if (m_timerId == 0)
            {
                m_isrunning = true;
                m_timerId = this->startTimer(m_timerTick, Qt::PreciseTimer);
            }
        }

        int Simulator::simuRateChanged(int rate)
        {
            //if( rate > 1e6 ) rate = 1e6;
            if (rate < 1) rate = 1;

            m_stepsPerus = rate / 1e6;
            if (m_stepsPerus < 1) m_stepsPerus = 1;
            m_stepNS = 1000 / m_stepsPerus;

            m_runMcu = false;

            if (BaseProcessor::self())
            {
                //double mcuSteps = McuComponent::self()->freq()*1e6;
                //if( mcuSteps > rate ) m_stepsPerus = (int)

                double mcuSteps = McuComponent::self()->freq() / m_stepsPerus;
                BaseProcessor::self()->setSteps(mcuSteps);
                m_mcuStepNS = 1000 / McuComponent::self()->freq();
                m_runMcu = true;
                //qDebug() <<"Simulator::simuRateChanged mcuSteps"<<mcuSteps<<m_mcuStepNS;
            }

            m_timerTick = 50 / m_timerSc;
            int fps = 1000 / m_timerTick;
            int mult = 20;
            if (fps == 40) mult = 40;

            m_circuitRate = rate / fps;

            if (rate < fps)
            {
                fps = rate;
                m_circuitRate = 1;
                m_timerTick = 1000 / rate;
            }

            if (m_isrunning)
            {
                pauseSim();
                emit rateChanged();
                resumeSim();
            }

            //PlotterWidget::self()->setPlotterTick( m_circuitRate*mult );
            PlotterWidget::self()->setPlotterTick(m_circuitRate * mult / m_stepsPerus);

            m_simuRate = m_circuitRate * fps;

            std::cout << "\nFPS:              " << fps
                      << "\nCircuit Rate:     " << m_circuitRate
                      << std::endl
                      << "\nSimulation Speed: " << m_simuRate
                      << "\nReactive SubRate: " << m_stepsPrea
                      << std::endl
                      << std::endl;

            return m_simuRate;
        }

        double Simulator::stepsPerus()
        {
            return m_stepsPerus;
        }

        bool Simulator::isRunning()
        {
            return m_isrunning;
        }

        bool Simulator::isPaused()
        {
            return m_paused;
        }

        int Simulator::reaClock()
        {
            return m_stepsPrea;
        }

        void Simulator::setReaClock(int value)
        {
            bool running = m_isrunning;
            if (running) stopSim();

            if (value < 1) value = 1;
            else if (value > 100) value = 100;

            m_stepsPrea = value;

            if (running) runContinuous();
        }

        int Simulator::noLinAcc() { return m_noLinAcc; }
        void Simulator::setNoLinAcc(int ac)
        {
            bool running = m_isrunning;
            if (running) stopSim();

            if (ac < 3) ac = 3;
            else if (ac > 14) ac = 14;
            m_noLinAcc = ac;

            if (running) runContinuous();
        }
        double Simulator::NLaccuracy()
        {
            return 1 / pow(10, m_noLinAcc) / 2;
        }

        uint64_t Simulator::step()
        {
            return m_step;
        }

        uint64_t Simulator::circTime()
        {
            return m_circTime;
        }

        void Simulator::addToEnodeBusList(eNode* nod)
        {
            if (!m_eNodeBusList.contains(nod)) m_eNodeBusList.append(nod);
        }

        void Simulator::remFromEnodeBusList(eNode* nod, bool del)
        {
            if (m_eNodeBusList.contains(nod)) m_eNodeBusList.removeOne(nod);
            if (del) { delete nod; }
        }

        void Simulator::addToEnodeList(eNode* nod)
        {
            if (!m_eNodeList.contains(nod)) m_eNodeList.append(nod);
        }

        void Simulator::remFromEnodeList(eNode* nod, bool del)
        {
            if (m_eNodeList.contains(nod))
            {
                m_eNodeList.removeOne(nod);
                if (del) delete nod;
            }
        }

        void Simulator::addToChangedNodeList(eNode* nod)
        {
            if (!m_eChangedNodeList.contains(nod)) m_eChangedNodeList.append(nod);
        }
        void Simulator::remFromChangedNodeList(eNode* nod)
        {
            m_eChangedNodeList.removeOne(nod);
        }

        void Simulator::addToElementList(eElement* el)
        {
            if (!m_elementList.contains(el)) m_elementList.append(el);
        }

        void Simulator::remFromElementList(eElement* el)
        {
            if (m_elementList.contains(el)) m_elementList.removeOne(el);
        }

        void Simulator::addToUpdateList(eElement* el)
        {
            if (!m_updateList.contains(el)) m_updateList.append(el);
        }

        void Simulator::remFromUpdateList(eElement* el)
        {
            m_updateList.removeOne(el);
        }

        void Simulator::addToSimuClockList(eElement* el)
        {
            if (!m_simuClock.contains(el)) m_simuClock.append(el);
        }

        void Simulator::remFromSimuClockList(eElement* el)
        {
            m_simuClock.removeOne(el);
        }

        void Simulator::addToChangedFast(eElement* el)
        {
            if (!m_changedFast.contains(el)) m_changedFast.append(el);
        }

        void Simulator::remFromChangedFast(eElement* el)
        {
            m_changedFast.removeOne(el);
        }

        void Simulator::addToReactiveList(eElement* el)
        {
            if (!m_reactive.contains(el)) m_reactive.append(el);
        }

        void Simulator::remFromReactiveList(eElement* el)
        {
            m_reactive.removeOne(el);
        }

        void Simulator::addToNoLinList(eElement* el)
        {
            if (!m_nonLinear.contains(el)) m_nonLinear.append(el);
        }

        void Simulator::remFromNoLinList(eElement* el)
        {
            m_nonLinear.removeOne(el);
        }

        void Simulator::addToMcuList(BaseProcessor* proc)
        {
            if (!m_mcuList.contains(proc)) m_mcuList.append(proc);
        }
        void Simulator::remFromMcuList(BaseProcessor* proc) { m_mcuList.removeOne(proc); }












        // Header
        public static Simulator Self() { return mself; }

        public void runContinuous();
        public void topTimer();
        public void resumeTimer();
        public void pauseSim();
        public void resumeSim();
        public void stopSim();
        public void stopDebug();
        public void startSim();
        public void debug(bool run);
        public void runGraphicStep();
        public void runExtraStep(uint64_t cycle);
        public void runGraphicStep1();
        public void runGraphicStep2();
        public void runCircuit();

        public int circuitRate() { return m_circuitRate; }
        public int simuRate() { return m_simuRate; }
        public int simuRateChanged(int rate);

        public void setTimerScale(int ts) { m_timerSc = ts; }

        public int reaClock();
        public void setReaClock(int value);

        public int noLinAcc();
        public void setNoLinAcc(int ac);
        public double NLaccuracy();

        public bool isRunning();
        public bool isPaused();

        public UInt64 step();
        public uint64_t circTime();
        public void setCircTime(uint64_t time);

        public QList<eNode*> geteNodes() { return m_eNodeList; }

        public void addToEnodeBusList(eNode* nod);
        public void remFromEnodeBusList(eNode* nod, bool del);

        public void addToEnodeList(eNode* nod);
        public void remFromEnodeList(eNode* nod, bool del);

        public void addToChangedNodeList(eNode* nod);
        public void remFromChangedNodeList(eNode* nod);

        public void addToElementList(eElement* el);
        public void remFromElementList(eElement* el);

        public void addToUpdateList(eElement* el);
        public void remFromUpdateList(eElement* el);

        public void addToChangedFast(eElement* el);
        public void remFromChangedFast(eElement* el);

        public void addToReactiveList(eElement* el);
        public void remFromReactiveList(eElement* el);

        public void addToSimuClockList(eElement* el);
        public void remFromSimuClockList(eElement* el);

        public void addToNoLinList(eElement* el);
        public void remFromNoLinList(eElement* el);

        public void addToMcuList(BaseProcessor* proc);
        public void remFromMcuList(BaseProcessor* proc);

        public void timerEvent(QTimerEvent* e);

        public double stepsPerus();

        public uint64_t stepsPerSec;

        public uint64_t mS() { return m_RefTimer.elapsed(); }


        //events
        public void OnPauseDebug();
        public void OnResumeDebug();
        public void OnRateChanged();


        private static Simulator mself=null;

        private inline void solveMatrix();

        private QFuture<void> CircuitFuture;

        private CircMatrix matrix;

        private QList<eNode*> eNodeList;
        private QList<eNode*> eChangedNodeList;
        private QList<eNode*> eNodeBusList;

        private QList<eElement*> elementList;
        private QList<eElement*> updateList;

        private QList<eElement*> changedFast;
        private QList<eElement*> reactive;
        private QList<eElement*> nonLinear;
        private QList<eElement*> simuClock;
        private QList<BaseProcessor*> mcuList;

        private bool isrunning;
        private bool debugging;
        private bool runMcu;
        private bool paused;
        private bool error;

        private int timerId;
        private int timerTick;
        private int timerSc;
        private int noLinAcc;
        private int numEnodes;
        private int simuRate;
        private int stepsPrea;

        private double stepsPerus;
        private double stepNS;
        private double mcuStepNS;

        private UInt64 circuitRate;
        private UInt64 reacCounter;
        private UInt64 updtCounter;

        private UInt64 circTime;
        private UInt64 step;
        private UInt64 tStep;
        private UInt64 lastStep;

        private UInt64 refTime;
        private UInt64 lastRefTime;
        private Timer refTimer;
    }
}
