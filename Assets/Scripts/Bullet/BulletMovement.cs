using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;


public class BulletMovement : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> Speed = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private Rigidbody2D _rigidbody;

    private Vector2 direction;
    

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
       
        
    }
    public override void OnNetworkSpawn()
    {
        
        direction = transform.up;
        _rigidbody.velocity = new Vector2(direction.x, direction.y)*(float)Speed.Value;

        base.OnNetworkSpawn();
        StartCoroutine(DestroyGameObject());
    }



    private IEnumerator DestroyGameObject()
    {
       
        yield return new WaitForSeconds(2f);
        
        if(IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
        
        
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(IsServer && !IsHost){
            if(other.CompareTag("Obstacle")){
                GetComponent<NetworkObject>().Despawn();
            }
        }
        
    }

    

   
   


}
