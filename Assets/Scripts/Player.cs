using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BaseObject {

    private Rigidbody2D rigidbody;
    private bool grounded;

    private float jumpVel;

    public int score;
    public int health;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        inEditor = true;

        score = 0;
        health = 5;

        updateHUD();
	}
	
	// Update is called once per frame
	void Update () {


        if (inEditor) return;

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

        if (Input.GetKey(KeyCode.W) && grounded)
        {
            jumpVel = 8.0f;
           
        }
        else jumpVel = 0.0f;

        rigidbody.velocity += new Vector2(0.0f, jumpVel);
    }

    public void updateHUD()
    {
        GameObject.Find("Score Text").GetComponent<Text>().text = "Score: " + score.ToString();
        GameObject.Find("Health Text").GetComponent<Text>().text = "Health: " + health.ToString();
    }
}
