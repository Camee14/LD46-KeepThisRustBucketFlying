using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public enum MiniGameState
    {
        IN_PROGRESS,
        SUCESS,
        FAILED
    }
    public GameObject m_ui;
    
    public virtual void Setup()
    {
        
    }

    public virtual void StartPlaying(){
        
    }
    public virtual void Dismiss(){

    }

    public virtual MiniGameState Play()
    {
        return MiniGameState.FAILED;
    }
}
