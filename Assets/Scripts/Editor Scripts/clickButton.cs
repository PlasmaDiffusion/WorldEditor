using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickButton : MonoBehaviour
{

    public GameObject prefabToCreate;

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

    protected GameObject lastCreated = null;

    protected virtual void clicked()
    {
        Debug.Log("Clicked");
        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();

        lastCreated = Instantiate(prefabToCreate) as GameObject;
        lastCreated.GetComponent<BaseObject>().makeDefaults = true;
        editor.attachObject(lastCreated);

    }

}
