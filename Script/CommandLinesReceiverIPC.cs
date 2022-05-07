using System;
using System.Collections.Generic;
using System.Text;

namespace OpenMacroInputComAPI
{


    public class CommandLinesReceiverIPC
    {
        public long m_lastCheckSized = 0;
        public TargetMemoryFileWithMutex m_targetMemoryFile;

        public const long m_maxMemorySize = 1000000;


        public CommandLinesReceiverIPC(string fileName)
        {
            m_targetMemoryFile = new TargetMemoryFileWithMutex(fileName);
        }


      

        public void CheckForContent( out Queue<string> commandsFound, bool removeContentAfter)
        {
            commandsFound = new Queue<string>();
            string text = "";
            m_targetMemoryFile.TextRecovering(out text, removeContentAfter);
            m_lastCheckSized = text.Length;
            string[] d = text.ToString().Split('\n');
            for (int i = 0; i < d.Length; i++)
            {
                commandsFound.Enqueue(d[i]);
            }

        }

        public long GetLastCheckSize()
        {
            return m_lastCheckSized;
        }

        public double GetLastCheckSizeInPourcent()
        {
            return (double)m_lastCheckSized / (double)m_maxMemorySize;
        }
    }


}
