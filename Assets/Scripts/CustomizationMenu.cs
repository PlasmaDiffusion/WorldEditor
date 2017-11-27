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

        if (windowNumber == 1)
        {
            collisionButton.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (windowNumber == 2)
        {
            rigidBodyButton.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (windowNumber == 3)
        {

        }
    }

    public void loadCustomCollisions()
    {
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
        if (objectId == 4 && Enemy.customEnemyCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[0] + 1;
        }

        //Object is coin
        if (objectId == 6 && Coin.customCoinCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Coin.customCoinCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Coin.customCoinCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Coin.customCoinCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Coin.customCoinCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Coin.customCoinCollisions[0] + 1;
        }

        //Object is spring
        if (objectId == 5 && Spring.customSpringCollisions != null)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Spring.customSpringCollisions[3] + 1;
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Spring.customSpringCollisions[4] + 1;
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Spring.customSpringCollisions[6] + 1;
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Spring.customSpringCollisions[5] + 1;
            GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Spring.customSpringCollisions[0] + 1;
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

        //When colliding with the 3 Ground objects
        eventValue = GameObject.Find("DropdownGround").GetComponent<Dropdown>().value;

        setCustomCollision(0, eventValue);
        setCustomCollision(1, eventValue);
        setCustomCollision(2, eventValue);
    }

    void setCustomCollision(int objectTypeCollidingWith, int spinnerValue)
    {
        //First determine the selected object we're changing
        switch (selectedObject.GetComponent<BaseObject>().prefabID)
        {

            case 3: //Player
                Player.customPlayerCollisions[objectTypeCollidingWith] = spinnerValue - 1; //It's -1 because a negative value means empty. The first element of the Dropdown is for empty.
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

        }
    }

    void loadRigidbody()
    {
        BaseObject obj = gameObject.GetComponent<BaseObject>();

        GameObject.Find("MassInput").GetComponent<InputField>().text = obj.mass.ToString();
        GameObject.Find("GravityInput").GetComponent<InputField>().text = obj.gravityScale.ToString();
        GameObject.Find("LinearDragInput").GetComponent<InputField>().text = obj.linearDrag.ToString();
        GameObject.Find("RotateToggle").GetComponent<Toggle>().isOn = obj.rotate;
    }

    public void setRigidbody()
    {
        
        BaseObject obj =gameObject.GetComponent<BaseObject>();

        obj.mass = float.Parse(GameObject.Find("MassInput").GetComponent<InputField>().text);
        obj.gravityScale = float.Parse(GameObject.Find("GravityInput").GetComponent<InputField>().text);
        obj.linearDrag = float.Parse(GameObject.Find("LinearDragInput").GetComponent<InputField>().text);
        obj.rotate = (GameObject.Find("RotateToggle").GetComponent<Toggle>().isOn);
    }
}
