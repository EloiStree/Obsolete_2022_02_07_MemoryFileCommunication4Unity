using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MFC_PushLargeBytesWithDateUpdate : MonoBehaviour
{
    public MemoryFileConnectionMono m_whereToPushBytes;
    public MemoryFileConnectionMono m_whereToPushDate;

    public void PushBytes( byte[] bytesToPush) {

        m_whereToPushBytes.SetAsBytes(bytesToPush);
        m_whereToPushDate.SetAsText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
    }
}
