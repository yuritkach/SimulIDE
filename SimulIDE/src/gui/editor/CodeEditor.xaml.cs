using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using SimulIDE.src.gui.circuitwidget.components.mcu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using static SimulIDE.src.gui.editor.EditorPage;

namespace SimulIDE.src.gui.editor
{
    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor _editor;

        public HighlightCurrentLineBackgroundRenderer(TextEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Background; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null)
                return;

            textView.EnsureVisualLines();
            var currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset);
            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine))
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush(Color.FromArgb(0x40, 0, 0, 0xFF)), null,
                    new Rect(rect.Location, new Size(textView.ActualWidth, rect.Height)));
            }
        }
    }


    public class BreakpointMargin : AbstractMargin
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hit)
        {
            return new PointHitTestResult(this, hit.HitPoint);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(20, 00);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //
        }

    }

        /// <summary>
        /// Interaction logic for CodeEditor.xaml
        /// </summary>
        public partial class CodeEditor : ICSharpCode.AvalonEdit.TextEditor
    {
        public CodeEditor()
        {
            InitializeComponent();
        }

        public static readonly List<string> picInstr = new List<string>() {"addlw","addwf","andlw","andwf","bcf","bov","bsf","btfsc","btg","btfss",
            "clrf","clrw","clrwdt","comf","decf","decfsz","goto","incf","incfsz","iorlw","iorwf","movf","movlw","movwf","reset","retfie","retlw","return",
            "rlf","rrfsublw","subwf","swapf","xorlw","xorwf"};

        public static readonly List<string> avrInstr = new List<string>() {"add","adc","adiw","sub","subi","sbc","sbci","sbiw","andi","ori","eor","com",
            "neg","sbr","cbr","dec","tst","clr","ser","mul","rjmp","ijmp","jmp","rcall","icall","ret","reti","cpse","cp","cpc","cpi","sbrc","sbrs","sbic",
            "sbis","brbs","brbc","breq","brne","brcs","brcc","brsh","brlo","brmi","brpl","brge","brlt","brhs","brhc","brts","brtc","brvs","brvc","brie",
            "brid","mov","movw","ldi","lds","ld","ldd","sts","st","std","lpm","in","out","push","pop","lsl","lsr","rol","ror","asr","swap","bset","bclr",
            "sbi","cbi","bst","bld","sec","clc","sen","cln","sez","clz","sei","cli","ses","cls","sev","clv","set","clt","seh","clh","wdr" };

        bool showSpaces = false;
        bool spaceTabs  = false;
        bool driveCirc  = false;
        int fontSize = 13;
        int tabSize = 4;
        public string FileName { get; set; }
        //Font font;

        public CodeEditor(ref DockPanel parent, TextBox outPane): base()
        {

            parent.Children.Add(this);
            Name = "Editor";
            this.outPane = outPane;
            //appPath   = QCoreApplication::applicationDirPath();

            debugger = null;
            debugLine = 0;
            brkAction = 0;
            state = DBG_STOPPED;
            running   = false;
            isCompiled = false;
            debugging = false;
            stepOver = false;
            driveCirc = false;
            ShowLineNumbers = true;
            SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C++");

            TextArea.TextView.BackgroundRenderers.Add(
                new HighlightCurrentLineBackgroundRenderer(this));
            TextArea.Caret.PositionChanged += (sender, e) =>
                TextArea.TextView.InvalidateLayer(KnownLayer.Background);

            BreakpointMargin z = new BreakpointMargin();
            z.MouseLeftButtonDown += BreakpointArea_MouseLeftButtonDown;

            TextArea.LeftMargins.Insert(0, z);


            //            QSettings* settings = MainWindow::self()->settings();


            bool spacesTab = false;
//            if (settings->contains("Editor_spaces_tabs"))
//                spacesTab = settings->value("Editor_spaces_tabs").toBool();

//            setSpaceTabs(spacesTab);

//            QPalette p = palette();
//            p.setColor(QPalette::Base, QColor(255, 255, 249));
//            p.setColor(QPalette::Text, QColor(0, 0, 0));
//            setPalette(p);

//            connect(this, SIGNAL(blockCountChanged(int)), this, SLOT(updateLineNumberAreaWidth(int)));
//            connect(this, SIGNAL(updateRequest(QRect, int)), this, SLOT(updateLineNumberArea(QRect, int)));

//            connect(Simulator::self(), SIGNAL(pauseDebug()), this, SLOT(pause()));
//            connect(Simulator::self(), SIGNAL(resumeDebug()), this, SLOT(resume()));

//            setLineWrapMode(QPlainTextEdit::NoWrap);
//            UpdateLineNumberAreaWidth(0);
        }

        private void BreakpointArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //brkPoints.Add()
        }

        ~CodeEditor()
        {
//            QPropertyEditorWidget::self()->removeObject(this);
//            if (debugger!=null) QPropertyEditorWidget::self()->removeObject(m_debugger);
        }

        public bool IsModified { get; set; }

        public event MyEventHandler OnDocumentChanged;
        public string GetPlainText()
        {

            return Document.Text;
            
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            IsModified = true;
            OnDocumentChanged?.Invoke(this);
        }

        public void SetFile( string filePath )
        {
            isCompiled= false;
            if(file == filePath ) return;
            debugger = null;
            outPane.AppendText( "-------------------------------------------------------\n" );
            outPane.AppendText(" File: ");
            outPane.AppendText(filePath );
            outPane.AppendText( "\n\n" );
            file = System.IO.Path.GetFileName(filePath);
            fileDir = System.IO.Path.GetDirectoryName(filePath);
            fileExt  = System.IO.Path.GetExtension(filePath);
            fileName = System.IO.Path.GetFileNameWithoutExtension(file);

            //    QDir::setCurrent(m_file );

//            string sintaxPath = SIMUAPI_AppPath::self()->availableDataFilePath("codeeditor/sintax/");
            //    if(m_file.endsWith(".gcb") )
            //    {
            //        //m_appPath+"/data/codeeditor/gcbasic.sintax");
            //        QString path = sintaxPath + "gcbasic.sintax";
            //    m_hlighter->readSintaxFile(path );

            //    debugger = new GcbDebugger(this, m_outPane, filePath );
            //}
            //    else 
            if (file.EndsWith(".cpp") || file.EndsWith(".c") || file.EndsWith(".ino") || file.EndsWith(".h"))
            {
                //m_appPath+"/data/codeeditor/cpp.sintax"
//                string path = sintaxPath + "cpp.sintax";
//                hlighter.SetMultiline( true );
//                hlighter.ReadSintaxFile(path );

                if(file.EndsWith(".ino") )
                    debugger = new InoDebugger(this, outPane, filePath );
            }
        //    else if(m_file.endsWith(".asm") )
        //    {
        //        // We should identify if pic or avr asm
        //        int isPic = 0;
        //int isAvr = 0;

        //isPic = getSintaxCoincidences(m_file, m_picInstr );

        //        if(isPic<50 ) isAvr = getSintaxCoincidences(m_file, m_avrInstr);

        //        m_outPane->writeText(tr("File recognized as: ") );

        //        if(isPic > isAvr )   // Is Pic
        //        {
        //            m_outPane->writeText( "Pic asm\n" );

        //QString path = sintaxPath + "pic14asm.sintax";
        //m_hlighter->readSintaxFile(path );

        //debugger = new PicAsmDebugger(this, m_outPane, filePath );
        //        }
        //        else if(isAvr > isPic )  // Is Avr
        //        {
        //            m_outPane->writeText( "Avr asm\n" );

        //QString path = sintaxPath + "avrasm.sintax";
        //m_hlighter->readSintaxFile(path );

        //debugger = new AvrAsmDebugger(this, m_outPane, filePath );
        //        }
        //        else m_outPane->writeText( "Unknown\n" );
        //    }
        //    else if(m_file.endsWith(".xml") 
        //         ||  m_file.endsWith(".package") 
        //         ||  m_file.endsWith(".subcircuit") )
        //    {
        //        QString path = sintaxPath + "xml.sintax";
        //m_hlighter->readSintaxFile(path );
        //    }
        //    else if(m_file.endsWith("Makefile") 
        //         ||  m_file.endsWith("makefile") )
        //    {
        //        QString path = sintaxPath + "makef.sintax";
        //m_hlighter->readSintaxFile(path );
        //    }
        //    else if(m_file.endsWith(".sac") )
        //    {
        //        debugger = new B16AsmDebugger(this, m_outPane, filePath );
        //    }
        }

        //int CodeEditor::getSintaxCoincidences(QString& fileName, QStringList& instructions )
        //{
        //    QStringList lines = fileToStringList(fileName, "CodeEditor::getSintaxCoincidences");

        //    int coincidences = 0;

        //    for (QString line : lines)
        //    {
        //        if (line.isEmpty()) continue;
        //        if (line.startsWith("#")) continue;
        //        if (line.startsWith(";")) continue;
        //        if (line.startsWith(".")) continue;
        //        line = line.toLower();

        //        for (QString instruction : instructions)
        //        {
        //            if (line.contains(QRegExp("\\b" + instruction + "\\b")))
        //                coincidences++;

        //            if (coincidences > 50) break;
        //        }
        //    }
        //    return coincidences;
        //}

        public string GetFilePath()
        {
           return file;
        }

        public void SetCompilerPath()
        {
//            if (debugger!=null) debugger.GetCompilerPath();
//            else
            {
        //        if (m_fileExt == "") MessageBoxNB("CodeEditor::setCompilerPath",
        //                                       tr("Please save the Document first"));
        //        else MessageBoxNB("CodeEditor::setCompilerPath",
        //                       tr("No Compiler available for: %1 files").arg(m_fileExt));
            }
        }

        public void Compile()
        {
            if (IsModified) EditorPage.Self().Save();
            debugLine = -1;
            //Update();  //TYV

            int error = -2;
            isCompiled = false;

            outPane.AppendText("-------------------------------------------------------\n");
            outPane.AppendText("Exec: ");

            if (file.EndsWith("Makefile")
                || file.EndsWith("makefile"))          // Is a Makefile, make it
            {
                outPane.AppendText("make " + file + "\n");

                Process makeproc = new Process();
                makeproc.StartInfo.WorkingDirectory=fileDir;
                makeproc.StartInfo.FileName= "make";
                makeproc.StartInfo.UseShellExecute = false;
                makeproc.StartInfo.CreateNoWindow = false;
                makeproc.StartInfo.RedirectStandardOutput = true;
                makeproc.Start();
                string stdout = makeproc.StandardOutput.ReadToEnd();
                string stderr = makeproc.StandardError.ReadToEnd();
                makeproc.WaitForExit();
                makeproc.Close();

                outPane.AppendText(stderr);
                outPane.AppendText("\n");
                outPane.AppendText(stdout);
                outPane.AppendText("\n\n");

                if (stderr.ToUpper().Contains("ERROR") || stdout.ToUpper().Contains("ERROR"))
                {
                    error = -1;
                }
                else error = 0;
            }
            else
            {
                if (debugger == null)
                {
                    outPane.AppendText("\nFile type not supported\n");
                    return;
                }
                error = debugger.Compile();
            }

            if (error == 0)
            {
                outPane.AppendText("\n     SUCCESS!!! Compilation Ok\n");
                isCompiled = true;
            }
            else
            {
                outPane.AppendText("\n     ERROR!!! Compilation Failed\n");

                if (error > 0) // goto error line number
                {
                    debugLine = error; // Show arrow in error line
                    UpdateScreen();
                }
            }
        }

        public void Upload()
        {
        //    if (m_file.endsWith(".hex"))     // is an .hex file, upload to proccessor
        //    {
        //        //m_outPane->writeText( "-------------------------------------------------------\n" );
        //        m_outPane->appendText("\n" + tr("Uploading: ") + "\n");
        //        m_outPane->appendText(m_file);
        //        m_outPane->writeText("\n\n");

        //        if (McuComponent::self()) McuComponent::self()->load(m_file);
        //        return;
        //    }
        //    if (!m_isCompiled) compile();
        //    if (!m_isCompiled) return;
        //    if (debugger) debugger->upload();
        }

        public void AddBreakPoint(int line)
        {
            if (!debugging) return;
            int validLine = debugger.GetValidLine(line);
            if (!brkPoints.Contains(line)) brkPoints.Add(validLine);
        }

        public void RemBreakPoint(int line) { brkPoints.RemoveAt(line); }

        public void Run()
        {
        //    if (m_state == DBG_RUNNING) return;

        //    if (!m_driveCirc) Simulator::self()->stopTimer();

        //    m_state = DBG_RUNNING;

        //    timerTick();
        }

        public void Step(bool over)
        {
        //    if (m_state == DBG_RUNNING) return;

        //    m_stepOver = over;

        //    if (over)
        //    {
        //        addBreakPoint(m_debugLine + 1);
        //        EditorWindow::self()->run();
        //    }
        //    else
        //    {
        //        if (!m_driveCirc) Simulator::self()->stopTimer();
        //        m_prevDebugLine = m_debugLine;
        //        m_state = DBG_STEPING;
        //        runClockTick();
        //    }
        //    //updateScreen();
        }

        public void StepOver()
        {
        //    QList<int> subLines = debugger->getSubLines();
        //    bool over = false;
        //    if (subLines.contains(m_debugLine)) over = true;
        //    //qDebug() << "CodeEditor::stepOver()"<<over;
        //    step(over);
        }

        //void CodeEditor::runClockTick()
        //{
        //    if (!m_debugging) return;
        //    if (m_state == DBG_PAUSED) return;

        //    uint64_t time0 = Simulator::self()->mS();
        //    int i = 0;
        //    for (i = 0; i < 200000; i++)
        //    {
        //        m_debugLine = debugger->step();

        //        if (m_debugLine >= 0) break;                // New Line reached

        //        if (Simulator::self()->mS() - time0 > 100)
        //            break; // Avoid blocking GUI
        //    }
        //    //qDebug() <<"m_prevDebugLine "<<m_prevDebugLine<< "  m_debugLine "<<m_debugLine;

        //    if (m_debugLine < 0)                           // Step Not Finished
        //    {
        //        QTimer::singleShot(5, this, SLOT(runClockTick()));
        //    }
        //    else                                            // Step Finished
        //    {
        //        if (m_driveCirc) Simulator::self()->runGraphicStep1();
        //        EditorWindow::self()->pause();
        //        if (!m_driveCirc) Simulator::self()->resumeTimer();
        //    }
        //    if (m_debugLine > 0) m_prevDebugLine = m_debugLine;
        //    else m_debugLine = m_prevDebugLine;
        //}

        //void CodeEditor::timerTick()
        //{
        //    if (m_state != DBG_RUNNING)
        //    {
        //        if (m_stepOver)
        //        {
        //            m_stepOver = false;
        //            m_brkPoints.takeLast();
        //        }
        //        return;
        //    }

        //    m_prevDebugLine = m_debugLine;
        //    for (int i = 0; i < 200000; i++)
        //    {
        //        m_debugLine = m_debugger->step();

        //        if ((m_prevDebugLine != m_debugLine) && m_brkPoints.contains(m_debugLine))
        //        {
        //            pause();
        //            break;
        //        }
        //        if (m_debugLine > 0) m_prevDebugLine = m_debugLine;
        //        else m_debugLine = m_prevDebugLine;
        //    }
        //    if (m_state == DBG_RUNNING) QTimer::singleShot(5, this, SLOT(timerTick()));
        //    else
        //    {
        //        if (!m_driveCirc) Simulator::self()->resumeTimer();
        //        EditorWindow::self()->pause();

        //        if (m_stepOver)
        //        {
        //            m_stepOver = false;
        //            m_brkPoints.takeLast();
        //        }
        //        updateScreen();
        //    }
        //    //if( !m_stepOver ) updateScreen();
        //}

        public bool InitDebbuger()
        {
            outPane.AppendText("-------------------------------------------------------\n");
            outPane.AppendText("Starting Debbuger..." + "\n");

            bool error = false;

            if (McuComponent.Self()==null)             // Must be an Mcu in Circuit
            {
                outPane.AppendText("\n   Error: No Mcu in Simulator... \n");
                error = true;
            }
            else if (debugger==null)             // No debugger for this file type
            {
                outPane.AppendText("\n    Error: No Debugger Suited for this File... \n");
                error = true;
            }
            else if (file == "")                                   // No File
            {
                outPane.AppendText("\n    Error: No File... \n");
                error = true;
            }
            //////////else if( !m_isCompiled ) 
            {
                Compile();
                if (!isCompiled)                           // Error compiling
                {
                    outPane.AppendText("\n    Error Compiling... \n");
                    error = true;
                }
            }
            outPane.AppendText("\n");
            if (error)
            {
                StopDebbuger();
            }
            else                                          // OK: Start Debugging
            {
                if (!debugger.LoadFirmware())      // Error Loading Firmware
                {
                    outPane.AppendText("\n    Error Loading Firmware... \n");
                    StopDebbuger();
                }
                else
                {
                    if (debugger.type == 1) EditorPage.Self().EnableStepOver(true);
                    else EditorPage.Self().EnableStepOver(false);

                    debugging = true;
                    Reset();

                    /*if( Simulator::self()->isRunning() ) Simulator::self()->stopSim();

                    if( m_driveCirc ) CircuitWidget::self()->powerCircDebug( false );
                    else
                    {
                        BaseProcessor::self()->setSteps( 0 );
                        CircuitWidget::self()->powerCircDebug( true );
                    }*/
                    //TYV SetDriveCirc(m_driveCirc);

                    outPane.AppendText("Debugger Started \n");
                    //TYV SetReadOnly(true);
                }
            }
            return debugging;
        }

        public void StopDebbuger()
        {
        //    if (m_debugging)
        //    {
        //        m_debugger->stop();
        //        m_brkPoints.clear();
        //        m_debugLine = m_prevDebugLine = 0;

        //        CircuitWidget::self()->powerCircOff();
        //        Simulator::self()->stopDebug();

        //        m_state = DBG_STOPPED;
        //        m_debugging = false;
        //        setReadOnly(false);
        //        updateScreen();
        //    }
        //    m_outPane->writeText(tr("Debugger Stopped ") + "\n");
        }

        public void Pause()
        {
        //    if (!m_debugging) return;
        //    //if( !m_running ) return;

        //    m_resume = m_state;
        //    m_state = DBG_PAUSED;
        //    updateScreen();
        }

        //void CodeEditor::resume()
        //{
        //    if (!m_debugging) return;

        //    if (!BaseProcessor::self())
        //    {
        //        m_outPane->writeText(tr("\nError:  Mcu Deleted while Debugging!!\n"));
        //        EditorWindow::self()->stop();
        //    }
        //    else
        //    {
        //        m_state = m_resume;
        //        if (!m_driveCirc) Simulator::self()->resumeTimer();
        //    }
        //    updateScreen();
        //}

        public void Reset()
        {
        //    if (m_state == DBG_RUNNING) pause();

        //    McuComponent::self()->reset();
        //    m_debugLine = 1; //m_debugger->getProgramStart();

        //    updateScreen();
        }

        //bool CodeEditor::driveCirc()
        //{
        //    return m_driveCirc;
        //}

        //void CodeEditor::setDriveCirc(bool drive)
        //{
        //    m_driveCirc = drive;

        //    if (m_debugging)
        //    {
        //        if (Simulator::self()->isRunning()) Simulator::self()->stopSim();

        //        CircuitWidget::self()->powerCircDebug(!m_driveCirc);
        //    }
        //}

        public void UpdateScreen()
        {
        //    setTextCursor(QTextCursor(document()->findBlockByLineNumber(m_debugLine - 1)));
        //    ensureCursorVisible();
        //    Update();
        }

        //int CodeEditor::lineNumberAreaWidth()
        //{
        //    int digits = 1;
        //    int max = qMax(1, blockCount());
        //    while (max >= 10) { max /= 10; ++digits; }
        //    return fontMetrics().height() + fontMetrics().width(QLatin1Char('9')) * digits;
        //}

        //void CodeEditor::updateLineNumberAreaWidth(int /* newBlockCount */ )
        //{
        //    setViewportMargins(lineNumberAreaWidth(), 0, 0, 0);
        //}

        //void CodeEditor::updateLineNumberArea( const QRect &rect, int dy)
        //{
        //    if (dy) m_lNumArea->scroll(0, dy);
        //    else m_lNumArea->update(0, rect.y(), m_lNumArea->width(), rect.height());
        //    if (rect.contains(viewport()->rect())) updateLineNumberAreaWidth(0);
        //}

        //void CodeEditor::resizeEvent(QResizeEvent* e)
        //{
        //    QPlainTextEdit::resizeEvent(e);
        //    QRect cr = contentsRect();
        //    m_lNumArea->setGeometry(QRect(cr.left(), cr.top(), lineNumberAreaWidth(), cr.height()));
        //}


        //void CodeEditor::lineNumberAreaPaintEvent(QPaintEvent*event )
        //{
        //    QPainter painter(m_lNumArea );
        //    painter.fillRect( event->rect(), Qt::lightGray);

        //    QTextBlock block = firstVisibleBlock();

        //    int blockNumber = block.blockNumber();
        //    int top = (int)blockBoundingGeometry(block).translated(contentOffset()).top();
        //    int fontSize = fontMetrics().height();

        //    while (block.isValid() && top <= event->rect().bottom() )
        //    {
        //        int blockSize = (int)blockBoundingRect(block).height();
        //        int bottom = top + blockSize;

        //        if (block.isVisible() && bottom >= event->rect().top() )
        //        {
        //            int lineNumber = blockNumber + 1;
        //            // Check if there is a new breakpoint request from context menu
        //            int pos = m_lNumArea->lastPos;
        //            if (pos > top && pos < bottom)
        //            {
        //                if (m_brkAction == 1) addBreakPoint(lineNumber);
        //                else if (m_brkAction == 2) remBreakPoint(lineNumber);
        //                m_brkAction = 0;
        //                m_lNumArea->lastPos = 0;
        //            }
        //            // Draw breakPoint icon
        //            if (m_brkPoints.contains(lineNumber))
        //            {
        //                painter.setBrush(QColor(Qt::yellow));
        //                painter.setPen(Qt::NoPen);
        //                painter.drawRect(0, top, fontSize, fontSize);
        //            }
        //            // Draw debug line icon
        //            if (lineNumber == m_debugLine)
        //                painter.drawImage(QRectF(0, top, fontSize, fontSize), QImage(":/finish.png"));
        //            // Draw line number
        //            QString number = QString::number(lineNumber);
        //            painter.setPen(Qt::black);
        //            painter.drawText(0, top, m_lNumArea->width(), fontSize, Qt::AlignRight, number);
        //        }
        //        block = block.next();
        //        top = bottom;
        //        ++blockNumber;
        //    }
        //}

        //void CodeEditor::focusInEvent(QFocusEvent* event)
        //{
        //    QPropertyEditorWidget::self()->setObject(this);
        //    if (m_debugger) QPropertyEditorWidget::self()->addObject(m_debugger);
        //    QPlainTextEdit::focusInEvent( event );
        //}

        //int CodeEditor::fontSize()
        //{
        //    return m_fontSize;
        //}

        //void CodeEditor::setFontSize(int size)
        //{
        //    m_fontSize = size;
        //    m_font.setPixelSize(size);
        //    setFont(m_font);

        //    MainWindow::self()->settings()->setValue("Editor_font_size", QString::number(m_fontSize));

        //    setTabSize(m_tabSize);
        //}

        //int CodeEditor::tabSize()
        //{
        //    return m_tabSize;
        //}

        //void CodeEditor::setTabSize(int size)
        //{
        //    m_tabSize = size;
        //    setTabStopWidth(m_tabSize * m_fontSize * 2 / 3);

        //    MainWindow::self()->settings()->setValue("Editor_tab_size", QString::number(m_tabSize));

        //    if (m_spaceTabs) setSpaceTabs(true);
        //}

        //bool CodeEditor::showSpaces()
        //{
        //    return m_showSpaces;
        //}
        //void CodeEditor::setShowSpaces(bool on)
        //{
        //    m_showSpaces = on;

        //    QTextOption option = document()->defaultTextOption();

        //    if (on) option.setFlags(option.flags() | QTextOption::ShowTabsAndSpaces);

        //    else option.setFlags(option.flags() & ~QTextOption::ShowTabsAndSpaces);

        //    document()->setDefaultTextOption(option);

        //    if (m_showSpaces)
        //        MainWindow::self()->settings()->setValue("Editor_show_spaces", "true");
        //    else
        //        MainWindow::self()->settings()->setValue("Editor_show_spaces", "false");
        //}

        //bool CodeEditor::spaceTabs()
        //{
        //    return m_spaceTabs;
        //}

        //void CodeEditor::setSpaceTabs(bool on)
        //{
        //    m_spaceTabs = on;

        //    if (on)
        //    {
        //        m_tab = "";
        //        for (int i = 0; i < m_tabSize; i++) m_tab += " ";
        //    }
        //    else m_tab = "\t";

        //    if (m_spaceTabs)
        //        MainWindow::self()->settings()->setValue("Editor_spaces_tabs", "true");
        //    else
        //        MainWindow::self()->settings()->setValue("Editor_spaces_tabs", "false");
        //}

        //void CodeEditor::keyPressEvent(QKeyEvent* event )
        //{
        //    if ( event->key() == Qt::Key_Plus && (event->modifiers() & Qt::ControlModifier) )
        //    {
        //        setFontSize(m_fontSize + 1);
        //    }
        //    else if ( event->key() == Qt::Key_Minus && (event->modifiers() & Qt::ControlModifier) )
        //    {
        //        setFontSize(m_fontSize - 1);
        //    }
        //    else if ( event->key() == Qt::Key_Tab )
        //    {
        //        if (textCursor().hasSelection()) indentSelection(false);
        //        else insertPlainText(m_tab);
        //    }
        //    else if ( event->key() == Qt::Key_Backtab )
        //    {
        //        if (textCursor().hasSelection()) indentSelection(true);
        //        else
        //        {
        //            textCursor().movePosition(QTextCursor::PreviousCharacter, QTextCursor::MoveAnchor, m_tab.size());
        //        }
        //    }
        //    else 
        //    {
        //        int tabs = 0;
        //        if ( event->key() == Qt::Key_Return )
        //        {
        //            int n0 = 0;
        //            int n = m_tab.size();
        //            QString line = textCursor().block().text();

        //            while (1)
        //            {
        //                QString part = line.mid(n0, n);
        //                if (part == m_tab)
        //                {
        //                    n0 += n;
        //                    tabs += 1;
        //                }
        //                else break;
        //            }
        //        }
        //        QPlainTextEdit::keyPressEvent( event );

        //        if ( event->key() == Qt::Key_Return )
        //        {
        //            for (int i = 0; i < tabs; i++) insertPlainText(m_tab);
        //        }
        //    }
        //}

        /*void CodeEditor::increaseSelectionIndent()
        {
            QTextCursor curs = textCursor();

            // Get the first and count of lines to indent.

            int spos = curs.anchor();
            int epos = curs.position();

            if( spos > epos ) std::swap(spos, epos);

            curs.setPosition( spos, QTextCursor::MoveAnchor );
            int sblock = curs.block().blockNumber();

            curs.setPosition( epos, QTextCursor::MoveAnchor );
            int eblock = curs.block().blockNumber();

            // Do the indent.

            curs.setPosition( spos, QTextCursor::MoveAnchor );

            curs.beginEditBlock();

            for( int i = 0; i <= ( eblock-sblock); ++i )
            {
                curs.movePosition( QTextCursor::StartOfBlock, QTextCursor::MoveAnchor );

                curs.insertText( m_tab );

                curs.movePosition( QTextCursor::NextBlock, QTextCursor::MoveAnchor );
            }
            curs.endEditBlock();

            // Set our cursor's selection to span all of the involved lines.

            curs.setPosition(spos, QTextCursor::MoveAnchor);
            curs.movePosition(QTextCursor::StartOfBlock, QTextCursor::MoveAnchor );

            while( curs.block().blockNumber() < eblock )
            {
                curs.movePosition(QTextCursor::NextBlock, QTextCursor::KeepAnchor );
            }
            curs.movePosition( QTextCursor::EndOfBlock, QTextCursor::KeepAnchor );

            setTextCursor( curs );
        }*/

        //void CodeEditor::indentSelection(bool unIndent)
        //{
        //    QTextCursor cur = textCursor();
        //    int a = cur.anchor();
        //    int p = cur.position();

        //    cur.beginEditBlock();

        //    if (a > p) std::swap(a, p);

        //    QString str = cur.selection().toPlainText();
        //    QString str2 = "";
        //    QStringList list = str.split("\n");

        //    int lines = list.count();

        //    for (int i = 0; i < lines; i++)
        //    {
        //        QString line = list[i];

        //        if (unIndent)
        //        {
        //            int n = m_tab.size();
        //            int n1 = n;
        //            int n2 = 0;

        //            while (n1 > 0)
        //            {
        //                if (line.size() <= n2) break;
        //                QString car = line.at(n2);

        //                if (car == " ")
        //                {
        //                    n1 -= 1;
        //                    n2 += 1;
        //                }
        //                else if (car == "\t")
        //                {
        //                    n1 -= n;
        //                    if (n1 >= 0) n2 += 1;
        //                }
        //                else n1 = 0;
        //            }
        //            line.replace(0, n2, "");
        //        }
        //        else line.insert(0, m_tab);

        //        if (i < lines - 1) line += "\n";

        //        str2 += line;
        //    }
        //    cur.removeSelectedText();
        //    cur.insertText(str2);
        //    p = cur.position();

        //    cur.setPosition(a);
        //    cur.setPosition(p, QTextCursor::KeepAnchor);

        //    setTextCursor(cur);

        //    cur.endEditBlock();
        //}



        //// CLASS LineNumberArea ******************************************************
        //LineNumberArea::LineNumberArea(CodeEditor* editor ) : QWidget(editor)
        //{
        //    m_codeEditor = editor;
        //}
        //LineNumberArea::~LineNumberArea() { }

        //void LineNumberArea::contextMenuEvent(QContextMenuEvent*event)
        //{
        //    event->accept();

        //    if (!m_codeEditor->debugStarted()) return;

        //    QMenu menu;

        //    QAction* addm_brkAction = menu.addAction(QIcon(":/breakpoint.png"), tr("Add BreakPoint"));
        //    connect(addm_brkAction, SIGNAL(triggered()), m_codeEditor, SLOT(slotAddBreak()));

        //    QAction* remm_brkAction = menu.addAction(QIcon(":/nobreakpoint.png"), tr("Remove BreakPoint"));
        //    connect(remm_brkAction, SIGNAL(triggered()), m_codeEditor, SLOT(slotRemBreak()));

        //    if (menu.exec(event->globalPos()) != 0) lastPos = event->pos().y();
        //}

        
        protected BaseDebugger debugger;
        protected TextBox outPane;

        //QString m_appPath;
        protected string file;
        protected string fileDir;
        protected string fileName;
        protected string fileExt;

        protected string tab;

        protected List<int> brkPoints= new List<int>();

        protected int brkAction;    // 0 = no action, 1 = add brkpoint, 2 = rem brkpoint
        protected int debugLine;
        protected int prevDebugLine;
        protected int state;
        protected int resume;

        protected bool isCompiled;
        protected bool debugging;
        protected bool running;

        protected bool stepOver;
                
        static string font;

        static int DBG_STOPPED = 0;
        static int DBG_STEPING = 1;
        static int DBG_RUNNING = 2;
        static int DBG_PAUSED = 3;


        public bool HasDebugger()
        {
            return debugger != null;
        }

        public bool Compiled { get; set; }

    }

}

    
//    public:
//        CodeEditor(QWidget* parent, OutPanelText* outPane);
//    ~CodeEditor();

