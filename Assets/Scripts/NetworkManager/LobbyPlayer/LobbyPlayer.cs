

using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour {
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private SpriteRenderer sprite;

    private LobbyPlayerData _data;
    public void SetData(LobbyPlayerData data){
        _data = data;
        text.text = data.GameTag;

        if(_data.IsReady && sprite != null && gameObject!=null){
            sprite.color = Color.green;
        }
        gameObject.SetActive(true);
        
    }
}