using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class ClientConnectHandler : NetworkBehaviour
{
    public List<uint> AlternatePlayerPrefabs;

    public void SetClientPlayerPrefab(int index)
    {
        if (index > AlternatePlayerPrefabs.Count)
        {
            Debug.LogError($"Trying to assign player Prefab index of {index} when there are only {AlternatePlayerPrefabs.Count} entries!");
            return;
        }
        if (NetworkManager.IsListening || IsSpawned)
        {
            Debug.LogError("This needs to be set this before connecting!");
            return;
        }
        NetworkManager.NetworkConfig.ConnectionData = System.BitConverter.GetBytes(index);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.ConnectionApprovalCallback = ConnectionApprovalCallback;
        }
    }

    private void ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        
        var playerPrefabIndex = System.BitConverter.ToInt32(request.Payload);
        Debug.Log("INDEX:" + playerPrefabIndex);
        if (AlternatePlayerPrefabs.Count > playerPrefabIndex)
        {
            response.PlayerPrefabHash = AlternatePlayerPrefabs[playerPrefabIndex];
        }
        else
        {
            Debug.LogError($"Client provided player Prefab index of {playerPrefabIndex} when there are only {AlternatePlayerPrefabs.Count} entries!");
            return;
        }
        // Continue filling out the response
    }
}
