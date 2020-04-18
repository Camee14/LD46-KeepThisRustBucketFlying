using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float m_xMax = 1f;
    public float m_xMin = -1f;

    private Vector3 m_cameraTargetPos;

    private void Start() {
        m_cameraTargetPos = transform.position;
    }

    public void UpdateCameraPos()
    {
        float input = Input.GetAxis("Horizontal");
        m_cameraTargetPos.x = Mathf.Clamp(m_cameraTargetPos.x + input, m_xMin, m_xMax);

        transform.position = Vector3.Lerp(transform.position, m_cameraTargetPos, Time.deltaTime);
    }
}
