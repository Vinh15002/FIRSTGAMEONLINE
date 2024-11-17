using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnItem : NetworkBehaviour
{

    public void OnEnable()
    {
        DropItem.dropItemRate += OnDropItem;
    }

    public void OnDisable()
    {
        DropItem.dropItemRate -= OnDropItem;
    }


    private void OnDropItem(int rate, Vector3 position)
    {
        int rand = Random.Range(0, 100);
        if(rand <= rate)
        {
            Spawn(position);
        }
    }

    private void Spawn(Vector3 position)
    {
        int rand = Random.Range(0, 100);
        SendTheClientRpc(rand, position);
        
       
    }
    

    [ClientRpc]
    private void SendTheClientRpc(int rand, Vector3 position)
    {
        if (rand >= 40)
        {
            ObjectPooling.Singleton.SpawnItem(position, 1);
        }
        else if (rand >= 15)
        {
            ObjectPooling.Singleton.SpawnItem(position, 2);
        }
        else if (rand >= 10)
        {
            ObjectPooling.Singleton.SpawnItem(position, 3);
        }
        else
        {
            ObjectPooling.Singleton.SpawnItem(position, 4);
        }
    }
}
