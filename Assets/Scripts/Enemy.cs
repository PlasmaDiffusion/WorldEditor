using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseObject {

    private Rigidbody2D rigidbody2D;
    float horizontalVelocity;
    GameObject playerReference;

    // Use this for initialization
    void Start () {
        inEditor = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        horizontalVelocity = -1.0f; //* Some rigid body speed thing

   
    }
	
	// Update is called once per frame
	void Update () {

        if (inEditor) return;

        rigidbody2D.velocity = new Vector2(horizontalVelocity, rigidbody2D.velocity.y);

       

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //If its below then its ground. Don't make it ground the direction. Otherwise it isn't ground, so turn around
        if (other.transform.position.y + 0.5f >= transform.position.y)
        {
            Debug.Log("Collided");
        horizontalVelocity *= -1.0f;
        //transform.position += new Vector3(horizontalVelocity, 0.0f, 0.0f);
        
        }

        Player player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            //Check if player landed on top
            if (other.transform.position.y -0.5f > transform.position.y)
            {
                player.score += 5;
                player.updateHUD();
                Rigidbody2D otherRigidbody= other.gameObject.GetComponent<Rigidbody2D>();
                otherRigidbody.velocity = new Vector3(otherRigidbody.velocity.x, 10.0f);
                Destroy(gameObject);
            }
            else //If not they get damaged
            {
                player.health -= 1;
                player.updateHUD();
            }
        }
    }
}