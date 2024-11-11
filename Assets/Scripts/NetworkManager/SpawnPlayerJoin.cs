using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

public class SpawnPlayerJoin : MonoBehaviour
{
    
    void Start()
    {
        if(LobbyManager.Instance.IsHost()){
            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.createLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }
        else{
            RelayServerData relayServerData = new RelayServerData(UpdateLobbyManager.Instance.joinedLobby, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
