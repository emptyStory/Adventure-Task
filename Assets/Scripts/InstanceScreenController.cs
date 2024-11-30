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

    //����������, �������������� ������� � ����� �� ������� � ���������
    public GameObject storeRoot;

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

    public void StoreEnterButtonPressed() //������� �������� � ������ � ����������
    {
        storeRoot.SetActive(true);
    }

    public void StoreExitButtonPressed() //������� �������� � ������ � ����������
    {
        storeRoot.SetActive(false);
    }
}
