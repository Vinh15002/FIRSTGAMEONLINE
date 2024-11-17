
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Collector
{
    public  class Collector : NetworkBehaviour
    {
        public void SetActive(Vector3 position)
        {
            gameObject.SetActive(true);
            transform.position = position;
        }

        public void SetDestroy()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            
        }
    }
}
