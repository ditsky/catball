using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UNET;
using UnityEngine;

public class ConnectionManager : NetworkBehaviour
{

    // Simple starting positions
    Vector3 host = new Vector3(16, 210, -50);
    Vector3 client = new Vector3(-3, -220, -50);

    public GameObject connectionButtonPanel;
    
    public string ipAddress = "73.89.31.27";
    UNetTransport transport;

    public void Host() {
        connectionButtonPanel.SetActive(false);
        
        // Activate callback for potentially joining clients
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void StartServer() {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartServer();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientID, NetworkManager.ConnectionApprovedDelegate callback)
    {
        // Some password verfication for joining not really needed, also hard coded
        bool approve = System.Text.Encoding.ASCII.GetString(connectionData) == "catball";
        callback(true, null, approve, client, Quaternion.identity);
    }

    public void Join() {
        
        // Set the ip address before joining
        transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        transport.ConnectAddress = ipAddress;

        connectionButtonPanel.SetActive(false);

        // Join CATBALL!!! 
        NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("catball");
        NetworkManager.Singleton.StartClient();

    }

    public void IPAddressChanged(string newAddress)
    {
        // Get the ip address from the actual unity input field
        this.ipAddress = newAddress;
    }
}
