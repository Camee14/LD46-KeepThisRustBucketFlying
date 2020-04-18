using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    private Game m_gameInstance;
    private EndSequence m_EndSequence;
    
    private bool m_gameRunning = true;
    void Awake()
    {
        m_gameInstance = new Game();
        m_EndSequence = new EndSequence();
    }

    void Update()
    {
        if (m_gameRunning)
        {
            m_gameRunning = m_gameInstance.Update();
            if (!m_gameRunning)
            {
                m_EndSequence.Setup();
            }
        }
        else
        {
            m_EndSequence.Update();
        }
    }
}
