using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A base object is something that can be dragged around in the editor, AND becomes an interactable object in the game. Anything in the gameplay will inherit from this class.
public class BaseObject : MonoBehaviour {


    public bool inEditor;
    protected bool clickedOn;
    static bool disableClick;

    public int prefabID;

    protected int[] customCollisions;
    
    public int health;

    //Rigid body tool stuff
    public float mass;
    public float linearDrag;
    public float gravityScale;
    public bool rotate;

    public bool makeDefaults;

    //Animation tool stuff
    public Vector2 overwriteVelocity;
    protected Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        inEditor = true;

        health = 1;

        customCollisions = new int[8];

        defaultCustomValues();

        if (prefabID == 1) transform.eulerAngles = new Vector3(0.0f, 0.0f, 45.0f);

        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        Debug.Log("start called ");

        //Set some default values the first time created. Don't do this after loading, otherwise custom stuff might get overwritten.
        if (makeDefaults)
        { 
        //rb editor default values
        mass = 0.0f;
        linearDrag = 0.0f;
        gravityScale = 0.0f;
        rotate = false;

        overwriteVelocity = new Vector2(0.0f, 0.0f);
        }

    }

    //Transitions from editor state to game state
    public void becomeGameState()
    {
        inEditor = false;

        //Player and enemies always get rigid bodies
        if (prefabID == 3 || prefabID == 4) rigidbody = gameObject.AddComponent<Rigidbody2D>();

        //Check for custom rigidbody
        if (mass != 0.0f || linearDrag != 0.0f || gravityScale != 0.0f || rotate)
        {


            //If there are rigidbody values that aren't default, add a rigid body regardless of the object.
            if (!rigidbody) rigidbody = gameObject.AddComponent<Rigidbody2D>();

            
            
                rigidbody.mass = mass;
                rigidbody.drag = linearDrag;
                rigidbody.gravityScale = gravityScale;

                //By default rotations are off
                if (rotate)
                rigidbody.constraints = RigidbodyConstraints2D.None;
            



        }
    


        //Check for custom velocity (animation editor)
        if (overwriteVelocity.x != 0 || overwriteVelocity.y != 0)
        {


            //If there are movement values that aren't default, add a rigid body regardless of the object.
            if (!rigidbody) rigidbody = gameObject.AddComponent<Rigidbody2D>();


         rigidbody.velocity = overwriteVelocity;
       



        }

        //Turn rigid body on
        if (rigidbody) rigidbody.simulated = true;

        //Default rotations to off
        if (rigidbody && !rotate) rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    // Update is called once per frame
    protected void Update () {
        if (inEditor) return;

        if (overwriteVelocity.x != 0 || overwriteVelocity.y != 0)
        rigidbody.velocity = overwriteVelocity;

	}

    //For clicking to move, clicking to delete, or clicking to customize
    protected virtual void OnMouseOver()
    {
        if (!inEditor) return;

        disableClick = GameObject.Find("CustomUI").GetComponent<CustomizationMenu>().turnedOn;

        if (disableClick) return;

       if (Input.GetMouseButtonDown(0)) //Drag on left click
        {

        
        Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();
        editor.attachObject(gameObject);

        clickedOn = true;
        }
       else if (Input.GetMouseButtonDown(1)) //Destroy on right click
        {
            Destroy(gameObject);
        }
        else if (Input.GetMouseButtonDown(2) || Input.GetKey(KeyCode.Space)) //Customize if middle click
        {

            CustomizationMenu menu = GameObject.Find("CustomUI").GetComponent<CustomizationMenu>();
           menu.selectedObject = gameObject;
           menu.toggle();
            menu.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    //Load custom collisions if the object has it
    public void loadCustomValues(int[] customCollisionList)
    {
        customCollisions = new int[8];

        if (customCollisionList != null)
        {
            Debug.Log("Loading some custom collisions");
            for (int i = 0; i < customCollisions.Length; i++)
            {
                customCollisions[i] = customCollisionList[i];
                Debug.Log(customCollisionList[i]);
            }

        }
        else //If the object has none then default to nothing
        {
            defaultCustomValues();
        }
    }

    public void loadRigidBody()
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody)
        {
            rigidbody.mass = mass;
            rigidbody.drag = linearDrag;
            rigidbody.gravityScale = gravityScale;
            rigidbody.mass = mass;
        }

    }

    //Custom collisions are set to nothing
    protected void defaultCustomValues()
    {
        for (int i = 0; i < customCollisions.Length; i++) customCollisions[i] = -1;
    }


    public void OnCollisionEnter2D(Collision2D other)
    {

        BaseObject obj  = other.gameObject.GetComponent<BaseObject>();

        if (obj)
        {


            //Fire an event based on what object id was collided with
            fireEvent(customCollisions[obj.prefabID]);


        }
        else Debug.Log("Not a base object");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        BaseObject obj = other.gameObject.GetComponent<BaseObject>();

        if (obj)
        {


            //Fire an event based on what object id was collided with
            fireEvent(customCollisions[obj.prefabID]);


        }
        else Debug.Log("Not a base object");
    }

    public void fireEvent(int eventID)
    {

        Debug.Log("Firing custom event " + eventID);

        switch ( eventID)
        {
            case 0: //Get health

                health++;
                break;

            case 1: //Get points
                Player p = GameObject.Find("Player").GetComponent<Player>();
                p.score++;
                p.updateHUD();
                break;

            case 2: //Take damage
                health--;


                if (health <= 0)
                {
                    if (name != "Player") Destroy(gameObject);
                    else GameObject.Find("PlayButton").GetComponent<PlayButton>().startLevel();
                }

                break;

            case 3: //Destroy

                if (name != "Player") Destroy(gameObject);
                else GameObject.Find("PlayButton").GetComponent<PlayButton>().startLevel();

                break;
            case 4: //Bounce
               Rigidbody2D rigidbody =  gameObject.GetComponent<Rigidbody2D>();
                if (rigidbody)
                {
                    if (rigidbody.velocity.y < 20.0f)
                        rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + 20.0f);
                }

                break;

        }
    }

}
