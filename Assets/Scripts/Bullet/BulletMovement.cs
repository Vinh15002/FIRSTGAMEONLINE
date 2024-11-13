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
       
        //StartCoroutine(DestroyGameObject());
    }

    public void SetActive(Vector3 position, Quaternion quaternion, Vector2 directory){
        gameObject.SetActive(true);
        transform.position=position;
        transform.rotation = quaternion;
        _rigidbody.velocity = directory*Speed.Value;
        StartCoroutine(DestroyGameObject());
    }



    public IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(2f);
        Reset();
       

    }


    public void Reset(){
        transform.position = Vector3.zero;
        _rigidbody.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Obstacle")){
            ObjectPooling.Singleton.SpawnBom(transform.position);
            Reset();

            
        }
        
    }

    

   
   


}
