using Assets.Scripts.Collector;

using Unity.Netcode;
using UnityEngine;

public class CollectorSpeed : Collector
{

    [SerializeField]
    private int speedup;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (NetworkManager.Singleton.IsServer)
        {


            other.GetComponent<PlayerController>().speed.Value += speedup;
            
        }

        ObjectPooling.Singleton.SpawnUIDamdge(transform.position, $"+{speedup} Speed", Color.blue);
        if (!IsHost && IsOwner)
        {
            SendInforClientRpc(transform.position);
        }
        SetDestroy();

    }

    [ClientRpc]
    private void SendInforClientRpc(Vector3 position)
    {
        ObjectPooling.Singleton.SpawnUIDamdge(position, $"+{speedup} speed", Color.blue);
        SetDestroy();
    }

}
