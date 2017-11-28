using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindow : clickButton
{
    private GameObject animationTool;
    private GameObject animationTool2;

    bool active;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(clicked);

        active = false;
        animationTool = GameObject.Find("SpeedControl").gameObject;
        animationTool2 = GameObject.Find("DirectionController").gameObject;

        animationTool.SetActive(false);
        animationTool2.SetActive(false);
    }
    
    //Toggle stuff on and off
    protected override void clicked()
    {
        if (active)
        {
            active = false;
            transform.GetChild(0).GetComponent<Text>().text = "Custom Movement (Animation Tool)";

        }
        else if (!active)
        {
            active = true;
            transform.GetChild(0).GetComponent<Text>().text = "Close";

        }

        animationTool.SetActive(active);
        animationTool2.SetActive(active);


    }
}