using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes.Game.Variables
{
	public class ContinousButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private float delay;
        [SerializeField]
        private ButtonClickedEvent onClick;

        private bool pressed;
        private Button targetButton;

        void Awake()
        {
            targetButton = GetComponent<Button>();
        }


        public void OnPointerDown(PointerEventData eventdata)
        {
            if (!targetButton.interactable)
            {
                return;
            }

            pressed = true;
            StartCoroutine(Repeat());
        }

        public void OnPointerUp(PointerEventData eventdata)
        {
            StopAllCoroutines();
            pressed = false;
        }

        private IEnumerator Repeat()
        {
            while (pressed)
            {
                onClick?.Invoke();
                yield return new WaitForSeconds(delay);
            }

        }
    }
}
