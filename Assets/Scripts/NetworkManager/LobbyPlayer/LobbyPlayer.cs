

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayer : MonoBehaviour {
    [SerializeField]
    private TMP_Text namePlayer;

    [SerializeField]
    private Image imageCharactor;


    [SerializeField]
    private GameObject ready;

    private LobbyPlayerData _data;
    public void SetData(LobbyPlayerData data){
       
        _data = data;
        namePlayer.text = data.GameTag;
        if(imageCharactor !=null && gameObject != null){
            imageCharactor.sprite = CharactorSelectionUI.instance._charectorSelectionData.AllPlayer[data.IndexSelected].ImageCharactor;
        }
            
        
        if(_data.IsReady && gameObject!=null && ready!=null){
            ready.SetActive(true);
        }
        else if (!_data.IsReady && gameObject!=null && ready!=null){
            ready.SetActive(false);
        }

        // imageCharactor.GetComponent<Image>().

        gameObject.SetActive(true);
        
        
            
        
    }
       
}