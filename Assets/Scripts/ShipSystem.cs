using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSystem : MonoBehaviour
{
    public float m_startingHealth = 100f;
    public Transform m_uiPrefab;
    
    private float m_health = 100f;
    
    private MiniGame m_miniGame;
    private SpriteRenderer m_renderer;
    private Image m_ui;
    public MiniGame MiniGame => m_miniGame;

    private void Awake() 
    {
        m_miniGame = GetComponent<MiniGame>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_miniGame.Setup();
        m_health = m_startingHealth;
        
        Transform ui = Instantiate(m_uiPrefab, transform.position + Vector3.forward * 2f, Camera.main.transform.rotation);
        ui.GetComponent<Canvas>().worldCamera = Camera.main;

        m_ui = ui.GetComponentInChildren<Image>();
    }

    public bool Decay(float rate)
    {
        m_health -= rate * Time.deltaTime;
        m_ui.fillAmount = m_health / m_startingHealth;
        return m_health <= 0;
    }

    public void Replenish()
    {
        m_health = m_startingHealth;
    }

    public void ApplyDamage(float amount)
    {
        m_health -= amount;
    }

    public bool CheckWorldPosInBounds(Vector3 worldPos)
    {
        Bounds bounds = m_renderer.bounds;
        return bounds.min.x < worldPos.x && bounds.max.x > worldPos.x
            && bounds.min.z < worldPos.z && bounds.max.z > worldPos.z;
    }
}
