using System;

using Assets.Scripts.Core.Models;
using Assets.Scripts.Prefabs;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class CatapultBehaviour : MonoBehaviour
    {
        [SerializeField] 
        TMP_Text nameText;
        [SerializeField] 
        GameObject spriteContainer;
        [SerializeField] 
        HealthBarBehaviour healthBarBehaviour;

        private Player player;

        public void Init(Player player)
        {
            this.player = player;

            this.nameText.text = player.Name;
            this.nameText.color = player.Color;

            this.healthBarBehaviour.Init(0, player.Health, player.Health);


            var rb = GetComponent<Rigidbody>();

            Vector3 currentCoM = rb.centerOfMass;
            float localHeight = GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size.y;
            float shiftAmount = localHeight * 0.10f;
            rb.centerOfMass = new Vector3(currentCoM.x, currentCoM.y - shiftAmount, currentCoM.z);
        }

        private void Update()
        {
            if (Base.Core.Game.IsRunning)
            {
                if (player.Catapult.FiringAngle > 0)
                {
                    var currentScale = spriteContainer.transform.localScale;
                    
                    if (currentScale.x < 0)
                    {
                        this.spriteContainer.transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);    
                    }
                }
                else if (player.Catapult.FiringAngle < 0)
                {
                    var currentScale = spriteContainer.transform.localScale;

                    if (currentScale.x > 0)
                    {
                        this.spriteContainer.transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
                    }
                }

                this.healthBarBehaviour.SetValue(player.Health);
            }
        }
    }
}
