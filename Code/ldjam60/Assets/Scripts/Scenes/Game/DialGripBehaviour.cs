using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scenes.Game
{
    public class DialGripBehaviour : MonoBehaviour, IDragHandler
    {

        [SerializeField] 
        private float rotationSpeed = 0.005f;
        [SerializeField]
        private DialBehaviour dialBehaviour;

        public void OnDrag(PointerEventData eventData)
        {
            float xMovement = eventData.delta.x * rotationSpeed;
            float yMovement = eventData.delta.y * rotationSpeed;

            float totalMovement = xMovement + yMovement;

            dialBehaviour.ChangeDial(totalMovement);
        }
    }
}
