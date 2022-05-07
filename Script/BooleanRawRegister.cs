using System;
using System.Collections.Generic;
using System.Text;

namespace OpenMacroInputComAPI
{
    public class BooleanRawRegisterNamedReference
    {
        public string m_name;
        public BooleanRawRegister m_booleanRegister;

        public BooleanRawRegisterNamedReference(string name, BooleanRawRegister booleanRegister)
        {
            this.m_name = name;
            this.m_booleanRegister = booleanRegister;
        }
    }

    /**
     The first version of my register was oriented object. But the complexity of it did not allow:
        - reference to a array by index that allow a fast access to the data
        - easy convertion to compute shader in unity
        - cast assembly of boolean without too much oop or programming
        - easy compression of data
     I know that I created this classe for those points and I know that they are weakness to it.
     If I am wrong on how to desing it feel free to ping me on discord: http://eloistree.page.link/discord
         
        */

    public class BooleanRawRegister
    {
        public enum Size : uint { _4x4 = 4 * 4, _16x16 = 16 * 16, _32x32 = 32 * 32, _64x64 = 64 * 64, _128x128 = 128 * 128, _256x256 = 256 * 256, _512x512 = 512 * 512, _1024x1024 = 1024 * 1024 }
        // 16 > 256 > 1024 > 4096 > 16.384 > 65.536 > 1.048.576

        public BooleanRawRegister(Size size = Size._32x32)
        {
            SetSize(size);
        }

        public Size m_size;
        private const uint m_defaultSize= (uint) Size._4x4;
        public uint m_currentlength = m_defaultSize;
        public bool[] m_booleanArray = new bool[m_defaultSize];
        public string[] m_booleanClaimedArray = new string[m_defaultSize];



        public void SetSize(Size size)
        {
            if (m_currentlength > (uint)size)
                throw new NotImplementedException("I did not implemented a code that can be size down");


            m_size = size;
            m_currentlength = (uint)size;

            bool[] ab = new bool[m_currentlength];
            string[] abs = new string[m_currentlength];

            for (int i = 0; i < m_booleanArray.Length; i++)
            {
                ab[i] = m_booleanArray[i];

            }
            for (int i = 0; i < m_booleanArray.Length; i++)
            {
                abs[i] = m_booleanClaimedArray[i];

            }

            m_booleanArray = ab;
            m_booleanClaimedArray = abs;
        }

        public uint GetCurrentMaxSize() { return m_currentlength; }


        public void SetIndexName(int index, string name)
        {
            if (index >= m_currentlength)
                SetSize(index+1);
            m_booleanClaimedArray[index] = name.Trim().ToLower();
        }

        public void SetIndexValue(int index, bool value)
        {
            if (index >= m_currentlength)
                SetSize(index+1);
            m_booleanArray[index] = value;
        }

        public bool IsDefined(uint index)
        {
            if (index >= m_currentlength)
                return false;
            if (string.IsNullOrEmpty(m_booleanClaimedArray[index]))
                return false;
            return true;
        }

        internal void SetIndexValue(bool[] booleans)
        {
            if (booleans.Length > m_currentlength)
                SetSize(booleans.Length);
            for (int i = 0; i < booleans.Length; i++)
            {
                SetIndexValue(i, booleans[i]);

            }
        }


        internal void SetIndexValue(string[] tokens)
        {
            if (tokens.Length > m_currentlength)
                SetSize(tokens.Length);
            for (int i = 0; i < tokens.Length; i++)
            {
                SetIndexName(i, tokens[i]);

            }
        }

        private void SetSize(int length)
        {
            Size size = Size._4x4;
            if (length > (uint)Size._1024x1024)
                throw new Exception("Programme is not ready for such size.");
            else if (length > (uint)Size._512x512)
                size = Size._1024x1024;
            else if (length > (uint)Size._256x256)
                size = Size._512x512;
            else if (length > (uint)Size._128x128)
                size = Size._256x256;
            else if (length > (uint)Size._64x64)
                size = Size._128x128;
            else if (length > (uint)Size._32x32)
                size = Size._64x64;
            else if (length > (uint)Size._16x16)
                size = Size._32x32;
            else if (length > (uint)Size._4x4)
                size = Size._16x16;
            else size = Size._4x4;

            SetSize(size);
        }

        public bool GetState(uint index) { return m_booleanArray[index]; }
        public string GetName(uint index) { return m_booleanClaimedArray[index]; }
        public BooleanRef GetBooleanReference(uint index) { return new BooleanRef(this, index); }
        public BooleanRef GetBooleanReference(string booleanName)
        {

            booleanName = booleanName.ToLower().Trim();
            for (uint i = 0; i < m_booleanClaimedArray.Length; i++)
            {
                if (booleanName == m_booleanClaimedArray[i].ToLower().Trim())
                {
                    return new BooleanRef(this, i);

                }
            }
            return null;

        }


        public void SizeUp()
        {
            if (m_size == Size._4x4)
                SetSize(Size._16x16);
            if (m_size == Size._16x16)
                SetSize(Size._32x32);
            if (m_size == Size._32x32)
                SetSize(Size._64x64);
            if (m_size == Size._64x64)
                SetSize(Size._128x128);
            if (m_size == Size._128x128)
                SetSize(Size._256x256);
            if (m_size == Size._256x256)
                SetSize(Size._512x512);
            if (m_size == Size._512x512)
                SetSize(Size._1024x1024);
            if (m_size == Size._1024x1024)
                throw new NotImplementedException("The code linked to this classe is not design for so many value. Contact me if you really need this functionnality...");

        }

    }

    public class BooleanRef
    {
        public BooleanRawRegister m_rawRegister;
        public uint m_index;

        public BooleanRef(BooleanRawRegister register, uint index)
        {
            this.m_rawRegister = register;
            this.m_index = index;
        }

        public bool GetState() { return m_rawRegister.m_booleanArray[m_index]; }
        public string GetName() { return m_rawRegister.m_booleanClaimedArray[m_index]; }

    }



}
