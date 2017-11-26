using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour {

    bool turnedOn;
    public GameObject selectedObject;

    void Start()
    {
        turnedOn = false;
    }

	// Use this for initialization
	public void toggle()
    {
        if (!turnedOn)
        {
            turnedOn = true;

            loadCustomCollisions();
        }
        else if (turnedOn)
        {
            turnedOn = false;
        }
    }

    public void loadCustomCollisions()
    {
        int objectId = selectedObject.GetComponent<BaseObject>().prefabID;

        //Object is player
        if (objectId == 3)
        {
        GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Player.customPlayerCollisions[3];
        GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Player.customPlayerCollisions[4];
        GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Player.customPlayerCollisions[6];
        GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Player.customPlayerCollisions[5];
        GameObject.Find("DropdownGround").GetComponent<Dropdown>().value = Player.customPlayerCollisions[5];
        }

        //Object is enemy
        if (objectId == 4)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[3];
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[4];
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[6];
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Enemy.customEnemyCollisions[5];
        }

        //Object is coin
        if (objectId == 6)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Coin.customCoinCollisions[3];
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Coin.customCoinCollisions[4];
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Coin.customCoinCollisions[6];
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Coin.customCoinCollisions[5];
        }

        //Object is spring
        if (objectId == 5)
        {
            GameObject.Find("DropdownPlayer").GetComponent<Dropdown>().value = Spring.customSpringCollisions[3];
            GameObject.Find("DropdownEnemies").GetComponent<Dropdown>().value = Spring.customSpringCollisions[4];
            GameObject.Find("DropdownCoins").GetComponent<Dropdown>().value = Spring.customSpringCollisions[6];
            GameObject.Find("DropdownSpring").GetComponent<Dropdown>().value = Spring.customSpringCollisions[5];
        }
    }

    //Applies all custom collisions according to the Dropdown menus
    public void applyCustomCollisions()
    {
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
}
