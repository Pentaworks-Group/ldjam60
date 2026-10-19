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


        private Vector2 rotationCenter;
        private Button targetButton;

        void Awake()
        {
            targetButton = GetComponent<Button>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (targetButton != null && !targetButton.interactable)
            {
                return;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!targetButton.interactable)
            {
                return;
            }
            RotateDialTowardsPointer(eventData.position);
        }

        private void RotateDialTowardsPointer(Vector2 screenPosition)
        {

            Vector3 worldPos = transform.parent.position;
            Vector2 dialScreenPos = RectTransformUtility.WorldToScreenPoint(null, worldPos);


            Vector2 direction = screenPosition - dialScreenPos;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            targetAngle = Mathf.Clamp(targetAngle, 0f, 180f);

            float currentPercentage = Mathf.InverseLerp(180f, 0f, targetAngle);

            dialBehaviour.ChangeDialTo(currentPercentage);
        }
    }
}
