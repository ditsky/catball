using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using Unity.Netcode;


public class SteamLobby : MonoBehaviour {

    /*public GameObject connectionButtonPanel;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<GameLobbyJoinRequested_t> lobbyJoinRequested;

    public bool serverUp = false;
    private const string HostAddressKey = "HostAddress";

    void Start()
    {
        if (!SteamManager.Initialized) { return; } 
    
        string name = SteamFriends.GetPersonaName();
        Debug.Log(name);

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        lobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
    }


    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        NetworkManager.Singleton.StartHost();
        connectionButtonPanel.SetActive(false);
        serverUp = true;
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (serverUp) { return; }

        connectionButtonPanel.SetActive(false);

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = hostAddress
        ; //takes string
        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectPort = 12345;
        NetworkManager.Singleton.StartClient();
    }

    private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 6);
    }*/
    
}