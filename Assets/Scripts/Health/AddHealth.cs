using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AddHealth : NetworkBehaviour
{
    [SerializeField]
    private int heal = 2;
    
    [SerializeField]
    private float timeToDestroy = 5f;

    private float timeHeal = 0.5f;
    private float _timeHeal = 0.5f;

    [SerializeField]

    private NetworkObject UIHeal;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartCoroutine(DestroyGameObject());

    }

    private IEnumerator DestroyGameObject()
    {
       
        yield return new WaitForSeconds(timeToDestroy);
        
        if(NetworkManager.Singleton.IsServer){
            GetComponent<NetworkObject>().Despawn();
        }
        
        
        
    }
    private void FixedUpdate() {
        if(NetworkManager.Singleton.IsServer)
            _timeHeal += Time.deltaTime;
        
    }


    private void OnTriggerStay2D(Collider2D other) {

        if(other.CompareTag("Player")){
            if(_timeHeal >= timeHeal){
                if(NetworkManager.Singleton.IsServer){
                    other.GetComponent<HealthController>().GetHeal(heal);
                   
                    ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"+{heal} HP", Color.green);
                    
                }
                ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"+{heal} HP", Color.green);
                if(IsOwner){
                    SendInforClientRpc(other.transform.position);
                }
                if(IsHost){
                    ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"+{heal} HP", Color.green);
                }
                 _timeHeal = 0;
               
           }
        }
           
    }
        
            
    
    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"+{heal}", Color.green);
    }
}
