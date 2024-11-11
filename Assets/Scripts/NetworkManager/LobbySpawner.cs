using System.Collections.Generic;
using UnityEngine;

public class LobbySpawner : MonoBehaviour {


    public static LobbySpawner instance;

    


    
    [SerializeField] private List<LobbyPlayer> _players;

    public LobbySpawner()
    {
        instance = this;
    }

    private void Start() {
        
    }




    public void OnLobbyUpdated(List<LobbyPlayerData> playerDatas){
        
        for(int i = 0; i < playerDatas.Count; i++){
            LobbyPlayerData data = playerDatas[i];
            _players[i].SetData(data);
        }

    }
}