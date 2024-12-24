using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class InstanceScreenController : MonoBehaviour
{
    //����������, �������������� ������� � ����� �� ������� � ����������
    public GameObject mainScreen;
    public GameObject characterProgressRoot;
    public GameObject characterPanelBackgroundImage;
    public PlayableDirector characterProgressEnter;
    public PlayableDirector characterProgressExit;

    //����������, �������������� ������� � ����� �� ������� � ���������
    public GameObject storeRoot;

    public void CharacterProgressEnterButtonPressed() //������� �������� � ������ � ����������
    {
        mainScreen.SetActive(false);
        characterProgressRoot.SetActive(true);
        characterPanelBackgroundImage.SetActive(false);
        characterProgressEnter.Play();
    }

    public void CharacterProgressExitButtonPressed() //������� ������ �� ������� � ����������
    {
        characterProgressExit.Play();
        mainScreen.SetActive(true);
        characterProgressRoot.SetActive(false);
        characterPanelBackgroundImage.SetActive(true);
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
