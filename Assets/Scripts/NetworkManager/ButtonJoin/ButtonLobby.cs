using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLobby : MonoBehaviour
{
    public string lobbyID;


    public async void OnClicked(){
        if(gameObject!=null){
            
            await LobbyManager.Instance.JoinLobby(lobbyID);
           
            StartCoroutine(LobbyManager.Instance.LoadingScene());
        }
      
        
    }
}
