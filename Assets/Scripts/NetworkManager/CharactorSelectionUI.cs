using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharactorSelectionUI : MonoBehaviour
{
    public static CharactorSelectionUI instance;

    
    [SerializeField]
    private GameObject loadingScene;

    private int _currenSelected = 0;

    [SerializeField]
    public CharectorSelectionData _charectorSelectionData;

    [SerializeField]
    private TMP_Text attack;

    [SerializeField]
    private TMP_Text health;

    [SerializeField]
    private TMP_Text speed;

    [SerializeField]
    private TMP_Text rateOfFire;
    [SerializeField]
    private Image imageSelected;

    public CharactorSelectionUI()
    {
        instance = this;
    }

    private void Start() {
        
    }


    public async void OnLeftButtonClick(){
        AudioManager.Instance.PlaySoundEffect();
        _currenSelected = _currenSelected - 1 >= 0  ? _currenSelected-1 : _charectorSelectionData.AllPlayer.Count-1;
        Debug.Log("PlayerHasCode: " + _charectorSelectionData.AllPlayer[_currenSelected].hasCodeCharactor);
        await UpdateLobbyManager.Instance.SetSelectedPlayer(_currenSelected,_charectorSelectionData.AllPlayer[_currenSelected].hasCodeCharactor);
        UpdateMap();
    }


    public async void OnRightButtonClick(){
        AudioManager.Instance.PlaySoundEffect();
        _currenSelected = _currenSelected + 1 <=  _charectorSelectionData.AllPlayer.Count-1 ? _currenSelected + 1 : 0;
        Debug.Log("PlayerHasCode: " + _charectorSelectionData.AllPlayer[_currenSelected].hasCodeCharactor);
        await UpdateLobbyManager.Instance.SetSelectedPlayer(_currenSelected,_charectorSelectionData.AllPlayer[_currenSelected].hasCodeCharactor);
        UpdateMap();
    }

    private void UpdateMap()
    {
        PLayerData playerData = _charectorSelectionData.AllPlayer[_currenSelected];
        attack.text = playerData.damage;
        health.text = playerData.health;
        speed.text = playerData.speed;
        rateOfFire.text = playerData.rateOfFire;
        imageSelected.sprite = playerData.ImageCharactor;
    }


    public async void OnReadyButton(){
        AudioManager.Instance.PlaySoundEffect();
        await UpdateLobbyManager.Instance.SetPlayerReady();
    }

    public async void OnExitButton(){
        AudioManager.Instance.PlaySoundEffect();
        await UpdateLobbyManager.Instance.SetPlayerExit();
    }

    public async void OnStartButton(){
        AudioManager.Instance.PlaySoundEffect();
        loadingScene.SetActive(true);
        
        bool suc = await UpdateLobbyManager.Instance.OnPlayerStart();
        await Task.Delay(4000);
        if(suc){
            StartCoroutine(LobbyManager.Instance.LoadingScenceFade());
            SceneManager.LoadScene(LobbyManager.Instance._lobbyData.SenceName);
        }
      

        
    }


}
