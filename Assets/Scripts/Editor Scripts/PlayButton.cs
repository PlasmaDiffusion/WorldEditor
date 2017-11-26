using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    private Button btn;

	// Use this for initialization
	void Start () {
        btn = gameObject.GetComponent<Button>();

        btn.onClick.AddListener(startLevel);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Make every editor object become an active game object
    void startLevel()
    {
        Text t = transform.GetChild(0).GetComponent<Text>();

        //Play
        if (t.text != "End")
        {


        t.text = "End";
        GameObject.Find("EditorObject").GetComponent<Editor>().play();


        }
        else //Back to editor
        {
        t.text = "Play";
        GameObject.Find("EditorObject").GetComponent<Editor>().endPlay();
        }
    }
}
