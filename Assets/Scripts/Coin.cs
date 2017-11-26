using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BaseObject {

    public int worth;

    public static int[] customCoinCollisions;

    // Use this for initialization
    void Start () {
        inEditor = true;
        worth = 1;

        customCollisions = new int[8];
        loadCustomValues(customCoinCollisions);
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
