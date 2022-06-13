using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryFileConnectObserveAsTextMono : MonoBehaviour
{
    public MemoryFileConnectionMono m_observedDate;
    public bool m_useUpdate;

    public string m_textPrevious;
    public string m_text;
    public UnityEvent  m_onChange;

    public void Update()
    {
        if (m_useUpdate)
        Refresh();

    }

    private void Refresh()
    {
        m_observedDate.Connection.GetAsText(out  m_text);
        if (Eloi.E_StringUtility.AreNotEquals(in m_text, in m_textPrevious, true, true))
        {
            m_onChange.Invoke();
        }
        m_textPrevious = m_text;
    }
    private void Reset()
    {
        m_observedDate = GetComponent<MemoryFileConnectionMono>();
    }
}
