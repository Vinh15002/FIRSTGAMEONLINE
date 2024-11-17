using Assets.Scripts.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Events.ChangeDamageEvent;


public class BulletMovement : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<float> Speed = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

   
    private int dam;

    public int Dam
    {
        get => dam;
        private set => dam = value;
    }


    private Rigidbody2D _rigidbody;

    public Vector2 direction;

    private int damge;
    

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();

       
       
        
        // _rigidbody.velocity = new Vector2(direction.x, direction.y)*(float)Speed.Value;
       
        //StartCoroutine(DestroyGameObject());
    }
    public void SetActive(Vector3 position, Quaternion quaternion, Vector2 directory, int Damage){
        gameObject.SetActive(true);
        transform.position=position;
        transform.rotation = quaternion;
        _rigidbody.velocity = directory*Speed.Value;

        Dam = Damage;
        ChangeDamageEvent.changeDamage?.Invoke(Dam);






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
