using UnityEngine;

namespace Assets.Scripts.Scenes.Credits
{
    public class CreditsBehaviour : MonoBehaviour
    {
        public void OnBack()
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
        }
    }
}
