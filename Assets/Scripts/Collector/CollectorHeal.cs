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
            NetworkObject game =  Instantiate(UIHeal, other.transform.position, Quaternion.identity);
            game.Spawn();
            game.GetComponent<DamaePopup>().textValue.Value = $"+{amountHeal}";
            game.GetComponent<DamaePopup>().textColor.Value = "Green";

            other.GetComponent<HealthController>().GetHeal(amountHeal);
        }
        

    }


    // [ServerRpc]
    // private void PickUpCollectorServerRpc(ulong id){
    //     NetworkObject player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[]
    // }


}
