using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSpeed : MonoBehaviour {

    public UnityEngine.UI.Slider targetSlider;
    public UnityEngine.UI.Text targetText;

	// Use this for initialization
	void Start () {
        targetSlider = GameObject.FindObjectOfType<UnityEngine.UI.Slider>();
        targetText = GameObject.FindObjectOfType<UnityEngine.UI.Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
        targetText.text = targetSlider.value.ToString("F2");
	}
}
