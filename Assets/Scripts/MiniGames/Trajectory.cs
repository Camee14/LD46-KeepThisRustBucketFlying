using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trajectory : MiniGame
{
    public float m_startingSpeed = 1f;
    

    private RectTransform m_target;
    private RectTransform m_horizontalPointer;
    private RectTransform m_verticalPointer;
    private Button m_lockButton;

    private AudioSource m_pingPongAudio;

    private Vector3 m_min;
    private Vector3 m_max;
    private float m_pingPong;
    private float m_targetSize = 0.3f;
    private bool m_lock = false;
    private bool m_movingVerticalPointer = false;
    
    public override void Setup()
    {
        Transform container = m_ui.transform.GetChild(0);
        m_target = container.Find("Target").GetComponent<RectTransform>();
        m_horizontalPointer = container.Find("HorizontalPointer").GetComponent<RectTransform>();
        m_verticalPointer = container.Find("VerticalPointer").GetComponent<RectTransform>();
        m_lockButton = container.Find("Lock").GetComponent<Button>();

        m_pingPongAudio = container.Find("PingPongAudio").GetComponent<AudioSource>();
        
        Rect rect = m_ui.GetComponent<RectTransform>().rect;
        m_min = rect.min;
        m_max = rect.max;
        
        m_lockButton.onClick.AddListener(() => m_lock = true);
    }

    public override void StartPlaying()
    {
        SetHorizontalPos(0f);
        SetVerticalPos(0f);

        m_pingPong = 0f;
        m_movingVerticalPointer = false;
        m_lock = false;
        
        m_target.localScale = Vector3.one * m_targetSize;
        Vector3 targetPos = Vector3.zero;
        targetPos.x = Random.Range(m_min.x, m_max.x);
        targetPos.y = Random.Range(m_min.y, m_max.y);
        m_target.localPosition = targetPos;
        
        m_verticalPointer.gameObject.SetActive(false);
        m_ui.transform.GetChild(0).gameObject.SetActive(true);
    }

    public override MiniGameState Play()
    {
        if (m_lock)
        {
            if (m_movingVerticalPointer)
            {
                if (IsVerticalPointerOverTarget())
                {
                    return MiniGameState.SUCESS;
                }
                return MiniGameState.FAILED;
            }

            if (IsHorizontalPointerOverTarget())
            {
                m_pingPong = 0f;
                m_movingVerticalPointer = true;
                m_verticalPointer.gameObject.SetActive(true);
            }
            else
            {
                return MiniGameState.FAILED;
            }
        }
        m_lock = false;
        
        float per = Mathf.PingPong(m_pingPong, 1f);
        m_pingPong += m_startingSpeed * Time.deltaTime;
        
        if (m_movingVerticalPointer)
        {
            if (IsVerticalPointerOverTarget(0.5f) && !m_pingPongAudio.isPlaying)
            {
                m_pingPongAudio.Play();
            }

            SetVerticalPos(per);
        }
        else
        {
            if (IsHorizontalPointerOverTarget(0.5f) && !m_pingPongAudio.isPlaying)
            {
                m_pingPongAudio.Play();
            }
            
            SetHorizontalPos(per);
        }

        return MiniGameState.IN_PROGRESS;
    }

    private void SetHorizontalPos(float per)
    {
        Vector3 hPos = Vector3.zero;
        hPos.x = Mathf.Lerp(m_min.x, m_max.x, per);
        m_horizontalPointer.localPosition = hPos;
    }

    private void SetVerticalPos(float per)
    {
        Vector3 vPos = Vector3.zero;
        vPos.y = Mathf.Lerp(m_min.y, m_max.y, per);
        m_verticalPointer.localPosition = vPos;
    }

    private bool IsHorizontalPointerOverTarget(float tolerance = 1f)
    {
        float targetMin = m_target.localPosition.x + m_target.rect.xMin * m_targetSize * tolerance;
        float targetMax = m_target.localPosition.x + m_target.rect.xMax * m_targetSize * tolerance;
        float pointer = m_horizontalPointer.transform.localPosition.x;

        return targetMin <= pointer && targetMax >= pointer;
    }

    private bool IsVerticalPointerOverTarget(float tolerance = 1f)
    {
        float targetMin = m_target.localPosition.y + m_target.rect.yMin * m_targetSize * tolerance;
        float targetMax = m_target.localPosition.y + m_target.rect.yMax * m_targetSize * tolerance;
        float pointer = m_verticalPointer.transform.localPosition.y;

        return targetMin <= pointer && targetMax >= pointer;
    }

    public override void Dismiss()
    {
        m_ui.transform.GetChild(0).gameObject.SetActive(false);
    }
}
