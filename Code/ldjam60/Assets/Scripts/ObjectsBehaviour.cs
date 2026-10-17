using UnityEngine;

namespace Assets.Scripts
{
	public class ObjectsBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject outsideCollider;

        private float health;
        private bool indestructable;
        public float ImpactDamage { get; private set; }


        private float speedMultiplier = 0.001f;


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == outsideCollider)
            {
                Debug.Log("Reached the outside");
                Destroy(gameObject);
                return;
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<ObjectsBehaviour>(out ObjectsBehaviour component))
            {
                TakeImpactDamage(collision.relativeVelocity.sqrMagnitude, component.ImpactDamage);
            }
            else
            {
                TakeImpactDamage(collision.relativeVelocity.sqrMagnitude, speedMultiplier);
            }

        }

        private void TakeImpactDamage(float relativeSpeedSQR, float impactDamageMultiplier)
        {
            if (!indestructable)
            {
                var speedDmg = relativeSpeedSQR * impactDamageMultiplier;
                if (speedDmg > health * 0.1f)
                {
                    health -= speedDmg;
                    if (health <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void TakeDamage(float dmg)
        {
            if (!indestructable)
            {
                health -= dmg;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Init(float health, bool indestructable, float impactDamage)
        {
            this.health = health;
            this.indestructable = indestructable; 
            this.ImpactDamage = impactDamage;
        }

    }
}
