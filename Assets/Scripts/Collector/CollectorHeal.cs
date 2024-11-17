using Assets.Scripts.Collector;

using Unity.Netcode;
using UnityEngine;

public class CollectorHeal : Collector
{

    [SerializeField]
    private int amountHeal;


    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player")) return;
        if(NetworkManager.Singleton.IsServer){
            

            other.GetComponent<HealthController>().GetHeal(amountHeal);
           
        }

        ObjectPooling.Singleton.SpawnUIDamdge(transform.position, $"+{amountHeal} HP", Color.green);
        if(!IsHost && IsOwner){
            SendInforClientRpc(transform.position);
        }
        SetDestroy();

    }

    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"+{amountHeal} HP", Color.green);
        SetDestroy();
    }


    // [ServerRpc]
    // private void PickUpCollectorServerRpc(ulong id){
    //     NetworkObject player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[]
    // }


}
