using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingMods : MonoBehaviour
{
    // Disc Physics Object
    private Rigidbody2D disc;
    // Player Physics Object
    private Rigidbody2D scout;

    // Enables training mods for this player on game disc
    public void Load(Rigidbody2D Disc)
    {
        disc = Disc;
        scout = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If Player Right Clicks
        if (Input.GetMouseButtonDown(1))
        {
            disc.velocity = Vector2.zero;
            disc.transform.position = gameObject.transform.position;
        }

        // If player Middle Clicks
        if (Input.GetMouseButtonDown(2))
        {
            disc.velocity = (scout.velocity - disc.velocity) * 3;  
        }
            
    }
}
