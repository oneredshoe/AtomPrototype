using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CanvasScript : MonoBehaviour
{
    ChangeScenes cs;
    // Start is called before the first frame update
    void Start()
    {
       cs =  GetComponent<ChangeScenes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            cs.Survey();
        }
    }
}
