using System;
using System.Collections.Generic;
using System.Text;

namespace OpenMacroInputComAPI
{

    public class CommandLinesSenderIPC
    {
        public Queue<string> m_commandsToSend = new Queue<string>();
        public TargetMemoryFileWithMutex m_targetMemoryFile;

        public CommandLinesSenderIPC(string fileName)
        {
            m_targetMemoryFile = new TargetMemoryFileWithMutex(fileName);
        }

        public void Add(string commandLine)
        {
            m_commandsToSend.Enqueue(commandLine);
        }
        public void PushCommandsWaiting()
        {

            if (m_commandsToSend.Count > 0)
            {


                string value = string.Join("\n", m_commandsToSend.ToArray());
                m_targetMemoryFile.AppendTextAtEnd(value);
                m_commandsToSend.Clear();
            }
        }

        public void AddAndPushWhenPossible(params string[] commandlines) {
            if (commandlines == null || commandlines.Length < 1)
                return;
            for (int i = 0; i < commandlines.Length; i++)
            {
                if(!string.IsNullOrEmpty(commandlines[i]))
                    Add(commandlines[i]);
            }
            PushCommandsWaiting();
        }
    }


}
