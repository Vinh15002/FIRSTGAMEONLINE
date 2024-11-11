using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    
    [SerializeField]
    private Transform positonSpawn;

    public void OnClicked(){
      
            
        SendTheServerRpc();
        
        
    }
    [ServerRpc(RequireOwnership =false)]
    private void SendTheServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject game = Instantiate(playerPrefab,positonSpawn.position, Quaternion.identity);
        game.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}
