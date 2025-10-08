using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = (Player)GameObject.Find("Jack").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Jack")
        {
            Destroy(gameObject);
            player.CollectPumpkin();
        }
    }

}
