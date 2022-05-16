using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFC_ObserverToText : MonoBehaviour
{
    public MemoryFileConnectionMono m_observed;
    [TextArea(0,10)]
    public string m_previousText;
    [TextArea(0, 10)]
    public string m_currentText;
    public Eloi.PrimitiveUnityEvent_String m_newText;

    [ContextMenu("Check Memory for change")]
    public void CheckMemoryForChange() {
        m_observed.Connection.GetAsText(out m_currentText);
        if (Eloi.E_StringUtility.AreNotEquals(in m_currentText, in m_previousText, false, false)) {
            m_previousText = m_currentText;
            m_newText.Invoke(m_currentText);
        }
    }
 
}
