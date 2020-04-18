using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequence
{
    private Transform m_sean;
    private Vector3 m_seanVel = new Vector3(1f, 0.2f);
    public EndSequence()
    {
        m_sean = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Setup()
    {
        Camera main = Camera.main;
        main.transform.position = Vector3.right * 100f + Vector3.up * 5f;
        m_sean.transform.position = main.ViewportToWorldPoint(new Vector3(0, 0.5f, 5f)) - Vector3.right * 3f;
    }

    public void Update()
    {
        m_sean.transform.position += m_seanVel * Time.deltaTime;
        m_sean.transform.rotation *= Quaternion.AngleAxis(30f * Time.deltaTime, Vector3.forward);
    }
}
