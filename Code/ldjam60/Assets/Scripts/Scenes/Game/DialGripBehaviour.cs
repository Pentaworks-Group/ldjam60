using System.Collections;
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

            dialBehaviour.ChangeDial(-totalMovement);
        }


        //private bool pressed;
        //private Vector2 downPossition;

        //public void OnPointerDown(PointerEventData eventdata)
        //{
        //    downPossition = eventdata.position;
        //    pressed = true;
        //    StartCoroutine(Repeat());
        //}

        //public void OnPointerUp(PointerEventData eventdata)
        //{
        //    StopAllCoroutines();
        //    pressed = false;
        //}


        //private IEnumerator Repeat()
        //{
        //    while (pressed)
        //    {

        //    }
        //}
    }
}
