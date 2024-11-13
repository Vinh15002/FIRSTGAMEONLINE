using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ButtonLobby : MonoBehaviour
{
    public string lobbyID;


    public async void OnClicked(){
        if(gameObject!=null){
            
            await LobbyManager.Instance.JoinLobby(lobbyID);
            await Task.Delay(1000);
            StartCoroutine(LobbyManager.Instance.LoadingScene());
        }
      
        
    }
}
