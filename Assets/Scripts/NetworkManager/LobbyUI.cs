using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    private Image _mapImage;



    [SerializeField]
    private TMP_Text text;



    [SerializeField]
    private Button left;

    [SerializeField]
    private Button right;

    [SerializeField]
    private Button start;


    public LobbyUI()
    {
        instance = this;
    }

    private float timeToPress = 1.5f;

    void  Start()
    {
        Invoke("Loading", 2f);
        
    }

    public void Loading(){
        if(!LobbyManager.Instance.IsHost()){
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
        }
    }
    private void FixedUpdate() {
        timeToPress-=Time.deltaTime;
    }


    public async void OnReadyPress(){
        bool succeed = await GameLobbyController.Instance.SetPlayerRead();
    }


    public async void OnLeftButtonClick(){

        if(timeToPress <= 0){
            AudioManager.Instance.PlaySoundEffect();
            _currentMapIndex = _currentMapIndex - 1 >= 0  ? _currentMapIndex-1 : _mapselectionData.Maps.Count-1;
       
            await UpdateLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapselectionData.Maps[_currentMapIndex].SenceName);
            UpdateMap();
            timeToPress = 1.5f;
        }
       
    }

    public async void OnRightButtonClick(){
        if(timeToPress <= 0){
            AudioManager.Instance.PlaySoundEffect();
            _currentMapIndex = _currentMapIndex + 1 <= _mapselectionData.Maps.Count-1 ? _currentMapIndex + 1 :  0;
            await UpdateLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapselectionData.Maps[_currentMapIndex].SenceName);
            UpdateMap();
            timeToPress = 1.5f;
        }
    }

    private void UpdateMap()
    {
       
        _mapImage.sprite = _mapselectionData.Maps[_currentMapIndex].ImageMap;


        _mapName.text = _mapselectionData.Maps[_currentMapIndex].MapName;
    }

    public void SetMap(int mapIndex)
    {
        _currentMapIndex = mapIndex;
        UpdateMap();
    }

    public void SetStartButton(bool v)
    {   if(v){
            if(LobbyManager.Instance.IsHost()){
                start.gameObject.SetActive(true);
            }
        }
        else {
            start.gameObject.SetActive(false);
        }
        
        
    }


    public void OnstartButton(){
        start.gameObject.SetActive(true);
    }
}
