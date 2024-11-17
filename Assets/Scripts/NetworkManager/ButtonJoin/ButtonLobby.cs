using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLobby : MonoBehaviour
{
    public string lobbyID;


    public async void OnClicked(){
        if(gameObject!=null){
            gameObject.SetActive(false);
            await Task.Delay(1000);
          
            await LobbyManager.Instance.JoinLobby(lobbyID);

            await SceneManager.LoadSceneAsync("Lobby");
            
        }
      
        
    }
}
