using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : BaseObject {

    float springVelocity;
    
    public static int[] customSpringCollisions;

    // Use this for initialization
    void Start () {
        springVelocity = 20.0f;
        inEditor = true;

        customCollisions = new int[8];
        loadCustomValues(customSpringCollisions);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        BaseObject obj = other.GetComponent<BaseObject>();

        if (obj)
        {
            fireEvent(customCollisions[obj.prefabID]);
        }

        //Bounce if other object is above when colliding
        if (other.transform.position.y > transform.position.y)
        {
            Rigidbody2D rigidbody = other.gameObject.GetComponent<Rigidbody2D>();

            if (rigidbody)
            {
                if (rigidbody.velocity.y < springVelocity)
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + springVelocity);
            }
        }
    }
}
