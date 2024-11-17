

using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DestroyObjectTower: NetworkBehaviour{

    [SerializeField]
    private float timeToDestroy = 10f;



    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();
        
       
        StartCoroutine(DestroyObject());
    }


    public IEnumerator DestroyObject(){
        yield return new WaitForSeconds(timeToDestroy);
        if(IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
    }
}