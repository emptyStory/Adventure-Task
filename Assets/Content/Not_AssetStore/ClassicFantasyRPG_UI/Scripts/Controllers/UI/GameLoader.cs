using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class GameLoader : MonoBehaviour
    {
        public Slider loadingSlider;
        public int gameSceneIndex = 1;
        public float delaySliderValue = .3f;
        public float initDelay = 5f;

        private float _timeWhenToLoad = float.MaxValue;
        private float _startTime;
        private bool _loadingStarted = false;

        private void Start()
        {
            enabled = loadingSlider != null;
            if (!enabled) return;
            loadingSlider.minValue = 0;
            loadingSlider.maxValue = 1 + delaySliderValue;
        }

        private void Update()
        {
            if (_loadingStarted)
            {
                return;
            }

            var time = Time.time;
            if (time > _timeWhenToLoad)
            {
                _loadingStarted = true;
                StartCoroutine(LoadScene(gameSceneIndex));
            }
            else if (initDelay > 0 && delaySliderValue > 0)
            {
                loadingSlider.value = (time - _startTime) / initDelay * delaySliderValue;
            }
        }

        private IEnumerator LoadScene(int index)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(index);
            while (!loadSceneAsync.isDone)
            {
                loadingSlider.value = loadSceneAsync.progress + delaySliderValue;
                yield return null;
            }
        }

        private void OnEnable()
        {
            _startTime = Time.time;
            _timeWhenToLoad = _startTime + initDelay;
        }
    }
}