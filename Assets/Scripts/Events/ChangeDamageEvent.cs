using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    public class ChangeDamageEvent
    {
        public  delegate void ChangeDamage(int damage);

        public static ChangeDamage changeDamage;


        public delegate void ChangeDamgeBullet(int damage);

        public static ChangeDamgeBullet changeDamgeBullet;
    }
}
