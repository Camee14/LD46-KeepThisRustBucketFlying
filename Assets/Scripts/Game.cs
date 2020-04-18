using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game
{
    private CameraMove m_camMove;
    private Sean m_sean;
    private ShipSystem[] m_shipSystems;
    private int m_currentMiniGame;

    private float m_decayRate = 1f;
    private float m_decayIncreaseRate = 0.1f;
    private float m_failDamage = 25f;
    
    public Game()
    {
        Random.InitState(DateTime.Now.Millisecond);
        m_sean = new Sean();
        
        m_camMove = Camera.main.transform.GetComponent<CameraMove>();

        m_shipSystems = GameObject.FindObjectsOfType<ShipSystem>();
        m_currentMiniGame = -1;
    }

    public bool Update()
    {
        bool checkSystemsForInput = false;
        Vector3 mouseWorldPos = Vector3.zero;
        
        if(m_currentMiniGame >= 0)
        {
            ShipSystem system = m_shipSystems[m_currentMiniGame];
            MiniGame.MiniGameState result = system.MiniGame.Play();
            
            switch (result)
            {
                case MiniGame.MiniGameState.IN_PROGRESS: break;
                case MiniGame.MiniGameState.SUCESS: 
                    system.Replenish();
                    system.MiniGame.Dismiss();
                    m_currentMiniGame = -1;
                    break;
                case MiniGame.MiniGameState.FAILED:
                    system.ApplyDamage(m_failDamage);
                    system.MiniGame.Dismiss();
                    m_currentMiniGame = -1;
                    break;
                default: Debug.LogError("MiniGame in unexpected state"); break;
            }
        }
        else
        {
            m_camMove.UpdateCameraPos();
            if (Input.GetMouseButtonDown(0))
            {
                mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                checkSystemsForInput = true;
            }
        }
        
        for(int i = 0; i < m_shipSystems.Length; i++)
        {
            ShipSystem system = m_shipSystems[i];
            if (checkSystemsForInput && system.CheckWorldPosInBounds(mouseWorldPos))
            {
                system.MiniGame.StartPlaying();
                m_sean.SetTargetPos(system.transform.position);
                m_currentMiniGame = i;
            }

            if(system.Decay(m_decayRate))
            {
                return false;
            }
        }
        
        m_sean.UpdatePos();
        m_decayRate += m_decayIncreaseRate * Time.deltaTime;

        return true;
    }
    
}
