using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {

    public bool inEditor;
    protected bool clickedOn;

    public int prefabID;

	// Use this for initialization
	void Start () {
        inEditor = true;

        if (prefabID == 1) transform.eulerAngles = new Vector3(0.0f, 0.0f, 45.0f);
	}

    // Update is called once per frame
    protected void Update () {
        if (inEditor) return;
	}

    protected void OnMouseDown()
    {
        if (!inEditor) return;
        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();
        editor.attachObject(gameObject);

        clickedOn = true;
    }


}
