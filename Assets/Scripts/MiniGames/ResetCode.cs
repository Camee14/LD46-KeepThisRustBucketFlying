using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ResetCode : MiniGame
{
    private static readonly string[] k_alphabet = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};

    private TMP_InputField[] m_displayDigits;
    private TMP_InputField[] m_inputDigits;
    
    private string[] m_correctSequence = new string[4];
    private int m_currentInputIndex;
    
    public override void Setup()
    {
        m_displayDigits = new TMP_InputField[4];
        m_inputDigits = new TMP_InputField[4];
        
        Transform container = m_ui.transform.GetChild(0);
        Transform display = container.Find("Display");
        Transform input = container.Find("Input");
        
        int index = 0;
        foreach (Transform displayChild in display)
        {
            m_displayDigits[index] = displayChild.GetComponent<TMP_InputField>();
            index++;
        }

        index = 0;
        foreach (Transform inputChild in input)
        {
            m_inputDigits[index] = inputChild.GetComponent<TMP_InputField>();
            index++;
        }
    }
    
    public override void StartPlaying()
    {
         foreach (TMP_InputField displayDigit in m_displayDigits)
         {
             displayDigit.text = k_alphabet[Random.Range(0, k_alphabet.Length)];
         }
         foreach (TMP_InputField inputDigit in m_inputDigits)
         {
             inputDigit.text = "_";
         }
         
         List<string> correctSequence = new List<string>(Array.ConvertAll(m_displayDigits, input => input.text));
         correctSequence.Sort();
         m_correctSequence = correctSequence.ToArray();
         m_currentInputIndex = 0;
         
         m_ui.transform.GetChild(0).gameObject.SetActive(true);
    }

    public override MiniGameState Play()
    {
        string key = m_correctSequence[m_currentInputIndex];
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2))
        {
            if (Input.GetKeyDown(key))
            {
                m_inputDigits[m_currentInputIndex].text = key;
                m_currentInputIndex++;
            }
            else
            {
                return MiniGameState.FAILED;
            }
        }

        return m_currentInputIndex == 4 ? MiniGameState.SUCESS : MiniGameState.IN_PROGRESS;
    }

    public override void Dismiss()
    {
        m_ui.transform.GetChild(0).gameObject.SetActive(false);
    }
}
