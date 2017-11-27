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
    protected int[] customInputs;

    public int health;

	// Use this for initialization
	void Start () {
        inEditor = true;

        health = 1;


        customCollisions = new int[8];

        defaultCustomValues();

        if (prefabID == 1) transform.eulerAngles = new Vector3(0.0f, 0.0f, 45.0f);

	}

    // Update is called once per frame
    protected void Update () {
        if (inEditor) return;

	}

    //For clicking to move, clicking to delete, or clicking to customize
    protected void OnMouseOver()
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

    //Custom collisions are set to nothing
    protected void defaultCustomValues()
    {
        for (int i = 0; i < customCollisions.Length; i++) customCollisions[i] = -1;
    }

    protected void detectSpecialInputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            fireEvent(customInputs[0]);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            
        }
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

    public void showWindow()
    {

    }
}
