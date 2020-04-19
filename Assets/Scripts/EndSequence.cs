using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSequence : IGameStage
{
    private Transform m_sean;
    private Transform m_ship;
    
    private Vector3 m_seanVel = new Vector3(1f, 0f, 0.35f);
    private Vector3 m_shipVel = new Vector3(-1.2f, 0f, 0f);

    private enum SequenceStage
    {
        SHIP_WITHDRAW,
        BLACK_SCREEN,
        SPACEWALK
    }

    private const float m_shipWithdrawTime = 12f;
    private const float m_blackScreenTime = 2f;
    private const float m_spaceWalkTime = 5f;
    
    private SequenceStage m_activeStage = SequenceStage.SHIP_WITHDRAW;
    private float m_stageTimer;
    
    private Transform m_uiContainer;
    private Button m_restart;
    private TMP_Text m_score;
    private bool m_restartClicked = false;

    private float m_finalScore;
    public EndSequence()
    {
        m_sean = GameObject.FindGameObjectWithTag("Player").transform;
        m_ship = GameObject.Find("Ship").transform;
        
        m_uiContainer = GameObject.Find("RestartUI").transform.GetChild(0);
        m_restart = m_uiContainer.GetComponentInChildren<Button>();
        m_restart.onClick.AddListener(() => m_restartClicked = true);
        m_score = m_uiContainer.Find("Score").GetComponent<TMP_Text>();
        m_score.text = "";
    }

    public void SetFinalScore(float finalScore)
    {
        m_finalScore = finalScore;
    }

    public void Setup()
    {
        m_activeStage = SequenceStage.SHIP_WITHDRAW;
        m_stageTimer = 0f;
        
        m_restartClicked = false;
    }

    public bool Update()
    {
        switch (m_activeStage)
        {
            case SequenceStage.SHIP_WITHDRAW : UpdateShipWithdraw(); return true;
            case SequenceStage.BLACK_SCREEN : UpdateBlackScreen(); return true;
            case SequenceStage.SPACEWALK : UpdateSpaceWalk(); break;
        }

        if (m_restartClicked)
        {
            return false;
        }

        return true;
    }

    private void UpdateShipWithdraw()
    {
        if((m_stageTimer += Time.deltaTime) > m_shipWithdrawTime)
        {
            m_activeStage++;
            m_stageTimer = 0f;
            return;
        }
        
        m_sean.transform.position += m_shipVel * Time.deltaTime;
        m_ship.transform.position += m_shipVel * Time.deltaTime;
    }

    private void UpdateBlackScreen()
    {
        if((m_stageTimer += Time.deltaTime) > m_blackScreenTime)
        {
            m_activeStage++;
            m_stageTimer = 0f;
            
            m_sean.transform.position = new Vector3(-8, 0, 0);
        }
    }

    private void UpdateSpaceWalk()
    {
        if((m_stageTimer += Time.deltaTime) > m_spaceWalkTime)
        {
            m_score.text = $"You Kept the Ship Running for {m_finalScore:F0} Seconds";
            m_uiContainer.gameObject.SetActive(true);
        }
    
        m_sean.transform.position += m_seanVel * Time.deltaTime;
        m_sean.transform.rotation *= Quaternion.AngleAxis(30f * Time.deltaTime, Vector3.forward);
    }

    public void Dismiss()
    {
        m_uiContainer.gameObject.SetActive(false);
    }
}
