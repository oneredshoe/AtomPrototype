using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Points : MonoBehaviour
{
    public Text m_MyText;
    public PlayerController pc;


    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {

        m_MyText.text = "" + pc.fishEaten;
    }
}