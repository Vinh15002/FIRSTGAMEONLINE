using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class LobbyUI : MonoBehaviour
{

    public static LobbyUI instance;
    

    private int _currentMapIndex = 0;

    [SerializeField]
    private MapSelectionData _mapselectionData;

    [SerializeField]
    private TMP_Text _mapName;

    [SerializeField]
    private RawImage _mapImage;



    [SerializeField]
    private TMP_Text text;

    public LobbyUI()
    {
        instance = this;
    }

    [SerializeField]
    private Button left;
    [SerializeField]
    private Button right;

    [SerializeField]
    private Button start;

    // Start is called before the first frame update
    void Start()
    {
        text.text = $"Lobby Code: {GameLobbyController.Instance.GetLobbyCode()}";
        if(!GameLobbyController.Instance.IsHost){
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
        }
    }


    public async void OnReadyPress(){
        bool succeed = await GameLobbyController.Instance.SetPlayerRead();
    }


    public async void OnLeftButtonClick(){
        _currentMapIndex = _currentMapIndex - 1 >= 0  ? _currentMapIndex-1 : 0;
       
        await GameLobbyController.Instance.SetSelectedMap(_currentMapIndex, _mapselectionData.Maps[_currentMapIndex].SenceName);
        UpdateMap();
    }

    public async void OnRightButtonClick(){
        _currentMapIndex = _currentMapIndex + 1 <= _mapselectionData.Maps.Count-1 ? _currentMapIndex + 1 :  _mapselectionData.Maps.Count-1;
        await GameLobbyController.Instance.SetSelectedMap(_currentMapIndex, _mapselectionData.Maps[_currentMapIndex].SenceName);
        UpdateMap();
    }

    private void UpdateMap()
    {
       
        _mapImage.color= _mapselectionData.Maps[_currentMapIndex].MapThumnail;


        _mapName.text = _mapselectionData.Maps[_currentMapIndex].MapName;
    }

    public void SetMap(int mapIndex)
    {
        _currentMapIndex = mapIndex;
        UpdateMap();
    }

    public void SetStartButton()
    {
        if(GameLobbyController.Instance.IsHost && start != null){
            start.gameObject.SetActive(true);
        }
        
    }


    public async void OnstartButton(){
        await GameLobbyController.Instance.StartGame();
    }
}
