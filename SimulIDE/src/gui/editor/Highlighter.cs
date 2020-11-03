﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SimulIDE.src.gui.editor
{
    class Highlighter
    {


//           : QSyntaxHighlighter(parent )
//        {
//            m_multiline = false;

//            multiLineCommentFormat.setForeground(Qt::red);
//            multiLineCommentFormat.setFontItalic(true);

//            commentStartExpression = QRegExp("/\\*");
//            commentEndExpression = QRegExp("\\*/");
//        }
//        Highlighter::~Highlighter() { }

//        void Highlighter::readSintaxFile( const QString &fileName )
//{
//    m_highlightingRules.clear();

//    QTextCharFormat format;

//        QStringList keys;
//        QStringList text = fileToStringList(fileName, "Highlighter");

//    while( !text.isEmpty() )                        // Iterate trough lines
//    {
//        QString line = text.takeFirst().toLower();
//        if(line.isEmpty() ) continue;

//        if(line.startsWith("keywords:") )          // Find KeyWord List
//        {
//            keys = line.split(" ");
//            keys.removeFirst();
//            keys.removeAll(" ");
//            keys.removeAll("");
//            continue;
//        }

//    QStringList allWords = line.split(" ");
//    allWords.removeAll(" ");
//        allWords.removeAll("");
//        if(allWords.isEmpty() ) continue;

//        for(QString key : keys )
//        {
//            QStringList words = allWords;
//    QString first = words.takeFirst();

//            if( !first.startsWith(key ) ) continue; // Nothing found

//            // Found Keyword
//            if(first.remove(key) == "-style:")     // Found Style definition
//            {
//                bool ok = false;

//    first = words.takeFirst();          // Foregraund color
//                if(first != "default" )
//                {
//                    uint color = first.remove("#").toUInt(&ok, 16);
//                    if(ok ) format.setForeground(QColor(color) );
//}
//first = words.takeFirst();          // Backgraund color
//                if(first != "default" )
//                {
//                    uint color = first.remove("#").toUInt(&ok, 16);
//                    if(ok ) format.setBackground(QColor(color) );
//                }
//                first = words.takeFirst();          // Bold?
//                if(first == "true" ) format.setFontWeight(QFont::Bold );

//                first = words.takeFirst();          // Italic?
//                if(first == "true" ) format.setFontItalic( true );
//            }
//            else                                    // Is Keyword List
//            {
//                for(QString exp : words )
//                {
//                    if(exp.startsWith("\"")) // RegExp
//                        exp = exp.remove(0, 1).remove(exp.lastIndexOf("\""), 1);
//                    else exp = "\\b"+exp+"\\b";
//                    addRule(format, exp );
//                }
//                format.setFontWeight(QFont::Normal );         // Reset to Defaults
//                format.setForeground(Qt::black );             // Reset to Defaults
//            }
//            break;
//        }
//    }
//    format.setForeground(QColor(12303291) ); // Show Spaces color
//    addRule(format, QString( " " ) );
//    addRule(format, QString( "\t" ) );


//    this->rehighlight();
//}

//void Highlighter::addRegisters(QStringList patterns)
//{
//    QTextCharFormat format;
//    format.setFontWeight(QFont::Bold);
//    format.setForeground(QColor(55, 65, 20));

//    for (QString exp : patterns) addRule(format, "\\b" + exp + "\\b");
//    //addRuleSet( format, patterns );
//    this->rehighlight();
//}

//void Highlighter::highlightBlock( const QString &text )
//{
//    QString lcText = text;
//    lcText = lcText.toLower(); // Do case insensitive

//    for ( const HighlightingRule &rule : m_highlightingRules )
//    {
//        processRule(rule, lcText);
//    }

//    if (m_multiline)                              // Multiline comment:
//    {
//        setCurrentBlockState(0);
//        int startIndex = 0;
//        if (previousBlockState() != 1)
//            startIndex = commentStartExpression.indexIn(text);

//        while (startIndex >= 0)
//        {
//            int endIndex = commentEndExpression.indexIn(text, startIndex);
//            int commentLength;
//            if (endIndex == -1)
//            {
//                setCurrentBlockState(1);
//                commentLength = text.length() - startIndex;
//            }
//            else
//            {
//                commentLength = endIndex - startIndex + commentEndExpression.matchedLength();
//            }
//            setFormat(startIndex, commentLength, multiLineCommentFormat);
//            startIndex = commentStartExpression.indexIn(text, startIndex + commentLength);
//        }
//    }
//}

//void Highlighter::processRule(HighlightingRule rule, QString lcText)
//{
//    QRegExp expression(rule.pattern );
//    int index = expression.indexIn(lcText);
//    while (index >= 0)
//    {
//        int length = expression.matchedLength();
//        setFormat(index, length, rule.format);
//        index = expression.indexIn(lcText, index + length);
//    }
//}

///*void Highlighter::addRuleSet( QTextCharFormat format, QStringList regExps )
//{
//    for( QString exp : regExps ) addRule( format, exp );
//}*/

//void Highlighter::addRule(QTextCharFormat format, QString exp)
//{
//    HighlightingRule rule;

//    rule.pattern = QRegExp(exp);
//    rule.format = format;
//    m_highlightingRules.append(rule);
//}

//void Highlighter::setMultiline(bool set)
//{
//    m_multiline = set;
//}








        

//        //void addRuleSet( QTextCharFormat, QString );

        


//        private QRegExp commentStartExpression;
//        private QRegExp commentEndExpression;

//        private QTextCharFormat keyword1Format;
//        private QTextCharFormat registerFormat;
////QTextCharFormat classFormat;
//        private QTextCharFormat lineCommentFormat;
//        private QTextCharFormat preprocessorFormat;
//        private QTextCharFormat dataTypeFormat;
//        private QTextCharFormat numberFormat;
//        private QTextCharFormat multiLineCommentFormat;
//        private QTextCharFormat quotationFormat;
//        private QTextCharFormat htmlTagFormat;
//        private QTextCharFormat functionFormat;

}

    public struct HighlightingRule
    {
        //RegExp pattern;
        //TextCharFormat format;
    }
}
