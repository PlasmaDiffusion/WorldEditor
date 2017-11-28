using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkSpeed : MonoBehaviour {

    public Slider targetSlider;
    public Text targetText;

	// Use this for initialization
	void Start () {
        targetSlider = GameObject.FindObjectOfType<UnityEngine.UI.Slider>();
        targetText = gameObject.GetComponent<Text>();
            }
	
	// Update is called once per frame
	void Update () {
        targetText.text = targetSlider.value.ToString("F2");
	}
}
