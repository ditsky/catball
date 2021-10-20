using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    private Vector3 homePosition = new Vector3(186, 93,-50);
    private GameObject disc;
    public Dictionary<int, List<int>> playerTeams = new Dictionary<int, List<int>>()
    {
      // Team 0
      {0, new List<int>()},
      // Team 1
      {1, new List<int>()}
    };
    public int playerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
      disc = GameObject.Find("Disc");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Handles player initialization when they enter the game
    public void JoinGame(Player player) {
        // increment player count
        playerCount++;

        // set player team based on an even or odd player count to alternate teams
        int team = playerCount % 2;

        // set player data
        player.id = playerCount;
        player.team = team;

        // Add new player to the player map
        playerTeams[team].Add(player.id);
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
