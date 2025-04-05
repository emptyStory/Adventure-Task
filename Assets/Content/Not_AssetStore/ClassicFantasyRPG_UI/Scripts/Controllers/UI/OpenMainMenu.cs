using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.UI
{
    public class OpenMainMenu : MonoBehaviour
    {
        public ApplicationContext.Slide slide;
        public int sceneIndex;
        
        public void OpenSceneWithSlide()
        {
            ApplicationContext.InitSlide = slide;
            SceneManager.LoadScene(sceneIndex);
        }
    }
}