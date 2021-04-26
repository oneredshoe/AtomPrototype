using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Alert : MonoBehaviour
{
    public Text m_MyText;
    public bool m_alert;

    void Start()
    {
        m_alert = false;
        //Text sets your text to say this message
        m_MyText.text = "";
    }

    void Update()
    {
        if (m_alert == true)
        {
            m_MyText.text = "SPOTTED";
        }
        else
        {
            m_MyText.text = "";
        }

    }
}