//    int fontSize();
//    void setFontSize(int size);

//    int tabSize();
//    void setTabSize(int size);

//    bool showSpaces();
//    void setShowSpaces(bool on);

//    bool spaceTabs();
//    void setSpaceTabs(bool on);

//    bool driveCirc();
//    void setDriveCirc(bool drive);

//    void setFile(const QString &filePath);
//    QString getFilePath();

//    void lineNumberAreaPaintEvent(QPaintEvent*event);
//    int lineNumberAreaWidth();

    

//    bool debugStarted() { return m_debugging; }
//    bool initDebbuger();
    

//    void setCompilerPath();

//    signals:
//        void msg(QString text);

//    public slots:
//        void stopDebbuger();
//    void slotAddBreak() { m_brkAction = 1; }
//    void slotRemBreak() { m_brkAction = 2; }
//    void timerTick();
//    void compile();
//    void upload();
//    void step(bool over = false);
//    void stepOver();
//    void pause();
//    void resume();
//    void reset();
//    void run();

//    protected:
//        void resizeEvent(QResizeEvent*event);
//    void focusInEvent(QFocusEvent* );
//    void keyPressEvent(QKeyEvent* event );

//    private slots:
//        void updateLineNumberAreaWidth(int newBlockCount);
//    void updateLineNumberArea( const QRect &, int );
//    void runClockTick();

//    private:
//        int getSintaxCoincidences(QString& fileName, QStringList& instructions );
//    void addBreakPoint(int line);
//    void remBreakPoint(int line);
//    void updateScreen();

//    void setupDebugTimer();

//    void indentSelection(bool unIndent);


//};


//// ********************* CLASS LineNumberArea **********************************

