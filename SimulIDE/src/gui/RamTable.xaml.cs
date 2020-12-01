using SimulIDE.src.gui.editor;
using SimulIDE.src.simulator.elements.processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulIDE.src.gui
{
    /// <summary>
    /// Interaction logic for RamTable.xaml
    /// </summary>
    /// 
    public class RamTableValue
    {
        public string Reg { get; set; }
        public string Type { get; set; }
        public string Dec { get; set; }
        public string Value { get; set; }
    }

    public partial class RamTable : UserControl
    {
        public RamTable()
        {
            InitializeComponent();
            InitGrid(4, numRegs);
        }

        


        //# include "ramtable.h"
        //# include "baseprocessor.h"
        //# include "basedebugger.h"
        //# include "mainwindow.h"
        //# include "utils.h"

        public RamTable(BaseProcessor processor) : base()  //таблица 60*4
        {
            this.processor = processor;
            debugger = null;
            numRegs = 60;
            loadingVars = false;

            

             //   connect(this, SIGNAL(itemChanged(QTableWidgetItem *)),
             //            this, SLOT(addToWatch(QTableWidgetItem *)));
            }

        private DataGridTextColumn MakeColumn(string header,string field, int width)
        {
            DataGridTextColumn result = new DataGridTextColumn();
            result.Header = header;
            result.Binding = new Binding(field);
            result.Width = width;
            return result;
        }

        protected void InitGrid(int Cols, int Rows)
        {
            dataGrid.Columns.Clear();
            dataGrid.Columns.Add(MakeColumn("Reg","Reg",60));
            dataGrid.Columns.Add(MakeColumn("Type", "Type", 55));
            dataGrid.Columns.Add(MakeColumn("Dec", "Dec", 35));
            dataGrid.Columns.Add(MakeColumn("Value", "Value", 80));
            dataGrid.Items.Clear();
            for (int row = 0; row < Rows; row++)
                dataGrid.Items.Add(new RamTableValue() { Reg = "---", Type = "---", Dec = "---", Value = "---" });
        }


   



//            private void RamTable::slotContextMenu( const QPoint& point )
//{
//    QMenu menu;
//    if(m_debugger )
//    {
//        QAction* loadVars = menu.addAction(QIcon(":/open.png"), tr("Load Variables"));
//            connect(loadVars, SIGNAL(triggered()), this, SLOT(loadVariables()) );
//    }

        //        QAction* clearSelected = menu.addAction(QIcon(":/remove.png"), tr("Clear Selected"));
        //        connect(clearSelected, SIGNAL(triggered()), this, SLOT(clearSelected()) );

        //        QAction *clearTable = menu.addAction( QIcon(":/remove.png"),tr("Clear Table") );
        //        connect( clearTable, SIGNAL(triggered()), this, SLOT(clearTable()) );

        //        menu.addSeparator();

        //        QAction *loadVarSet = menu.addAction( QIcon(":/open.png"),tr("Load VarSet") );
        //        connect( loadVarSet, SIGNAL(triggered()), this, SLOT(loadVarSet()) );

        //        QAction *saveVarSet = menu.addAction( QIcon(":/save.png"),tr("Save VarSet") );
        //        connect( saveVarSet, SIGNAL(triggered()), this, SLOT(saveVarSet()) );

        //        menu.exec( mapToGlobal(point) );
        //        }

        public void ClearSelected()
        {
  //          for (QTableWidgetItem* item : selectedItems()) item->setData(0, "");
        }

        public void ClearTable()
        {
 //           for (QTableWidgetItem* item : findItems("*", Qt::MatchWildcard))
//            { if (item) item->setData(0, ""); }
        }

        public void LoadVarSet()
        {
            loadingVars = true;

 //           string dir = processor.GetFileName();

  //          string fileName = QFileDialog::getOpenFileName(this, tr("Load VarSet"), dir, tr("VarSets (*.vst);;All files (*.*)"));

 //           if (fileName!="")
//            {
            //    List<string> varSet = fileToStringList(fileName, "RamTable::loadVarSet");

            //    this.SetCurrentCell(0, 0);
            //    varSet.takeFirst(); // First is repeated.. why??

            //    int row = currentRow() - 1;
            //    for (QString var : varSet)
            //    {
            //        row++;
            //        if (row >= m_numRegs) break;
            //        item(row, 0)->setText(var);
            //    }
            //    this->setCurrentCell(0, 0);
            //}
            //m_loadingVars = false;
        }

        public void SaveVarSet()
        {
        //    const QString dir = m_processor->getFileName();
        //    //QCoreApplication::applicationDirPath()+"/data/varset";

        //    QString fileName = QFileDialog::getSaveFileName(this, tr("Save VarSet"), dir,
        //                                                 tr("VarSets (*.vst);;All files (*.*)"));
        //    if (!fileName.isEmpty())
        //    {
        //        if (!fileName.endsWith(".vst")) fileName.append(".vst");

        //        QFile file(fileName );

        //        if (!file.open(QFile::WriteOnly | QFile::Text))
        //        {
        //            QMessageBox::warning(0l, "RamTable::saveVarSet",
        //            tr("Cannot write file %1:\n%2.").arg(fileName).arg(file.errorString()));
        //            return;
        //        }

        //        QTextStream out(&file);
        //out.setCodec("UTF-8");
        //        QApplication::setOverrideCursor(Qt::WaitCursor);

        //        for (int row = 0; row < m_numRegs; row++)
        //        {
        //            QString name = item(row, 0)->text();
        //    out << name << "\n";
        //        }
        //        file.close();
        //        QApplication::restoreOverrideCursor();
        //    }
        }

        public void LoadVariables()
        {
            if (debugger==null) return;
            loadingVars = true;
            List<string> variables = debugger.GetVarList();
            foreach (string variable in variables)
            {
                int row = dataGrid.SelectedIndex + 1;
                if (row >= numRegs) break;
                ((RamTableValue)dataGrid.Items[row]).Reg=variable;
            }
            loadingVars = false;
        }

        public void UpdateValues()
        {
            if (processor!=null)
            {
                foreach (int _row in watchList.Keys)
                {
                    currentRow = _row;
                    string name = watchList[_row];
                    bool ok;
                    ok = int.TryParse(name, System.Globalization.NumberStyles.Integer,null,out int addr);
                    if (!ok)
                        ok = int.TryParse(name, System.Globalization.NumberStyles.HexNumber, null, out addr);

                    if (!ok) processor.UpdateRamValue(name);  // Var or Reg name
                    else                                            // Address
                    {
                        int value = processor.GetRamValue(addr);

                        if (value >= 0)
                        {
                            ((RamTableValue)dataGrid.Items[_row]).Type = "uint8";
                            ((RamTableValue)dataGrid.Items[_row]).Dec = value.ToString();
                            ((RamTableValue)dataGrid.Items[_row]).Value = Utils.DecToBase(value,2,8);
                        }
                    }
                }
            }
        }

        public void SetItemValue(int col, string value)
        {
            var item = ((RamTableValue)dataGrid.Items[currentRow]);
            switch (col)
            {
                case 0: item.Reg = value;break;
                case 1: item.Type = value; break;
                case 2: item.Dec = value; break;
                case 3: item.Value = value; break;
                default: break;
            }

        }

        public void SetItemValue(int col, float value)
        {
            SetItemValue(col, value.ToString());
        }

        public void SetItemValue(int col, Int32 value)
        {
            SetItemValue(col, value.ToString());
        }

        protected void AddToWatch(RamTableValue it)
        {
            //if (column(it) != 0) return;
            //int _row = row(it);
            //setCurrentCell(_row, 0);

            //QString name = it->text().remove(" ").remove("\t").remove("*");//.toLower();

            //if (name.isEmpty())
            //{
            //    watchList.remove(_row);
            //    verticalHeaderItem(_row)->setText("---");

            //    item(_row, 3)->setText("---");
            //    item(_row, 2)->setText("---");
            //    item(_row, 1)->setText("---");
            //}
            //else
            //{
            //    int value = m_processor->getRegAddress(name);
            //    if (value < 0)
            //    {
            //        bool ok;
            //        value = name.toInt(&ok, 10);
            //        if (!ok) value = name.toInt(&ok, 16);
            //        if (!ok) value = -1;
            //    }
            //    if (value >= 0)
            //    {
            //        watchList[_row] = name;
            //        verticalHeaderItem(_row)->setData(0, value);
            //    }
            //    if (!m_debugger) return;
            //    QString varType = m_debugger->getVarType(name);

            //    if (!m_loadingVars && varType.contains("array"))
            //    {
            //        int size = varType.replace("array", "").toInt();

            //        QStringList variables = m_debugger->getVarList();

            //        int indx = variables.indexOf(name);
            //        int listEnd = variables.size() - 1;
            //        for (int i = 1; i < size; i++)
            //        {
            //            int index = indx + i;
            //            if (index > listEnd) break;

            //            QString varName = variables.at(index);
            //            if (varName.contains(name)) item(_row + i, 0)->setText(varName);
            //        }
            //    }
            //}
        }

        public void SetDebugger(BaseDebugger deb)
        {
            debugger = deb;
        }

        public void RemDebugger(BaseDebugger deb)
        {
            if (debugger == deb) debugger = null;
        }

        protected BaseProcessor processor;
        protected BaseDebugger debugger;
        protected Dictionary<int, string> watchList = new Dictionary<int, string>();
        protected bool loadingVars;
        protected int numRegs;
        protected int currentRow;

    }
}
