using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseObject {
    
    private bool grounded;

    private float jumpVel;

    public int score;

    public static int[] customPlayerCollisions;

    public LayerMask collisionLayer;

    private float lastYVelocity;
    private float consistentVelocity;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        inEditor = true;

        //Jumping values
        lastYVelocity = 0.0f;
        consistentVelocity = 0.0f;

        //Hud values
        score = 0;
        health = 5;

        updateHUD();

        customCollisions = new int[8];
        loadCustomValues(customPlayerCollisions);

        name = "Player";
	}
	
	// Update is called once per frame
	void Update () {


        if (inEditor) return;


        //Check if grounded. Determined by if the y velocity is consistent for enough frames
        if (rigidbody.velocity.y == lastYVelocity)
        {
            consistentVelocity += Time.deltaTime;

            if (consistentVelocity > 0.2f) grounded = true;
        }
        else
        {
            grounded = false;
            consistentVelocity = 0.0f;
        }


        if (Input.GetKey(KeyCode.D))
        {
           rigidbody.velocity = new Vector2(5.0f, rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.velocity = new Vector2(-5.0f, rigidbody.velocity.y);
        }

        if (Input.GetKey(KeyCode.W) && grounded)
        {
            jumpVel = 8.0f;
            consistentVelocity = 0.0f;
            grounded = false;
           
        }
        else jumpVel = 0.0f;

        rigidbody.velocity += new Vector2(0.0f, jumpVel);


        lastYVelocity = rigidbody.velocity.y;
    }
    

    public void updateHUD()
    {
        GameObject.Find("Score Text").GetComponent<Text>().text = "Score: " + score.ToString();
        GameObject.Find("Health Text").GetComponent<Text>().text = "Health: " + health.ToString();
    }
}
