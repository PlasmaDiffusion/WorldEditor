using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionObject : BaseObject {

    public Rigidbody targetRigid;
    public Vector3 dValue;
    bool EnterBaseObject = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseObject>() != null)
        {
            Debug.Log("Enter:" + other.name);
            EnterBaseObject = true;
            targetRigid = other.GetComponent<Rigidbody>();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseObject>() != null)
        {
            Debug.Log("Exit:" + other.name);
            EnterBaseObject = false;
            targetRigid = null;
        }
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (EnterBaseObject)
        {
            Debug.Log("Add action to:" + targetRigid.name);
            targetRigid.isKinematic = false;
            targetRigid.velocity += dValue;
        }

        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();
        editor.holdingObject = false;
        editor.attachObject(null);
        editor.holdingObject = false;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = -0.5f;
        transform.position = pos;

    }
}
