using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCore : MiniGame
{
    private RectTransform m_horizontal;
    private RectTransform m_vertical;

    private Dial m_hDial;
    private Dial m_vDial;

    private float m_victoryTime = 1.7f;
    private float m_victoryTimer;
    private bool m_hasWon;
    
    public override void Setup()
    {
        Transform container = m_ui.transform.GetChild(0);
        m_horizontal = container.Find("Content/Horizontal").GetComponent<RectTransform>();
        m_vertical = container.Find("Content/Vertical").GetComponent<RectTransform>();

        m_hDial = container.Find("HorizontalDial").GetComponent<Dial>();
        m_vDial = container.Find("VerticalDial").GetComponent<Dial>();
    }

    public override void StartPlaying()
    {
        m_horizontal.localPosition = Vector3.right * (Random.Range(-15, 16) * 10f);
        m_vertical.localPosition = Vector3.up * (Random.Range(-15, 16) * 10f);
        
        m_ui.transform.GetChild(0).gameObject.SetActive(true);

        m_victoryTimer = m_victoryTime;
        m_hasWon = false;
    }

    public override MiniGameState Play()
    {
        if (m_hasWon)
        {
            if ((m_victoryTimer -= Time.deltaTime) < 0f)
            {
                return MiniGameState.SUCESS;
            }

            return MiniGameState.IN_PROGRESS;
        }

        MoveHorizontal();
        MoveVertical();

        m_hasWon = m_horizontal.localPosition == Vector3.zero && m_vertical.localPosition == Vector3.zero;
        return MiniGameState.IN_PROGRESS;
    }

    private void MoveHorizontal()
    {
        Vector3 nextPos = m_horizontal.localPosition;

        nextPos.x = Mathf.Clamp(nextPos.x + m_hDial.GetDialDelta() * 10f, -150f, 150f);
        m_horizontal.localPosition = nextPos;
    }

    private void MoveVertical()
    {
        Vector3 nextPos = m_vertical.localPosition;
        nextPos.y = Mathf.Clamp(nextPos.y + m_vDial.GetDialDelta() * 10f, -150f, 150f);
        m_vertical.localPosition = nextPos;
    }

    public override void Dismiss()
    {
        m_ui.transform.GetChild(0).gameObject.SetActive(false);
    }
}
