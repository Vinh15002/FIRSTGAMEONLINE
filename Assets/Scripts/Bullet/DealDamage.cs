using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

public class DealDamage : NetworkBehaviour
{
    [SerializeField]
    public NetworkVariable<int> damage = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Server);



    private void OnTriggerEnter2D(Collider2D other) {

        
        if(other.CompareTag("Enemy")){
            if(NetworkManager.Singleton.IsServer){
                other.GetComponent<EnemyHealth>().TakeDame(damage.Value);
                
            }
           
            GetComponent<BulletMovement>().Reset();

           
            ObjectPooling.Singleton.SpawnBom(other.transform.position);
            ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"-{damage.Value}", Color.red);
            
            
            if(!IsHost && IsOwner){
                
                SendInforClientRpc(other.transform.position);
            }

            
            //SendInforServerRpc(other.transform.position);


            
            // if(!IsHost){
            //     SendInforServerRpc(other.transform.position);
            // }
                
           
           
        }
        
        
    }
    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnBom(position);
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"-{damage.Value}", Color.red);
    }
}
