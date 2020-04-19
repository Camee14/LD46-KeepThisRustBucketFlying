using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteriorDebris : MonoBehaviour
{
    private Vector2 m_vel;
    private float m_rot;
    private void Awake()
    {
        m_vel = Random.insideUnitCircle * 0.1f;
        m_rot = Random.Range(20f, 60f);
    }

    public void UpdatePos()
    {
        transform.position += (Vector3)m_vel * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(m_rot * Time.deltaTime, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        m_vel = Vector2.Reflect(m_vel, other.GetContact(0).normal);
    }
}
