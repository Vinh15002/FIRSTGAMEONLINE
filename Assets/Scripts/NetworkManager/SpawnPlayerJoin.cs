using System.Collections;
using System.Collections.Generic;

using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using UnityEngine;
using System;


public class SpawnPlayerJoin : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject game;


    
    void Start()
    {
        if(LobbyManager.Instance.IsHost()){
            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.createLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            NetworkObject gameSpawn =  Instantiate(game, Vector3.zero, Quaternion.identity);
            gameSpawn.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
           


        }
        else if(!LobbyManager.Instance.IsHost())
        {


            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.joinedLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            Debug.Log("???" + NetworkManager.Singleton.LocalClientId);
            NetworkManager.Singleton.StartClient();

   

            //Debug.Log("????WTF: " + LobbyManager.Instance._localPlayerData.HasCodePlayer);
            SendTheServerRpc();
            


        }
    }

    [ServerRpc]
    private void SendTheServerRpc()
    {
        NetworkObject gameSpawn = Instantiate(game, Vector3.zero, Quaternion.identity);
        gameSpawn.GetComponent<NetworkObject>().Spawn();
    }

}
