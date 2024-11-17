
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float _timeAttack = 2f;

    public int typeAttack = 0;

    public UnityEvent<Collider2D> onDealDamage;
    public UnityEvent onDealDamageExit;
    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.CompareTag("Player") && _timeAttack < 0){
            onDealDamage?.Invoke(other);
            _timeAttack = 2f;
            
        }
    }

    // private void OnTriggerExit2D(Collider2D other) {
    //     if(other.CompareTag("Player")){
    //         onDealDamageExit?.Invoke();
    //     }
    // }



    private void FixedUpdate() {
        if(typeAttack == 1){
            RaycastHit2D ray = Physics2D.CircleCast(transform.position, 7f, transform.up, 7f, layerMask);
            if(ray && ray.collider.CompareTag("Player") ){
                if(_timeAttack <0f){
                    onDealDamage?.Invoke(ray.collider);
                    _timeAttack = 2f;
                }
                //GetComponent<Animator>().SetBool("CanMove", true);
               
            }

        }
        
        _timeAttack -= Time.deltaTime;
    }
}
