

using System.Collections.Generic;

using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/ObjectSpawner", fileName ="ObjectSpawner")]
public class ObjectSpawner : ScriptableObject{
    public NetworkObject prefab;

    public int amount;

    public int currentIndex;


    public List<NetworkObject> listObject;


    public void Instantiate(){
        listObject.Clear();
        for(int i = 0 ;  i < amount ; i++){
            NetworkObject game = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            game.gameObject.SetActive(false);
            listObject.Add(game);
        }
    }

    public void spawnObject(Vector3 position, Quaternion quaternion, Vector2 direction){
        listObject[currentIndex++].GetComponent<BulletMovement>().SetActive(position, quaternion, direction);
        if(currentIndex == amount){
            currentIndex = 0;
        }
    }

    public void spawnObjectBom(Vector3 position){
        listObject[currentIndex++].GetComponent<Bom>().SetActive(position);
        if(currentIndex == amount){
            currentIndex = 0;
        }
    }

    public void spawnObjectUI(Vector3 position, string text, Color color){
        listObject[currentIndex++].GetComponent<DamaePopup>().SetActive(position, text, color);
        if(currentIndex == amount){
            currentIndex = 0;
        }
    }

}