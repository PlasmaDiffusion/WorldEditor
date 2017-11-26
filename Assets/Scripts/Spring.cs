using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : BaseObject {

    float springVelocity;

	// Use this for initialization
	void Start () {
        springVelocity = 10.0f;
        inEditor = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided");
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
