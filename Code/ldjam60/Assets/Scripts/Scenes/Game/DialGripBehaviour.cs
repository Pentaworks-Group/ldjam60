using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game
{
    public class DialGripBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private DialBehaviour dialBehaviour;

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

            GameFrame.Base.Audio.Effects.Play("Gears");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!targetButton.interactable)
            {
                return;
            }
            RotateDialTowardsPointer(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameFrame.Base.Audio.Effects.Stop();
        }

        private void RotateDialTowardsPointer(Vector2 screenPosition)
        {

            Vector3 worldPos = transform.parent.position;
            Vector2 dialScreenPos = RectTransformUtility.WorldToScreenPoint(null, worldPos);


            Vector2 direction = screenPosition - dialScreenPos;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (targetAngle < 0f)
            {
                targetAngle += 360f;
            }

            //targetAngle = Mathf.Clamp(targetAngle, 0f, 180f);

            if (targetAngle > 180f)
            {
                targetAngle = targetAngle < 270f ? 180f : 0f;
            }

            float currentPercentage = Mathf.InverseLerp(180f, 0f, targetAngle);

            dialBehaviour.ChangeDialTo(currentPercentage);
        }


    }
}
