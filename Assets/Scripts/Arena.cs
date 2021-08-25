using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
   
    private Vector3 homePosition = new Vector3(186, 93,-50);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn(GameObject player)
    {
        StartCoroutine(DelayPop(player));
    }

    IEnumerator DelayPop(GameObject player) {
        player.SetActive(false);
        //Declare a yield instruction.
        WaitForSeconds wait = new WaitForSeconds(1);

        yield return wait; //Pause the loop for 1 second.
        Debug.Log("Done Waiting");
        player.SetActive(true);
        player.transform.position = homePosition;
   }
}
