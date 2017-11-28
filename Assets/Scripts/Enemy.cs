using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseObject {
    
    float horizontalVelocity;
    GameObject playerReference;
    public static int[] customEnemyCollisions;

    // Use this for initialization
    void Start () {
        inEditor = true;
        rigidbody = GetComponent<Rigidbody2D>();
        horizontalVelocity = -1.0f; //* Some rigid body speed thing


        customCollisions = new int[8];
        loadCustomValues(customEnemyCollisions);
     

    }
	
	// Update is called once per frame
	void Update () {

        if (inEditor) return;

        //Automatically move unless custom velocity is used
        if (overwriteVelocity.x == 0 && overwriteVelocity.y == 0)
            rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        else
        {
            rigidbody.velocity = overwriteVelocity;
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Fire an event based on what object id was collided with
        BaseObject obj = other.gameObject.GetComponent<BaseObject>();

        if (obj) fireEvent(customCollisions[obj.prefabID]);


        //If its below then its ground. Don't make the enemy turn around if ground. Otherwise it isn't ground, so turn around
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

                if (player.health < 0)
                { 
                //End game here
                GameObject.Find("PlayButton").GetComponent<PlayButton>().startLevel();
                }
            }
        }
    }
}