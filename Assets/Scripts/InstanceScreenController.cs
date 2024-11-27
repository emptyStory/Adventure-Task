using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InstanceScreenController : MonoBehaviour
{
    //����������, �������������� ������� � ����� �� ������� � ����������
    public GameObject characterProgressRoot;
    public PlayableDirector characterProgressEnter;
    public PlayableDirector characterProgressExit;

    public void CharacterProgressEnterButtonPressed() //������� �������� � ������ � ����������
    {
        characterProgressRoot.SetActive(true);
        characterProgressEnter.Play();
    }

    public void CharacterProgressExitButtonPressed() //������� ������ �� ������� � ����������
    {
        characterProgressExit.Play();
        characterProgressRoot.SetActive(false);
    }
}
