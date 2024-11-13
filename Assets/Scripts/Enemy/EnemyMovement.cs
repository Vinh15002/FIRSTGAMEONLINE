using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D _rigidbody2D;
    private Animator animator;

    public bool hasTarget = false;

    
    private Vector2 direction;
    
    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {

        
        if(hasTarget){
            _rigidbody2D.velocity= direction*speed;
            transform.localScale = new Vector2(direction.x < 0.1f ? -1:1, 1);
        }
        else{
            _rigidbody2D.velocity = Vector2.zero;
        }
        if(canMove){
            _rigidbody2D.velocity = Vector2.zero;
        }

        
    }

    public void findTarget(Vector3 position){
        hasTarget = true;
        Vector3 targetDirection = transform.position - position;
        direction = -targetDirection.normalized *speed;
        

    }


    public void onDie(){
        speed = 0;
    }


    public bool canMove{
        get{return animator.GetBool("canMove");}
    }

    

    
}
