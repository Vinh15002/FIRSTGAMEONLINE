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

    [SerializeField]
    private NetworkObject prefabBom;


    [SerializeField]
    private NetworkObject UIDamege;


    private void OnTriggerEnter2D(Collider2D other) {

        
        if(other.CompareTag("Enemy")){
            if(IsServer){
                other.GetComponent<EnemyHealth>().TakeDame(damage.Value);
                
            }
            SendInforServerRpc(other.transform.position);
            
           
        }
        
        
    }
    [ServerRpc(RequireOwnership =false)]
    private void SendInforServerRpc(Vector3 position)
    { 
       
        SpawnBom(position);
        SpawnUIDamdge(position);
        GetComponent<NetworkObject>().Despawn();
    }

    public void SpawnBom(Vector3 positon){
        NetworkObject game =  Instantiate(prefabBom, positon, Quaternion.identity);
        game.Spawn();
        
    }

    public void SpawnUIDamdge(Vector3 position){
        NetworkObject game =  Instantiate(UIDamege, position, Quaternion.identity);
        
        game.Spawn();
        game.GetComponent<DamaePopup>().textValue.Value = $"-{damage.Value}";
    }






}
