using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : BaseObject {

    // Use this for initialization
    void Start()
    {
        inEditor = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        BaseObject obj = other.GetComponent<BaseObject>();

        if (obj.prefabID == 3)
        {
            //End game here
            GameObject.Find("PlayButton").GetComponent<PlayButton>().startLevel();

        }

    }
}
