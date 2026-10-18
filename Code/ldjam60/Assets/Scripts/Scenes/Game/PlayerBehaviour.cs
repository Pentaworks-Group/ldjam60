using System;

using Assets.Scripts.Core.Models;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.Game
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        public UnityEvent<PlayerBehaviour> ShotFired = new UnityEvent<PlayerBehaviour>();
        public event Action<PlayerBehaviour, Single> AngleChanged;

        public abstract Player Player { get; }

        public void Init(Player player)
        {
            InnerInit(player);

        }

        public virtual void EnableInput()
        {
        }

        public virtual void DisableInput()
        {
        }

        protected abstract void InnerInit(Player player);

        protected virtual void InnerAwake()
        {

        }

        protected virtual void InnerStart()
        {

        }

        protected virtual void InnerUpdate()
        {

        }

        protected void OnAngleChanged(Single angle)
        {
            this.AngleChanged?.Invoke(this, angle);
        }

        private void Awake()
        {
            InnerAwake();
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
