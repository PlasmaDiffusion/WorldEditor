using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoad : MonoBehaviour {

    private Button btn;

    // Use this for initialization
    void Start()
    {
        btn = gameObject.GetComponent<Button>();

        btn.onClick.AddListener(clicked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Make every editor object become an active game object
    void clicked()
    {
        Text t = transform.GetChild(0).GetComponent<Text>();
        

        //Play
        if (t.text == "Save")
        {

            InputField inputField = transform.GetChild(2).GetComponent<InputField>();
            
            if (inputField.text != "")
                GameObject.Find("EditorObject").GetComponent<Editor>().Save(inputField.text);


        }
        else if (t.text == "Load")
        {
            InputField inputField = transform.parent.GetChild(2).GetComponent<InputField>();

            if (inputField.text != "")

            {
                //Destroy objects first
                GameObject.Find("EditorObject").GetComponent<Editor>().destroyAllSceneObjects();
                GameObject.Find("EditorObject").GetComponent<Editor>().Load(inputField.text);
            }
        }
    }
}
