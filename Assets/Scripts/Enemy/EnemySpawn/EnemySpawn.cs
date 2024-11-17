

using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Enemy.EnemySpawn
{
    public class EnemySpawn : NetworkBehaviour
    {
        [SerializeField]
        private float timeToSpawn;

        private float _timeToSpawn = 10f;



        [SerializeField] private List<NetworkObject> ListPrefab;


  
        public override void OnNetworkSpawn()
        {
           
            base.OnNetworkSpawn();
            
            
        }

        private void FixedUpdate()
        {
            if(IsServer)
            {
                _timeToSpawn -= Time.deltaTime;
                Spawn();
            }
            
        }

        private void Spawn()
        {
            if (_timeToSpawn <= 0)
            {
                int type = Random.Range(0, ListPrefab.Count - 1);
                NetworkObject obj = Instantiate(ListPrefab[type], transform.position, Quaternion.identity);
                obj.GetComponent<NetworkObject>().Spawn();
                _timeToSpawn = timeToSpawn;
                
            }
            //if(_timeToSpawn <= 0)
            //{
            //   
            //    SendTheClientRpc(type);
            //    _timeToSpawn = timeToSpawn;


            //}
        }

        
    }
}
