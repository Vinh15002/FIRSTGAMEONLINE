using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject _mainScreen;
    [SerializeField] private GameObject _joinScreen;

    [SerializeField] private TMP_InputField _textCode;

    public async void OnHostClicked(){


        bool suc = await GameLobbyController.Instance.CreateLobby();
        
        
       
        if(suc){
            await SceneManager.LoadSceneAsync("Lobby");
        }
    }

    public void OnJoinClicked(){
        _mainScreen.SetActive(false);
        _joinScreen.SetActive(true);
    }

    public async void OnSubmitCode(){
        string code = _textCode.text;
        code = code.TrimEnd();
        Debug.Log($"Code: {code}");
        bool suc = await GameLobbyController.Instance.JoinLobby(code);
        if(suc){
            await SceneManager.LoadSceneAsync("Lobby");
        }
    }


}
