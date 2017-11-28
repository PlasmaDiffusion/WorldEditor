using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionObject : BaseObject {

    public BaseObject target;
    public Vector2 dValue;
    bool EnterBaseObject = false;

    void OnTriggerEnter2D(Collider2D other)
    {

        BaseObject obj = other.GetComponent<BaseObject>();

        if (obj)
        {
            Debug.Log("Enter:" + other.name);
            EnterBaseObject = true;
            target = other.gameObject.GetComponent<BaseObject>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.GetComponent<BaseObject>() != null)
        {
            Debug.Log("Exit:" + other.name);
            EnterBaseObject = false;
            target = null;


        }
    }

    //When clicked, the target object will have its velocity overwritten
    protected override void OnMouseOver()
    {
        //base.OnMouseOver();

        
        if (Input.GetMouseButtonDown(0))
        {

            if (EnterBaseObject)
        {
            Debug.Log("Velocity overwritten for: " + target.name);
            target.overwriteVelocity = dValue;
        }

       

        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();
        editor.holdingObject = false;
        editor.attachObject(null);
        editor.holdingObject = false;
        Destroy(gameObject);

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = 0.0f;
        transform.position = pos;

    }
}
