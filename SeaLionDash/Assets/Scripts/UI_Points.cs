using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Points : MonoBehaviour
{
    public Text m_MyText;
    public int m_points;

    void Start()
    {
        m_points = 0;
        //Text sets your text to say this message
        m_MyText.text = "" +  m_points;
    }

    void Update()
    {
        m_MyText.text = "" + m_points;
    }
}