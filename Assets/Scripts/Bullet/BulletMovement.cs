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

    public Vector2 direction;
    

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public override void OnNetworkSpawn()
    {
        //direction = transform.up;
        base.OnNetworkSpawn();
        
        // _rigidbody.velocity = new Vector2(direction.x, direction.y)*(float)Speed.Value;
       
        StartCoroutine(DestroyGameObject());

        Debug.Log("DirectionBullet: " + direction);
    }

    private void FixedUpdate() {
        if(IsServer){
            
            transform.Translate(Vector2.up*Speed.Value*Time.deltaTime);
        }
        
    }



    private IEnumerator DestroyGameObject()
    {
       
        yield return new WaitForSeconds(2f);
        
        if(IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
        
        
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(IsServer){
            if(other.CompareTag("Obstacle")){
                GetComponent<NetworkObject>().Despawn();
            }
        }
        
    }

    

   
   


}
