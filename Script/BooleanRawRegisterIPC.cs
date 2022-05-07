using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OpenMacroInputComAPI
{
    public class BooleanRawRegisterIPC
    {
        BooleanRawRegister m_booleanRegister = new BooleanRawRegister(BooleanRawRegister.Size._4x4);

        public BooleanRawMemoryCompressor m_linkedRegisterCompressor ;
        TargetMemoryFileWithMutex m_booleanRegisterFileNameMMF;
        TargetMemoryFileWithMutex m_booleanRegisterFileValueMMF;

        public BooleanRawRegisterIPC(ref BooleanRawRegister register, string registerMemoryFileName = "DefaultBooleanRegister")
        {
            m_booleanRegister = register;
            m_linkedRegisterCompressor = new BooleanRawMemoryCompressor(ref register);
            
            m_booleanRegisterFileValueMMF = new TargetMemoryFileWithMutex(registerMemoryFileName + "BooleanValue");
            m_booleanRegisterFileNameMMF = new TargetMemoryFileWithMutex(registerMemoryFileName + "BooleanIndexName");

        }


        public void ImportRegisterStateFromMemoryValue()
        {

            ImportRegister_BooleanState();
            ImportRegister_BooleanIndexName();

        }
        public void ImportRegister_BooleanState()
        {
            string text ="";
            m_booleanRegisterFileValueMMF.TextRecovering(out text, false);
            m_linkedRegisterCompressor.SetWithBooleans(text);

        }
        public void ImportRegister_BooleanIndexName()
        {

            string text = "";
            m_booleanRegisterFileNameMMF.TextRecovering(out text, false);
            m_linkedRegisterCompressor.SetWithNames(text);


        }
        public void OverrideMemoryValueWithRegister()
        {
            OverrideMemoryValue_BooleanState();
            OverrideMemoryValue_BooleanIndexName();

        }
        public void OverrideMemoryValue_BooleanState()
        {
            string text = m_linkedRegisterCompressor.GetBooleansCompressed();
            m_booleanRegisterFileValueMMF.SetText(text);

        }
        public void OverrideMemoryValue_BooleanIndexName()
        {
            string text = m_linkedRegisterCompressor.GetIdsCompressed();
            m_booleanRegisterFileNameMMF.SetText(text);



        }
    }




    public class BooleanRawMemoryCompressor
    {

        public BooleanRawRegister m_registerTarget;

        public BooleanRawMemoryCompressor(ref BooleanRawRegister registerTarget)
        {
            m_registerTarget = registerTarget;
        }

        public string GetIdsCompressed()
        {
            StringBuilder sb = new StringBuilder();
            string[] names = m_registerTarget.m_booleanClaimedArray;
            if (names.Length == 0)
                return "";
            for (int i = 0; i < names.Length; i++)
            {
                sb.Append(names[i] == null ? "" : names[i]);
                if (i < names.Length - 1)
                    sb.Append('|');
            }
            return sb.ToString();
        }

        public string GetBooleansCompressed()
        {
            StringBuilder sb = new StringBuilder();
            bool[] boolvalue = m_registerTarget.m_booleanArray;
            for (int i = 0; i < boolvalue.Length; i++)
            {
                sb.Append(boolvalue[i]?'1':'0');
            }
            return sb.ToString();
        }
        public void SetWithBooleans(string booleanCompressed)
        {

            char[] c = booleanCompressed.ToString().Trim().ToCharArray();
            bool[] b = new bool[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                b[i] = c[i] == '1';
            }
            m_registerTarget.SetIndexValue(b);

        }
        public void SetWithNames(string booleanCompressed)
        {

            string [] tokens = booleanCompressed.Split('|');

            m_registerTarget.SetIndexValue(tokens);
        }
    }
}