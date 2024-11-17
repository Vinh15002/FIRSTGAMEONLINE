using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    public class ChangeHealthBarEnemy
    {
        public delegate void ChangeHealth(float presenthealth);
        public static ChangeHealth changeHealth;
    }
}
