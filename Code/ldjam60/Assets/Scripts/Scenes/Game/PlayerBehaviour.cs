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
        public event Action<PlayerBehaviour, Single> PowerChanged;
        
        private Boolean isActive;
        public Boolean IsActive => isActive;

        public abstract Player Player { get; }

        public void Init(Player player)
        {
            InnerInit(player);
        }
                
        public void Activate()
        {
            OnActivate();
            this.isActive = true;
        }
                
        public void Deactivate()
        {
            OnDeactivate();
            this.isActive = false;
        }

        protected abstract void InnerInit(Player player);

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected virtual void InnerAwake()
        {
        }

        protected virtual void InnerStart()
        {
        }

        protected virtual void InnerOnDestroy()
        {
        }

        protected virtual void InnerUpdate()
        {
        }

        protected void OnAngleChanged(Single angle)
        {
            this.AngleChanged?.Invoke(this, angle);
        }

        protected void OnPowerChanged(Single power)
        {
            this.PowerChanged?.Invoke(this, power);
        }

        private void Awake()
        {
            InnerAwake();
        }

        private void Start()
        {
            InnerStart();
        }

        private void OnDestroy()
        {
            InnerOnDestroy();
        }

        private void Update()
        {
            InnerUpdate();
        }
    }
}
