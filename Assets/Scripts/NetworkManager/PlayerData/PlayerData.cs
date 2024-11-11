
using UnityEngine;


[CreateAssetMenu(menuName = "Data/PlayerData", fileName ="PlayerData")]
public class PLayerData : ScriptableObject {
    public Sprite ImageCharactor;
    public string damage;
    public string health;

    public string speed;

    public string rateOfFire;

    public uint hasCodeCharactor;
}