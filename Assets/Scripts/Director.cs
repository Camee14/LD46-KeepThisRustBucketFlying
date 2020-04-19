using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    private IGameStage[] m_stages;
    private int m_currentStage;
    private IGameStage CurrentStage => m_stages[m_currentStage];

    private void IncrementStageCounter()
    {
        m_currentStage = (m_currentStage + 1) % m_stages.Length;
    }

    void Awake()
    {
        EndSequence endSequence = new EndSequence();
        Game game = new Game(endSequence);
        
        m_stages = new IGameStage[2];
        m_stages[0] = game;
        m_stages[1] = endSequence;
    }

    void Start()
    {
        CurrentStage.Setup();
    }

    void Update()
    {
        bool result = CurrentStage.Update();
        if (!result)
        {
            CurrentStage.Dismiss();
            IncrementStageCounter();
            CurrentStage.Setup();
        }
    }
}
