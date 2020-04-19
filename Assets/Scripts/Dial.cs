using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dial : MonoBehaviour , IDragHandler
{
    public float m_threshold = 10f;
    public bool m_isVertical = false;
    
    private float m_dialSubPos = 0f;

    private int m_dialDelta = 0;
    private int m_dialPos = 0;
    
    public void OnDrag(PointerEventData eventData)
    {
        m_dialSubPos += m_isVertical ? eventData.delta.y : eventData.delta.x;
        
        if (Mathf.Abs(m_dialSubPos) >= m_threshold)
        {
            m_dialDelta = Mathf.RoundToInt(m_dialSubPos / m_threshold);
            m_dialPos += m_dialDelta;
        }
        
        m_dialSubPos = m_dialSubPos % m_threshold;
    }

    public int GetDialDelta()
    {
        int delta = m_dialDelta;
        m_dialDelta = 0;
        return delta;
    }
}
