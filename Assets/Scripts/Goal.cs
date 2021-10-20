using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public int team;
    private Text goalText;
    
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
        if (col.gameObject.tag == "disc")
        {
            goalText = GameObject.Find("goalText").GetComponent<Text>();
            goalText.text = "Team " + team + " GOALLL!";
        }
    }
}
