using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour {

    public bool turnedOn;
    public GameObject selectedObject;

    private GameObject exitButton;
    private GameObject collisionButton;
    private GameObject rigidBodyButton;

    void Start()
    {
        turnedOn = false;

        exitButton = transform.GetChild(0).gameObject;
        collisionButton = transform.GetChild(1).gameObject;
        rigidBodyButton = transform.GetChild(2).gameObject;

        exitButton.SetActive(false);
        collisionButton.SetActive(false);
        rigidBodyButton.SetActive(false);

    }


	// Use this for initialization
	public void toggle()
    {
        if (!turnedOn)
        {
            turnedOn = true;

            exitButton.SetActive(true);
            collisionButton.SetActive(true);
            rigidBodyButton.SetActive(true);


            
            hideWindows();
        }
        else if (turnedOn)
        {
            turnedOn = false;

            exitButton.SetActive(false);
            collisionButton.SetActive(false);
            rigidBodyButton.SetActive(false);


        }
    }
    private void hideWindows()
    {
        collisionButton.transform.GetChild(1).gameObject.SetActive(false);
        rigidBodyButton.transform.GetChild(1).gameObject.SetActive(false);
    }


    public void showWindow(int windowNumber)
    {

        hideWindows();

        //Collision window
        if (windowNumber == 1)
        {
            collisionButton.transform.GetChild(1).gameObject.SetActive(true);
            loadCustomCollisions();

        }

        //Rigidbody window
        if (windowNumber == 2)
        {
            rigidBodyButton.transform.GetChild(1).gameObject.SetActive(true);
            loadRigidbody();
        }
        
    }

    public void loadCustomCollisions()
    {
        if (selectedObject == null)
        {
            Debug.Log("Selected object got destroyed.");
            return;
        }

        int objectId = selectedObject.GetComponent<BaseObject>().prefabID;

        //Object is player
        if (objectId == 3 && Player.customPlayerCollisions != null)
        {
        GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Player.customPlayerCollisions[3] + 1;
        GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Player.customPlayerCollisions[4] + 1;
        GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Player.customPlayerCollisions[6] + 1;
        GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Player.customPlayerCollisions[5] + 1;
        GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Player.customPlayerCollisions[0] + 1;
        }

        //Object is enemy
        else if (objectId == 4 && Enemy.customEnemyCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[0] + 1;
        }

        //Object is coin
        else if (objectId == 6 && Coin.customCoinCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Coin.customCoinCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Coin.customCoinCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Coin.customCoinCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Coin.customCoinCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Coin.customCoinCollisions[0] + 1;
        }

        //Object is spring
        else if (objectId == 5 && Spring.customSpringCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Spring.customSpringCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Spring.customSpringCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Spring.customSpringCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Spring.customSpringCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Spring.customSpringCollisions[0] + 1;
        }

        //General object
        else
        {
            Editor editor = GameObject.Find("EditorObject").GetComponent<Editor>();

            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = editor.customGeneralCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = editor.customGeneralCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = editor.customGeneralCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = editor.customGeneralCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = editor.customGeneralCollisions[0] + 1;
        }
    }

    //Applies all custom collisions according to the Dropdown menus
    public void applyCustomCollisions()
    {
        if (selectedObject == null)
        {
            Debug.Log("Selected object got destroyed.");
            return;
        }
        

        int eventValue;

        //When colliding with the player
        eventValue = GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value;

        setCustomCollision(3, eventValue);

        //When colliding with enemies
        eventValue = GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value;

        setCustomCollision(4, eventValue);

        //When colliding with coins
        eventValue = GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value;

        setCustomCollision(6, eventValue);

        //When colliding with springs
        eventValue = GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value;

        setCustomCollision(5, eventValue);

        //When colliding with the Ground objects
        eventValue = GameObject.Find("DropdownGround").GetComponent<Dropdown>().value;

        setCustomCollision(0, eventValue);
        setCustomCollision(1, eventValue);
        setCustomCollision(2, eventValue);
        setCustomCollision(7, eventValue);
    }

    void setCustomCollision(int objectTypeCollidingWith, int spinnerValue)
    {
        if (selectedObject == null)
        {
            Debug.Log("Selected object got destroyed.");
            return;
        }

        //First determine the selected object we're changing
        switch (selectedObject.GetComponent<BaseObject>().prefabID)
        {

            case 3: //Player
                Player.customPlayerCollisions[objectTypeCollidingWith] = spinnerValue - 1; //It's -1 because a negative value means empty. The first element of the Dropdown at 0 is empty.
                break;

            case 4: //Enemies
                Enemy.customEnemyCollisions[objectTypeCollidingWith] = spinnerValue - 1;
                break;
                

            case 6: // Coins
                Coin.customCoinCollisions[objectTypeCollidingWith] = spinnerValue - 1;
                break;
                
            case 5: // Spring
                Spring.customSpringCollisions[objectTypeCollidingWith] = spinnerValue - 1;
                break;
            default: //Any other object
                GameObject.Find("EditorObject").GetComponent<Editor>().customGeneralCollisions[objectTypeCollidingWith] = spinnerValue - 1;
                break;

        }
    }

    void loadRigidbody()
    {
        if (selectedObject == null)
        {
            Debug.Log("Selected object got destroyed.");
            return;
        }

        BaseObject obj = selectedObject.GetComponent<BaseObject>();

                if (obj == null) return;

        GameObject.Find("MassInput").GetComponent<InputField>().text = obj.mass.ToString();
        GameObject.Find("GravityInput").GetComponent<InputField>().text = obj.gravityScale.ToString();
        GameObject.Find("LinearDragInput").GetComponent<InputField>().text = obj.linearDrag.ToString();
        GameObject.Find("RotateToggle").GetComponent<Toggle>().isOn = obj.rotate;
    }

    public void setRigidbody()
    {
        if (selectedObject == null)
        {
            Debug.Log("Selected object got destroyed.");
            return;
        }

        BaseObject obj = selectedObject.GetComponent<BaseObject>();

        if (obj == null) return;

        //Read in rigid body input values. Use TryParse in case a field was left empty.
        float newMass;
        if (float.TryParse(GameObject.Find("MassInput").GetComponent<InputField>().text, out newMass))
            obj.mass = newMass;

        float newGravity;
        if (float.TryParse(GameObject.Find("GravityInput").GetComponent<InputField>().text, out newGravity))
            obj.gravityScale = newGravity;

        float newDrag;
        if(float.TryParse(GameObject.Find("LinearDragInput").GetComponent<InputField>().text, out newDrag))
        obj.linearDrag = newDrag;

        
        obj.rotate = GameObject.Find("RotateToggle").GetComponent<Toggle>().isOn;



    }
}
