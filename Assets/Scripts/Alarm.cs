using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    private AudioSource m_source;
    private int m_systemsInAlarm;
    void Awake()
    {
        m_source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!m_source.isPlaying)
        {
            m_source.Play();
        }
    }

    public void Cancel()
    {
        m_source.Stop();
    }

    public void ReduceVolume(float reduction)
    {
        m_source.volume -= reduction;
    }

    public void ResetVolume()
    {
        m_source.volume = 1f;
    }

}
