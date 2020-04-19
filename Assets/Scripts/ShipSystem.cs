using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSystem : MonoBehaviour
{
    public float m_startingHealth = 100f;

    public float m_startingDecayRate = 0.75f;
    public float m_decayIncreaseRate = 0.1f;
    public float m_failDamage = 25f;

    public Transform m_uiPrefab;
    
    private float m_health = 100f;
    private float m_decayRate;

    private MiniGame m_miniGame;
    private SpriteRenderer m_renderer;
    private Image m_ui;
    private float m_alarmTimer;
    public MiniGame MiniGame => m_miniGame;

    private void Awake()
    {
        m_miniGame = GetComponent<MiniGame>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_miniGame.Setup();
        m_health = m_startingHealth;

        Transform ui = Instantiate(m_uiPrefab, transform.position + Vector3.forward, Camera.main.transform.rotation);
        ui.SetParent(transform);
        ui.GetComponent<Canvas>().worldCamera = Camera.main;

        m_ui = ui.GetComponentInChildren<Image>();
    }

    public void Setup()
    {
        m_decayRate = m_startingDecayRate;
        Replenish();
    }

    public bool Decay()
    {
        m_health -= m_decayRate * Time.deltaTime;
        SetUI();
        
        m_decayRate += m_decayIncreaseRate * Time.deltaTime;
        
        return m_health <= 0;
    }

    public void Replenish()
    {
        m_health = m_startingHealth;
    }

    public void ApplyDamage()
    {
        m_health -= m_failDamage;
    }

    public bool CheckWorldPosInBounds(Vector3 worldPos)
    {
        Bounds bounds = m_renderer.bounds;
        return bounds.min.x < worldPos.x && bounds.max.x > worldPos.x
            && bounds.min.z < worldPos.z && bounds.max.z > worldPos.z;
    }

    private void SetUI()
    {
        float healthPer = m_health / m_startingHealth;
        m_ui.fillAmount = healthPer;

        if (healthPer < 0.33f)
        {
            m_ui.color = Color.red;
            float scl = 1f + 0.2f * Mathf.Sin(m_alarmTimer);
            m_ui.transform.localScale = Vector3.one * scl;

            m_alarmTimer += 5f * Time.deltaTime;
        }
        else
        {
            m_ui.transform.localScale = Vector3.one;
            m_alarmTimer = 0f;
        }
    }
}
