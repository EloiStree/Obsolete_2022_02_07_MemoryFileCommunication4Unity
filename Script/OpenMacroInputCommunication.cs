using System;
using System.Collections.Generic;
using System.Text;

namespace OpenMacroInputComAPI
{
    public class OpenMacroInputCommunication
    {
        public CommandLinesSenderIPC m_sendCommands;
        public CommandLinesReceiverIPC m_recoveredLeftOverCommands;

        public Queue<string> m_sendCommandsWhenPossible = new Queue<string>();
        public Queue<string> m_leftOverCommands = new Queue<string>();


        public OpenMacroInputCommunication(string senderMemMapFileName= "CommandLinesStacker", string recoverMemMapFileName ="CommandLinesLeftover") {
            m_sendCommands = new CommandLinesSenderIPC(senderMemMapFileName);
            m_recoveredLeftOverCommands = new CommandLinesReceiverIPC(recoverMemMapFileName);
        }


        public void AllocateTimeToSendAndReceive() {

            m_sendCommands.AddAndPushWhenPossible(m_sendCommandsWhenPossible.ToArray());
            Queue<string> found= new Queue<string>();
            m_recoveredLeftOverCommands.CheckForContent(out found, false);
        }

    }


}
