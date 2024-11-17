using Assets.Scripts.Enemy.EnemySpawn;
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



    [SerializeField]
    private ObjectSpawner Item01;


    [SerializeField]
    private ObjectSpawner Item02;


    [SerializeField]
    private ObjectSpawner Item03;

    [SerializeField]
    private ObjectSpawner Item04;





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
        Item01.Instantiate();
        Item02.Instantiate();
        Item03.Instantiate();
        Item04.Instantiate();

       
    }

    public void SpawnBullet(Vector3 position, Quaternion quaternion, Vector2 direction, int typeBullet, int Damage){
        if(typeBullet==0){
            bullet01.spawnObject(position,quaternion,direction, Damage);
        }
        else if(typeBullet == 2){
            bulletUtil.spawnObject(position,quaternion,direction, Damage);
        }
        else if(typeBullet == 1){
            bullet02.spawnObject(position,quaternion,direction, Damage);
        }
        else if(typeBullet == 3){
            bullet03.spawnObject(position,quaternion,direction, Damage);
        }
        
    }


    public void SpawnBomEnemy01(Vector3 position, Quaternion quaternion, Vector2 direction, int Damage){
        BomEnemy01.spawnObject(position,quaternion,direction, Damage);
    }

    public void SpawnBom(Vector3 position){
        Bom.spawnObjectBom(position);
    }

    public void SpawnUIDamdge(Vector3 position, string text, Color color){
        UIDamagePopup.spawnObjectUI(position,text,color);
    }

    public void SpawnItem(Vector3 positon, int typeItem)
    {
        if(typeItem == 1)
        {
            Item01.spawnItem(positon);
        }
        else if(typeItem == 2)
        {
            Item02.spawnItem(positon);
        }
        else if(typeItem == 3)
        {
            Item03.spawnItem(positon);
        }
        else
        {
            Item04.spawnItem(positon);
        }
    }


    
}
