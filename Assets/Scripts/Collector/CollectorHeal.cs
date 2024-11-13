using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CollectorHeal : NetworkBehaviour
{

    [SerializeField]
    private int amountHeal;

    [SerializeField]

    private NetworkObject UIHeal;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(NetworkManager.Singleton.IsServer){
            

            other.GetComponent<HealthController>().GetHeal(amountHeal);
        }

        ObjectPooling.Singleton.SpawnUIDamdge(transform.position, $"+{amountHeal}", Color.green);
        if(!IsHost && IsOwner){
            SendInforClientRpc(transform.position);
        }

    }

    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"+{amountHeal}", Color.green);
    }


    // [ServerRpc]
    // private void PickUpCollectorServerRpc(ulong id){
    //     NetworkObject player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[]
    // }


}
