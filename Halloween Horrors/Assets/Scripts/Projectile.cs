using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{       
    
    Rigidbody2D rbody;
    
    void Awake() //called when a gameobject is initialised, initialising rigidbody2d
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    

    // Update is called once per frame
    public void Launch (Vector2 direction, float force)
    {
        rbody.AddForce(direction * force);   
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }

}
