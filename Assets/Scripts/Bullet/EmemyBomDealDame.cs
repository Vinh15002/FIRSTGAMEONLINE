using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EmemyBomDealDame : NetworkBehaviour
{
    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private NetworkObject UIDamege;


    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(NetworkManager.Singleton.IsServer){
                other.GetComponent<HealthController>().TakeDame(damage);
                
            }
           
            

            
            GetComponent<BulletMovement>().Reset();

           
            //ObjectPooling.Singleton.SpawnBom(other.transform.position);
            ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"-{damage}", Color.red);
            

            if(!IsHost && IsOwner){
                
                SendInforClientRpc(other.transform.position);
            }
            // GetComponent<BulletMovement>().Reset();
            
            
            
                

                // NetworkObject game =  Instantiate(UIDamege, other.transform.position, Quaternion.identity);
               
                // game.Spawn();
                // game.GetComponent<DamaePopup>().textValue.Value = $"-{damage}";

                //GetComponent<BulletMovement>().Reset();
                
        }
    }
    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"-{damage}", Color.red);
    }
}
