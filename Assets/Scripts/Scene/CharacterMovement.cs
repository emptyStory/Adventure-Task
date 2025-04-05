using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using static ActionItem;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour
{
    public Transform[] points; // ������ �����, � ������� ����� ��������� ��������
    public ActionItem actionsData;
    public AudioSource audioPlayer;

    // ���� ��� ������� � ���������� ��������
    public Canvas canvas; // ������ �� ��� ������
    public TMP_Text outputText; // ������ �� ��������� �������, ���� ����� ���������� �����

    private List<ActionItemsData> actionItemsData = new List<ActionItemsData>();
    private Animator animator; // �������� ���������
    private NavMeshAgent agent;
    private int randomPointIndex;

    private void Start()
    {
        actionItemsData = actionsData.actionItemsData;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(StartCharacterBehavior());
    }

    private IEnumerator StartCharacterBehavior()
    {
        // �������� �������� � ����� � ��� �� ������� � ���������� �����
        TextOutput();

        if (actionsData != null && actionItemsData.Count > 0)
        {
            // �������� ����� �������� �� ������� �������� actionItemsData
            yield return new WaitForSeconds(actionItemsData[0].waitTimeAtPoint);

            while (true)
            {
                MoveToRandomPoint();
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f); // ����, ���� �������� �� ��������� �����

                yield return ExecutePointProcess(); // ��������� ������� �� �����
            }
        }
    }

    private void MoveToRandomPoint()
    {
        int randomIndex = Random.Range(0, points.Length);
        randomPointIndex = randomIndex;
        agent.SetDestination(points[randomIndex].position);
    }

    private IEnumerator ExecutePointProcess()
    {
        if (actionItemsData[randomPointIndex].animations != null)
        {
            AnimationOutput();
        }

        if (actionItemsData[randomPointIndex].associatedGameObjects != null)
        {
            GameObjectPlaceOutput();
        }

        if (actionItemsData[randomPointIndex].soundClips != null)
        {
            SoundOutput();
        }

        if (actionItemsData[randomPointIndex].texts != null)
        {
            TextOutput();
        }

        yield return new WaitForSeconds(actionItemsData[randomPointIndex].waitTimeAtPoint); // ���� ����� ���������� ��������
    }

    private IEnumerator PlayAnimations()
    {
        // ��������� ������ ��������
        animator.SetTrigger(actionItemsData[randomPointIndex].animations[0]);

        // ���� ���������� ��������
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // ���� � ������ ������ ������ ��������, ���������� � ����������
        for (int i = 1; i < actionItemsData[randomPointIndex].animations.Count; i++)
        {
            animator.SetTrigger(actionItemsData[randomPointIndex].animations[i]);

            // ���� ���������� ������ ����������� ��������
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    private void AnimationOutput()
    {
        StartCoroutine(PlayAnimations());
    }
    private void GameObjectPlaceOutput()
    {
        // ���������, ��� ������ associatedGameObjects � gameObjectsPlace ����� ���������� �����
        if (actionItemsData[randomPointIndex].associatedGameObjects.Count != actionItemsData[randomPointIndex].gameObjectsPlace.Count)
        {
            Debug.LogError("���������� GameObjects � ���� ��� �� ���������� �� ���������!");
            return;
        }

        // �������� �� ���� GameObjects � ������������ �� � ��������������� Transforms
        for (int i = 0; i < actionItemsData[randomPointIndex].associatedGameObjects.Count; i++)
        {
            GameObject prefab = actionItemsData[randomPointIndex].associatedGameObjects[i];
            Transform place = actionItemsData[randomPointIndex].gameObjectsPlace[i];

            // ������������ GameObject � ��������� Transform
            Instantiate(prefab, place.position, place.rotation, place);
        }
    }

    
    private IEnumerator PlaySounds()
    {
        // �������� �� ���� ������ � ������������� ��
        for (int i = 0; i < actionItemsData[randomPointIndex].soundClips.Count; i++)
        {
            AudioClip sound = actionItemsData[randomPointIndex].soundClips[i];

            // ������������� ����
            audioPlayer.PlayOneShot(sound);

            // ����, ���� ���� �� ����������
            yield return new WaitForSeconds(sound.length);
        }
    }
    
    private void SoundOutput()
    {
        StartCoroutine(PlaySounds());
    }

    private IEnumerator WaitForTextOutput()
    {
        yield return new WaitForSeconds(actionItemsData[randomPointIndex].waitTimeAtPoint);
        canvas.gameObject.SetActive(false); // �������� canvas ����� ��������
    }

    private void TextOutput()
    {
        // ���������, ��� � ��� ���� ����� ��� ������
        if (actionItemsData[randomPointIndex].texts.Count > 0)
        {
            canvas.gameObject.SetActive(true);
            outputText.text = actionItemsData[randomPointIndex].texts[0]; // ������� ������ �����

            StartCoroutine(WaitForTextOutput()); // ��������� ��������
        }
        else
        {
            Debug.LogWarning("��� ������ ��� ������!");
        }
    }

    public void AddNewPoint(Transform newPoint)
    {
        // ��������� ����� ����� � ������
        List<Transform> pointList = new List<Transform>(points);
        pointList.Add(newPoint);
        points = pointList.ToArray();

        // ��������� NavMesh, ���� ����������
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }
}