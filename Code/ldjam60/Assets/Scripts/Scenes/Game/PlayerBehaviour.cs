using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class PlayerBehaviour : MonoBehaviour
    {
        protected virtual void InnerStart()
        {

        }

        protected virtual void InnerUpdate()
        {

        }

        private void Start()
        {
            InnerStart();
        }

        private void Update()
        {
            InnerUpdate();
        }   
    }
}
