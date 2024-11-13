using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.UIElements;

public class HandleApprovelNetwork : MonoBehaviour
{
    public Transform position;

    private const int MaxPlayer = 4;

    private void Start() {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("Connect Approval");
        response.Approved = true;
        response.CreatePlayerObject = false;
        Debug.Log("HAS CODE: " + LobbyManager.Instance._localPlayerData.HasCodePlayer);
        response.PlayerPrefabHash = LobbyManager.Instance._localPlayerData.HasCodePlayer;
        response.Position = position.position;
        if(NetworkManager.Singleton.ConnectedClients.Count >= MaxPlayer){
            response.Approved = false;
            response.Reason = "Server is FULL";
        }

        response.Pending = false;
    }
}
