
using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkSpawn : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject player01;

    [SerializeField]
    private NetworkObject player02;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        ProcessSpawn();
    }

    private void ProcessSpawn()
    {
       
        //if (IsOwner)
        //{
        //    SendingTheServerRpc();
        //}
        //else if (IsHost) {
        //    SendingThe2ServerRpc();
        //}


    }

    [ServerRpc]
    private void SendingThe2ServerRpc(ServerRpcParams serverRpcParams = default)
    {
        NetworkObject game = Instantiate(player01, Vector3.zero, Quaternion.identity);
        game.GetComponent<NetworkObject>().SpawnAsPlayerObject(serverRpcParams.Receive.SenderClientId);
    }

    [ServerRpc]
    private void SendingTheServerRpc(ServerRpcParams serverRpcParams=default)
    {
        Debug.Log("?????");
        NetworkObject game = Instantiate(player01, Vector3.zero, Quaternion.identity);
        game.GetComponent<NetworkObject>().SpawnAsPlayerObject(serverRpcParams.Receive.SenderClientId);
    }
}
