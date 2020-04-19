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
    private InteriorDebris[] m_InteriorDebris;
    
    private Alarm m_alarm;
    private AudioSource m_success;
    private AudioSource m_fail;

    private EndSequence m_endSequence;
    private float m_scoreTimer;
    
    public Game(EndSequence endSequence)
    {
        Random.InitState(DateTime.Now.Millisecond);
        m_sean = new Sean();
        
        m_camMove = Camera.main.transform.GetComponent<CameraMove>();

        m_shipSystems = GameObject.FindObjectsOfType<ShipSystem>();
        m_currentMiniGame = -1;

        m_InteriorDebris = GameObject.FindObjectsOfType<InteriorDebris>();

        m_alarm = GameObject.Find("Alarm").GetComponent<Alarm>();
        m_success = GameObject.Find("SuccessAudio").GetComponent<AudioSource>();
        m_fail = GameObject.Find("FailAudio").GetComponent<AudioSource>();
        
        m_endSequence = endSequence;
    }

    public void Setup()
    {
        m_sean.Transform.position = Vector3.zero;
        m_sean.Transform.rotation = Quaternion.identity;
        
        m_sean.SetTargetPos(Vector3.zero);

        GameObject.Find("Ship").transform.position = Vector3.zero;

        Camera.main.transform.position = new Vector3(0, 0, -5f);

        foreach (ShipSystem system in m_shipSystems)
        {
            system.Setup();
        }
        
        m_alarm.Cancel();
        m_alarm.ResetVolume();
        
        m_success.Stop();
        m_fail.Stop();
        
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
                    m_success.Play();
                    m_currentMiniGame = -1;
                    break;
                case MiniGame.MiniGameState.FAILED:
                    system.ApplyDamage();
                    system.MiniGame.Dismiss();
                    m_fail.Play();
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

        bool shipAlarmActive = false;
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

            if (system.AlarmActive())
            {
                shipAlarmActive = true;
            }
        }

        if (shipAlarmActive)
        {
            m_alarm.Play();
        }
        else
        {
            m_alarm.Cancel();
        }

        m_sean.UpdatePos();
        m_scoreTimer += Time.deltaTime;

        foreach (InteriorDebris debris in m_InteriorDebris)
        {
            debris.UpdatePos();
        }

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
