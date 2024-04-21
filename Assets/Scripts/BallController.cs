using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class BallController : NetworkBehaviour
{

    private Rigidbody2D disc;
    private SoundEffects soundEffects;

    // Start is called before the first frame update
    void Start()
    {
        disc = GetComponent<Rigidbody2D> ();
        soundEffects = GetComponent<SoundEffects>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "goal")
        {
            soundEffects.Goal();
            StartCoroutine(DelayReset());
        }
        else if (col.gameObject.tag == "Player")
        {
            if (true) return;

            disc.mass = 0;
            ulong clientId = col.gameObject.GetComponent<NetworkObject>().OwnerClientId;

            GetComponent<NetworkObject>().ChangeOwnership(clientId);

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{clientId}
                }
            };
            
            ConnectToPlayerClientRpc(clientRpcParams);
        }
    }

    [ClientRpc]
    public void ConnectToPlayerClientRpc(ClientRpcParams clientRpcParams = default)
    {

        // Create a joint to connect the disc to the player
        RelativeJoint2D joint = gameObject.AddComponent<RelativeJoint2D>();

        // conects the joint to the other object
        joint.connectedBody = disc;

        // Stops objects from continuing to collide and creating more joints
        joint.enableCollision = false;

        // Slightly distance the disc from the player sprite
        joint.linearOffset = new Vector2(0, -0.3f);
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
