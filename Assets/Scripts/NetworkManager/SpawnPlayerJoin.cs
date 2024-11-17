using System.Collections;
using System.Collections.Generic;

using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Unity.Services.Lobbies;
using UnityEngine.UIElements;


public class SpawnPlayerJoin : NetworkBehaviour
{

    
    
    void Start()
    {
    
 
        if (LobbyManager.Instance.IsHost()){
            
            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.createLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            Debug.Log("Player HardCode: " + LobbyManager.Instance._localPlayerData.HasCodePlayer);


            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(LobbyManager.Instance._localPlayerData.HasCodePlayer);
            NetworkManager.Singleton.StartHost();
            Debug.Log("Host Joined");





        }
        else if(!LobbyManager.Instance.IsHost())
        {


            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.joinedLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            Debug.Log("Player HardCode: " + LobbyManager.Instance._localPlayerData.HasCodePlayer);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(LobbyManager.Instance._localPlayerData.HasCodePlayer);
            NetworkManager.Singleton.StartClient();
            Debug.Log("Client Joined");








            //Debug.Log("????WTF: " + LobbyManager.Instance._localPlayerData.HasCodePlayer);
            //SendTheServerRpc();
        }
    }

    
}
