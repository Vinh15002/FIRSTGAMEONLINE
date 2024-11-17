using Assets.Scripts.Enemy.EnemySpawn;

using Unity.Netcode;
using UnityEngine;


namespace Assets.Scripts.Enemy
{
    public class Enemy : NetworkBehaviour
    {


        [SerializeField]
        private EnemyScriptObject infor;

       
        private int _currentHealth;


       
        private int _maxHealth;

     
        private int _damage;

        
        private float _speed;

       
        private float _scale;




        private void Awake()
        {
            _currentHealth = infor.health;
            _maxHealth = infor.maxHealth;
            _damage = infor.damage;
            _speed = infor.speed;
            _scale = infor.scale;
            transform.localScale = new Vector3(Scale, Scale, Scale);
        }

      






        public int CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        
        public int MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;

        }


        public int Damage
        {
            get => _damage;
            set => _damage = value;

        }


       
        public float Speed
        {
            get => _speed; 
            set => _speed = value;
        }


       
        public float Scale
        {
            get => _scale;

            set => _scale = value;
        }


        public void SetActive(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.localScale = new Vector3(Scale, Scale, Scale);
        }

        public void SetDestroy()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }

    }
}
