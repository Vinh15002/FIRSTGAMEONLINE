using Assets.Scripts.Collector;
using Unity.Netcode;
using UnityEngine;

public class CollectorDamage : Collector
{
    [SerializeField]
    private int adddamage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (NetworkManager.Singleton.IsServer)
        {


            other.GetComponent<Fire>().DamageFire.Value += adddamage;
           
        }
        

        ObjectPooling.Singleton.SpawnUIDamdge(transform.position, $"+{adddamage} Damage", Color.cyan);
        if (!IsHost && IsOwner)
        {
            SendInforClientRpc(transform.position);
        }
        SetDestroy();

    }

    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"+{adddamage} Damage", Color.cyan);
        SetDestroy();
    }

}
