using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BaseObject {

    public int worth;

	// Use this for initialization
	void Start () {
        inEditor = true;
        worth = 1;
	}
	
	
    void OnTriggerEnter2D(Collider2D other)
    {
        BaseObject obj = other.GetComponent<BaseObject>();

            if (obj.prefabID == 3)
            {
            Player player = other.GetComponent<Player>();
            player.score+= worth;
            player.updateHUD();
            Destroy(gameObject);
            }

    }
}
