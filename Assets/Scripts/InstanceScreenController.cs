using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InstanceScreenController : MonoBehaviour
{
    //переменные, контролирующие переход и выход из раздела с персонажем
    public GameObject characterProgressRoot;
    public PlayableDirector characterProgressEnter;
    public PlayableDirector characterProgressExit;

    public void CharacterProgressEnterButtonPressed() //функция перехода в раздел с персонажем
    {
        characterProgressRoot.SetActive(true);
        characterProgressEnter.Play();
    }

    public void CharacterProgressExitButtonPressed() //функция выхода из раздела с персонажем
    {
        characterProgressExit.Play();
        characterProgressRoot.SetActive(false);
    }
}
