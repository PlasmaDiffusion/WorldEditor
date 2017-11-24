using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

    public bool inEditor;
    private bool clickedOn;

	// Use this for initialization
	void Start () {
        inEditor = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (inEditor) return;
	}

    void OnMouseDown()
    {
        if (!inEditor) return;
        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();
        editor.attachObject(gameObject);

        clickedOn = true;
    }


}
