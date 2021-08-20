
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using UnityEngine;

namespace Network
{
    public class Breacher : NetworkBehaviour
    {

        private ulong playerId;
        private bool isServer;

        private NetworkVariableUInt players = new NetworkVariableUInt(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        }, 
            0U);
        
        public NetworkDictionary<ulong, Vector3> PlayerPositions = new NetworkDictionary<ulong, Vector3>(new NetworkVariableSettings 
        {
            WritePermission = NetworkVariablePermission.Everyone,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public override void NetworkStart()
        {
            players.Value++;
            
            NetworkObject player = gameObject.GetComponent<NetworkObject>();
            playerId = players.Value;

            Vector3 homePosition = new Vector3(186, 93,-50);
            PlayerPositions.Add(playerId, homePosition);

            isServer = NetworkManager.Singleton.IsServer;
            Scout scout = gameObject.GetComponent<Scout>();
            if (!IsLocalPlayer)
            {
                scout.isLocalPlayer = false;
                InvokeRepeating("SendPosition", 1.0f, 0.1f);
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ulong clientId, Vector3 position)
        {
            Debug.Log("Submitting Position Request For: " + clientId);
            PlayerPositions[clientId] = position;
        }

        void SendPosition()
        {
            SubmitPositionRequestServerRpc(playerId, transform.position);
        }
        
        void Update()
        {
            if (!isServer)
            {
                Debug.Log("Updating Player Position For: " + playerId);
                //transform.position = PlayerPositions[playerId];
            }
        }
    }
}