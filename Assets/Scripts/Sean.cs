using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sean
{
    private Transform m_transform;
    private Vector3 m_targetPos;
    
    private float m_walkAnimTimer;

    public Transform Transform => m_transform;
    
    public Sean()
    {
        m_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void SetTargetPos(Vector3 pos)
    {
        m_targetPos = pos;
    }

    public void UpdatePos()
    {
        Vector3 nextPos = Vector3.Lerp(m_transform.position, m_targetPos, Time.deltaTime);

        float vel = Vector3.Magnitude(nextPos - m_transform.position);
        m_transform.position = nextPos;
        
        //WalkAnim(vel);
    }

    private void WalkAnim(float vel)
    {
        if (vel < 2f)
        {
            m_transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
        }

        if ((m_walkAnimTimer -= Time.deltaTime) <= 0)
        {
            //m_transform.rotation.ToAngleAxis(out float angle, out Vector3 axis);
            //float dir = angle > 0 ? -1f : 1f;
            //m_transform.rotation *= Quaternion.AngleAxis(30f, Vector3.forward);
            //m_walkAnimTimer = 1f;
        }
    }
}
