
using Unity.Netcode;
using UnityEngine;

public class RadaFindEnemy : NetworkBehaviour
{



    private float timeToCall = 0.5f;

    private float _timeToCall = 0f;

    private void FixedUpdate() {
        if(IsServer){
            _timeToCall += Time.deltaTime;
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(!other.CompareTag("Enemy")) return;
        if(NetworkManager.Singleton.IsServer && _timeToCall >= timeToCall){
            GetComponentInParent<BulletChaseMovement>().ChangeDirection(other.transform.position);
            _timeToCall = 0;
        }
    }





}
