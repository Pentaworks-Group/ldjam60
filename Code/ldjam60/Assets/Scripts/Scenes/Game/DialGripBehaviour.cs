using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class DialGripBehaviour : MonoBehaviour, IDragHandler
    {

        [SerializeField]
        private float rotationSpeed = 0.005f;
        [SerializeField]
        private DialBehaviour dialBehaviour;


        private Button targetButton;

        void Awake()
        {
            targetButton = GetComponent<Button>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!targetButton.interactable)
            {
                return;
            }

            float xMovement = eventData.delta.x * rotationSpeed;
            float yMovement = eventData.delta.y * rotationSpeed;

            float totalMovement = xMovement + yMovement;

            dialBehaviour.ChangeDial(totalMovement);
        }
    }
}
