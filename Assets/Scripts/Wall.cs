using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public int bounceHouse;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D rigidBody = col.gameObject.GetComponent<Rigidbody2D>(); 
        Vector2 reverse = new Vector2(rigidBody.velocity.x * -1, rigidBody.velocity.y-1);
        rigidBody.AddForce(reverse * bounceHouse);
    }
}
