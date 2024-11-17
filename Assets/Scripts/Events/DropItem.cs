

using UnityEngine;

namespace Assets.Scripts.Events
{
    public class DropItem 
    {
        public delegate void DropItemRate(int rate, Vector3 position);
        public static DropItemRate dropItemRate;
    }
}
