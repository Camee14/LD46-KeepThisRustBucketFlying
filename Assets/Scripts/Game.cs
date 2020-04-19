using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : IGameStage
{
    private CameraMove m_camMove;
    private Sean m_sean;
    private ShipSystem[] m_shipSystems;
    private int m_currentMiniGame;

    private EndSequence m_endSequence;
    private float m_scoreTimer;
    
    public Game(EndSequence endSequence)
    {
        Random.InitState(DateTime.Now.Millisecond);
        m_sean = new Sean();
        
        m_camMove = Camera.main.transform.GetComponent<CameraMove>();

        m_shipSystems = GameObject.FindObjectsOfType<ShipSystem>();
        m_currentMiniGame = -1;

        m_endSequence = endSequence;
    }

    public void Setup()
    {
        m_sean.Transform.position = Vector3.zero;
        m_sean.SetTargetPos(Vector3.zero);
        
        GameObject.Find("Ship").transform.position = Vector3.zero;

        Camera.main.transform.position = new Vector3(0, 5f, 0);

        foreach (ShipSystem system in m_shipSystems)
        {
            system.Setup();
        }

        m_scoreTimer = 0f;
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
                    system.ApplyDamage();
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

            if(system.Decay())
            {
                return false;
            }
        }
        
        m_sean.UpdatePos();
        m_scoreTimer += Time.deltaTime;

        return true;
    }

    public void Dismiss()
    {
        foreach (ShipSystem system in m_shipSystems)
        {
            system.MiniGame.Dismiss();
        }
        
        m_endSequence.SetFinalScore(m_scoreTimer);
    }
}
