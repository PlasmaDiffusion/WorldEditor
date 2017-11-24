using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D rigidbody;
    private bool grounded;

    private float jumpVel;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (rigidbody.velocity.y > -0.2f && rigidbody.velocity.y < 0.2f) grounded = true;

        else grounded = false;

        if (Input.GetKey(KeyCode.D))
        {
           rigidbody.velocity = new Vector2(5.0f, rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.velocity = new Vector2(-5.0f, rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            jumpVel = 8.0f;
           
        }
        else jumpVel = 0.0f;

        rigidbody.velocity += new Vector2(0.0f, jumpVel);
    }
}
