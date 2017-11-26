using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Just a place to die...
public class OutOfBounds : MonoBehaviour {


    void OnCollisionEnter2D(Collision2D other)
    {
        //Destroy object. If its the player then end the game (but not in the editor)
        if (other.gameObject.name == "Player" && !other.gameObject.GetComponent<BaseObject>().inEditor) GameObject.Find("PlayButton").GetComponent<PlayButton>().startLevel();
        else Destroy(other.gameObject);
    }
}
