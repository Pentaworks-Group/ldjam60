using System;

using Assets.Scripts.Core.Models;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        public event Action<ProjectileBehaviour> MoveFinished;

        private Boolean isFired = false;
        private Rigidbody myRigidbody;
        private ObjectsBehaviour objectBehaviour;

        public Projectile Projectile { get; private set; }

        public void Init(Projectile projectile)
        {
            this.Projectile = projectile;

        }

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            this.objectBehaviour = GetComponent<ObjectsBehaviour>();

            this.objectBehaviour.ObjectDestroyed.AddListener(OnObjectDestroyed);
        }

        private void OnObjectDestroyed(GameObject destroyedObject)
        {
            this.objectBehaviour.ObjectDestroyed.RemoveListener(OnObjectDestroyed);
            MoveFinished?.Invoke(this);
        }

        private void Update()
        {
            if (Base.Core.Game.IsRunning)
            {
                if (!isFired)
                {
                    if (myRigidbody.linearVelocity.magnitude > 0.1)
                    {
                        isFired = true;
                    }
                }
                else
                {
                    if (myRigidbody.linearVelocity.magnitude < 0.1f)
                    {
                         MoveFinished?.Invoke(this);
                    }
                }
            }
        }
    }
}
