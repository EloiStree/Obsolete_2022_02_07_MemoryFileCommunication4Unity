using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class MFC_RecoverLargeBytesWithDateUpdate : MonoBehaviour
{
    public MemoryFileConnectionMono m_whereToRecoverBytes;
    public BytesEvent m_recoveredOnUnityThread;
    [System.Serializable]
    public class BytesEvent : UnityEvent<byte[]> { }

    public int m_sizeOfByteArray;
     byte[] m_bytes = new byte[0];
    public long m_timeToRecover;
    [ContextMenu("Try To Recovert")]
    public void TryToRecoverUnityThread()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        if (m_bytes.Length != m_whereToRecoverBytes.m_connection.m_setupInfo.m_maxMemorySize)
            m_bytes = new byte[m_whereToRecoverBytes.m_connection.m_setupInfo.m_maxMemorySize];

        m_whereToRecoverBytes.Connection.GetAsBytes(out m_bytes);
        //  m_recoveredOnUnityThread.Invoke(m_bytes);
        watch.Stop();
        m_timeToRecover = watch.ElapsedMilliseconds;
    }



    public bool m_useThread;
    public Thread thread;
    private void Awake()
    {
        if (m_useThread)
        {
            thread = new Thread(FetchBytes);
            thread.Start();
            
        }
    }
    private void OnDestroy()
    {
        if (thread != null)
        {
            thread.Abort();
        }
    }

    private void Update()
    {
        if (m_hasSomethingToPush)
        {
            m_hasSomethingToPush = false;
            m_recoveredOnUnityThread.Invoke(m_bytes);
        }
    }

    public byte[] m_bytesToFetch;
    public bool m_requestFetch;
    public bool m_hasSomethingToPush;

    [ContextMenu("Try To Recovert")]
    public void RequestToFetchFromThread() {
        m_requestFetch = true;
    }
    private void FetchBytes()
    {

        while (true) {
            if (m_requestFetch )
            {
                m_requestFetch = false;
                Stopwatch watch = new Stopwatch();
                watch.Start();
                if (m_bytes.Length != m_whereToRecoverBytes.m_connection.m_setupInfo.m_maxMemorySize)
                    m_bytes = new byte[m_whereToRecoverBytes.m_connection.m_setupInfo.m_maxMemorySize];

                m_whereToRecoverBytes.Connection.GetAsBytes(out m_bytes);
                //  m_recoveredOnUnityThread.Invoke(m_bytes);
                watch.Stop();
                m_timeToRecover = watch.ElapsedMilliseconds;
                m_hasSomethingToPush = true;
            }
            Thread.Sleep(1);
        }
       
    }
}
