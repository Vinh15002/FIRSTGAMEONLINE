

namespace Assets.Scripts.Events
{
    public class ChangeHealthBarEnemy
    {
        public delegate void ChangeHealth(float presenthealth);
        public static ChangeHealth changeHealth;
    }
}
