using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    private Game m_gameInstance;

    void Awake()
    {
        m_gameInstance = new Game();
    }

    void Update()
    {
        m_gameInstance.Update();
    }
}
