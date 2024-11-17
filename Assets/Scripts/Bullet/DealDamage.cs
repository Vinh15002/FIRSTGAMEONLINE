
using Assets.Scripts.Events;

using Unity.Netcode;
using UnityEngine;

public class DealDamage : NetworkBehaviour
{
    //[SerializeField]
    //public NetworkVariable<int> damage = new NetworkVariable<int>(5, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private int damage = 0;


    private void OnEnable()
    {
        ChangeDamageEvent.changeDamage += OnChangeDame;
    }

    private void OnDisable()
    {
        ChangeDamageEvent.changeDamage -= OnChangeDame;
    }

    private void OnChangeDame(int dam)
    {
        damage = dam;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        
        if(other.CompareTag("Enemy")){
            if(NetworkManager.Singleton.IsServer)
                other.GetComponent<EnemyHealth>().TakeDame(damage);
                
            
           
            GetComponent<BulletMovement>().Reset();

           
            ObjectPooling.Singleton.SpawnBom(other.transform.position);
            ObjectPooling.Singleton.SpawnUIDamdge(other.transform.position, $"-{damage} HP", Color.red);
            
            
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
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"-{damage} HP", Color.red);

    }
}
