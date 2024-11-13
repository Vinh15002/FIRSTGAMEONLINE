using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectPooling : NetworkBehaviour
{
    public static ObjectPooling Singleton {get; private set;}


    [SerializeField]
    private ObjectSpawner bullet01;

    [SerializeField]
    private ObjectSpawner bullet02;

    [SerializeField]
    private ObjectSpawner bullet03;


    [SerializeField]
    private ObjectSpawner bulletUtil;

    [SerializeField]
    private ObjectSpawner Bom;


    [SerializeField]
    private ObjectSpawner BomEnemy01;

    [SerializeField]
    private ObjectSpawner UIDamagePopup;



    

    private void Awake() {
        Singleton = this;
       
        SpawnPooling();
        
            
    }

   
    private void SpawnPooling()
    {
        bullet01.Instantiate();
        Bom.Instantiate();
        BomEnemy01.Instantiate();
        UIDamagePopup.Instantiate();
        bulletUtil.Instantiate();
        bullet02.Instantiate();
        bullet03.Instantiate();
    }

    public void SpawnBullet(Vector3 position, Quaternion quaternion, Vector2 direction, int typeBullet){
        if(typeBullet==0){
            bullet01.spawnObject(position,quaternion,direction);
        }
        else if(typeBullet == 2){
            bulletUtil.spawnObject(position,quaternion,direction);
        }
        else if(typeBullet == 1){
            bullet02.spawnObject(position,quaternion,direction);
        }
        else if(typeBullet == 3){
            bullet03.spawnObject(position,quaternion,direction);
        }
        
    }


    public void SpawnBomEnemy01(Vector3 position, Quaternion quaternion, Vector2 direction){
        BomEnemy01.spawnObject(position,quaternion,direction);
    }

    public void SpawnBom(Vector3 position){
        Bom.spawnObjectBom(position);
    }

    public void SpawnUIDamdge(Vector3 position, string text, Color color){
        UIDamagePopup.spawnObjectUI(position,text,color);
    }
}
