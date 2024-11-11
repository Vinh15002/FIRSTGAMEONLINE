using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FindEnemy : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject bulletChase;



    [SerializeField]
    private float timeToDealDame = 1f;
    private float _timeToDealDame = 0f;


    private void FixedUpdate() {
        if(IsServer){
            _timeToDealDame += Time.deltaTime;
        }
    }


    private void OnTriggerStay2D(Collider2D other) {
        if(!other.CompareTag("Enemy")) return;
        if(NetworkManager.Singleton.IsServer){
            TowerEvent.getPosition?.Invoke(other.transform.position);

            if(_timeToDealDame >= timeToDealDame){
                NetworkObject game = Instantiate(bulletChase, transform.position, transform.rotation);
                game.Spawn();
                _timeToDealDame = 0;
            }
        }
    }
}
