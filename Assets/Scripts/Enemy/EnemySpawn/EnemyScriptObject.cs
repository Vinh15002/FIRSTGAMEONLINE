using JetBrains.Annotations;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Enemy.EnemySpawn
{

    [CreateAssetMenu(menuName = "Data/Enemy", fileName = "Enemy")]
    public class EnemyScriptObject : ScriptableObject
    {
       

        public float speed;
        public int maxHealth;
        public int health;
        public int damage;
        public float scale;

        


      


    }
}
