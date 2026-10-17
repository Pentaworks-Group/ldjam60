using UnityEngine;

namespace Assets.Scripts
{
	public class ObjectsDestructionBehaviour : MonoBehaviour
	{
        [SerializeField]
        private GameObject outsideCollider;
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == outsideCollider)
            {
                Debug.Log("Reached the outside");
                Destroy(gameObject);
            }
        }
    }
}
