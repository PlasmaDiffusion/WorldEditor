using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour {


    public GameObject selectedObject;
    public bool holdingObject;
    private float waitTime;

	// Use this for initialization
	void Start () {

        selectedObject = null;
	}
	
	// Update is called once per frame
	void Update () {


        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (selectedObject) selectedObject.transform.position = newPos;


        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            return;
        }

            if (Input.GetMouseButtonDown(0) && holdingObject)
        {
            //Snap selected object onto position in grid
            if (selectedObject)
            {
                selectedObject.transform.position = newPos;
                selectedObject = null;
                holdingObject = false;
            }
        }
    }

    void OnMouseDown()
    {
        
    }

   public void attachObject(GameObject obj)
    {
        if (holdingObject) return;

        selectedObject = obj;
        waitTime = 1.0f;
        holdingObject = true;

        if(obj!= null && obj.GetComponent<Rigidbody>()!=null)
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
