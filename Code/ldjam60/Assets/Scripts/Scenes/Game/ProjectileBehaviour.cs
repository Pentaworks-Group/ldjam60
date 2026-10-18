using System;

using Assets.Scripts.Core.Models;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        public event Action<ProjectileBehaviour> ProjectileStopped;

        private Rigidbody myRigidbody;

        public Projectile Projectile {get; private set;}

        public void Init(Projectile projectile)
        {
            this.Projectile = projectile;
        }

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if ((Base.Core.Game.IsRunning) && (Projectile.IsFired) && (!Projectile.IsStationary))
            {
                if (myRigidbody.linearVelocity.magnitude < 0.1f)
                {
                    Projectile.IsStationary = true;

                    ProjectileStopped?.Invoke(this);
                }
            }
        }
    }
}
