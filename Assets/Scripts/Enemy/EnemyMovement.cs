using Assets.Scripts.Enemy;
using System.Collections;
using Unity.Netcode;

using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    
    private Rigidbody2D _rigidbody2D;
    private Animator animator;

    public bool hasTarget = false;
    private float scale;
    private float speed;

    [SerializeField] private LayerMask layerMask;

    
    private Vector2 direction;

    private float timeToMove = 3f;
    
    private void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        scale = GetComponent<Enemy>().Scale;
        speed = GetComponent<Enemy>().Speed;

    }

    
    private void FixedUpdate() {
        if (!hasTarget)
        {
            RandomMove();
            HandleMoveEnemy();
            timeToMove -= Time.deltaTime;
            _rigidbody2D.velocity = direction * speed;

        }
        else
        {
            
           
           
            _rigidbody2D.velocity = direction * speed;
        }
        transform.localScale = new Vector3(direction.x < 0.1f ? -scale : scale, scale, 1);


        if (canMove)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
        






    }


    public void HandleMoveEnemy()
    {
        RaycastHit2D ray = Physics2D.CircleCast(transform.position, 2f, transform.up, 2f, layerMask);
        if (ray)
        {
            direction = ray.normal;
            Debug.Log("WTF");
        }
       
    }


    public void findTarget(Vector3 position){
        hasTarget = true;
        Vector3 targetDirection = transform.position - position;
        direction = -targetDirection.normalized;
        

    }

    public void RandomMove()
    {
        if(timeToMove < 0)
        {
            float x = Random.RandomRange(-1f, 1f);
            float y = Random.RandomRange(-1f, 1f);
            direction = new Vector2(x, y);
            timeToMove = 3f;

        }


    }


    public void onDie(){
        speed = 0;
    }


    public bool canMove{
        get{return animator.GetBool("canMove");}
    }

    

    
}
