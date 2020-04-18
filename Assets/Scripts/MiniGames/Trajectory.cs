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

    private Vector3 m_min;
    private Vector3 m_max;
    private float m_pingPong;
    private float m_targetSize = 0.3f;
    private bool m_lock = false;
    private bool m_movingVerticalPointer = false;
    
    public override void Setup()
    {
        m_target = m_ui.transform.Find("Target").GetComponent<RectTransform>();
        m_horizontalPointer = m_ui.transform.Find("HorizontalPointer").GetComponent<RectTransform>();
        m_verticalPointer = m_ui.transform.Find("VerticalPointer").GetComponent<RectTransform>();
        m_lockButton = m_ui.transform.Find("Lock").GetComponent<Button>();
        
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
        m_ui.SetActive(true);
    }

    public override MiniGameState Play()
    {
        if (m_lock)
        {
            float targetMin;
            float targetMax;
            float pointer;
            if (m_movingVerticalPointer)
            {
                targetMin = m_target.localPosition.y + m_target.rect.yMin * m_targetSize;
                targetMax = m_target.localPosition.y + m_target.rect.yMax * m_targetSize;
                pointer = m_verticalPointer.transform.localPosition.y;
                
                if (targetMin <= pointer && targetMax >= pointer)
                {
                    return MiniGameState.SUCESS;
                }
                return MiniGameState.FAILED;
            }

            targetMin = m_target.localPosition.x + m_target.rect.xMin * m_targetSize;
            targetMax = m_target.localPosition.x + m_target.rect.xMax * m_targetSize;
            pointer = m_horizontalPointer.transform.localPosition.x;
            
            if (targetMin > pointer && targetMax < pointer)
            {
                return MiniGameState.FAILED;
            }
            
            m_pingPong = 0f;
            m_movingVerticalPointer = true;
            m_verticalPointer.gameObject.SetActive(true);
        }
        m_lock = false;
        
        float per = Mathf.PingPong(m_pingPong, 1f);
        m_pingPong += m_startingSpeed * Time.deltaTime;
        
        if (m_movingVerticalPointer)
        {
            SetVerticalPos(per);
        }
        else
        {
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

    public override void Dismiss()
    {
        m_ui.SetActive(false);
    }
}
