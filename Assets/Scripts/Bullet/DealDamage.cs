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
        
        if(!other.CompareTag("Enemy")) return;
        
        
        if(NetworkManager.Singleton.IsServer){
            other.GetComponent<EnemyHealth>().TakeDame(damage.Value);
            Vector3 position = gameObject.transform.position;
            SpawnBom(position);
            SpawnUIDamdge(position);
            GetComponent<NetworkObject>().Despawn();
        }
        
        
    }

   
    public void SpawnBom(Vector3 positon){
        NetworkObject game =  Instantiate(prefabBom, positon, Quaternion.identity);
        game.Spawn();
        
    }

    public void SpawnUIDamdge(Vector3 position){
        NetworkObject game =  Instantiate(UIDamege, position, Quaternion.identity);
        game.GetComponent<DamaePopup>().textValue.Value = $"-{damage.Value}";
        game.Spawn();
    }






}
