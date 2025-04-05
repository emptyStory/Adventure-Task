using UnityEngine;

namespace Controllers.UI
{
    public class GamePauser : MonoBehaviour
    {
        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }
    }
}