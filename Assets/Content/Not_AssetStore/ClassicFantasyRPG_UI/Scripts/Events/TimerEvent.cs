using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class TimerEvent : MonoBehaviour
    {
        public UnityEvent onTimerEvent;
        public float waitTimeInSeconds = 5f;

        private void Start()
        {
            StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(waitTimeInSeconds);
            onTimerEvent?.Invoke();
        }
    }
}