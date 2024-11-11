using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bom : NetworkBehaviour
{
    private float _timeToDestroy = 3f;
    [SerializeField]
    private float speed = 10f;


    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();

        GetComponent<Rigidbody2D>().velocity = transform.up*speed;
        
        
        StartCoroutine(DestroyGameObject());

    }

    public IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(_timeToDestroy);

        if(IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
    }



}
