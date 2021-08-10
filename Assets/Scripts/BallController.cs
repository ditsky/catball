using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{

    private Rigidbody2D ball;
    private Text goalText;

    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<Rigidbody2D> ();
        goalText = GameObject.Find("goalText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "goal")
        {
            goalText.text = "GOALLLLLLLL!!!!!!!!!!!!!!!!!!!";
            StartCoroutine(DelayReset());
        }
    }

    IEnumerator DelayReset() {
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(3);


        Debug.Log("Wait to reset");
        yield return wait; //Pause the loop for 3 seconds.
        Debug.Log("Resetting");
        SceneManager.LoadScene("main");
   }


}
