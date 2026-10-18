using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;

namespace Assets.Scripts.Scenes.Game.Variables
{
	public class ContinousButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private float delay;
        [SerializeField]
        private ButtonClickedEvent onClick;

        private bool pressed;

        public void OnPointerDown(PointerEventData eventdata)
        {
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
