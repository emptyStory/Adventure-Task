using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using static TimelineItems;

public class CharacterMovementV2 : MonoBehaviour
{
    public Transform[] points; // ������ �����, � ������� ����� ��������� ��������
    public TimelineItems timelinesData; // ������ ����������
    public PlayableDirector playableDirector; // ��������� PlayableDirector
    public Animator animator; // �������� ���������

    // ������ ��������� �������
    public List<string> startTexts = new List<string>();
    public int startTextOutputDuration;

    // ���� ��� ������� � ���������� ��������
    public Canvas canvas; // ������ �� ��� ������
    public TMP_Text outputText; // ������ �� ��������� �������, ���� ����� ���������� �����

    public Transform character;
    public Vector3 position;

    private List<TimelineItemsData> timelineItemsData = new List<TimelineItemsData>();
    private NavMeshAgent agent;
    private int currentPointIndex = -1; // ��������� �������� -1 ��� ���������� ������ ������ �����

    private void Start()
    {
        position = character.position;

        timelineItemsData = timelinesData.timelineItemsData;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(StartCharacterBehavior());
        position = agent.transform.position;
    }

    private IEnumerator StartCharacterBehavior()
    {
        StartTextOutput();
        // ���� ��������� ����� ����� ������� ��������
        yield return new WaitForSeconds(5);

        while (true) // ����������� ���� ��� ����������� ��������
        {
            animator.SetTrigger("isWalking");
            MoveToRandomPoint();

            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);

            animator.SetBool("isWalking", false);
            animator.SetTrigger("isIdle");

            yield return ExecutePointProcess(); // ��������� ������� �� ������� �����
        }
    }

    private void MoveToRandomPoint()
    {
        // ���������, ��� ������ ����� �� ������
        if (points == null || points.Length == 0)
        {
            Debug.LogError("������ ����� ���� ��� �� ��������!");
            return; // ���������� ����������, ���� ������ ����
        }

        int newPointIndex;

        // ���������� ����� ������, ���� �� �� ����� ���������� �� ��������
        do
        {
            newPointIndex = Random.Range(0, points.Length);
        } while (newPointIndex == currentPointIndex);

        currentPointIndex = newPointIndex; // ��������� ������� ������
        // animator.SetTrigger("isWalking");
        agent.SetDestination(points[currentPointIndex].position); // ��������� � ����� �����
    }

    private IEnumerator ExecutePointProcess()
    {
        if (currentPointIndex >= timelineItemsData.Count) // ���������� �� >=
        {
            yield break; // ��������� ����������, ���� ������ ��� ���������
        }

        TimelineItemsData currentTimeline = timelineItemsData[currentPointIndex];
        PlayTimeline(currentTimeline);
        TextOutput(); // ��������� ����� ������

        // �������� ���������� ���������
        while (playableDirector.state == PlayState.Playing) // ���������� �� Playing
        {
            yield return null; // ����, ���� �������� �� ����������
        }
    }

    private void PlayTimeline(TimelineItemsData timeline)
    {
        playableDirector.playableAsset = timeline.playableAsset; // ������������� ��������������� ��������
        playableDirector.Play(); // ��������� ��������������� ���������

        // ������������� �� ������� ��������� ���������
        playableDirector.stopped += OnTimelineStopped;
    }

    private void ResetCharacterPosition()
    {
        // �������� ������� ������� ���������
        Vector3 position = agent.transform.position;

        // ���������� Y-����������, ����� �������� ��� �� ������ �����
        position.y = 0; // ��� ����������� ������, ������� ������������� ������ ����� � ����� �����

        // ��������� ����� �������
        agent.transform.position = position;

        // ���������� �������� ���������
        agent.transform.rotation = Quaternion.identity; // ��� ����������� ��������� ��������
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        Vector3 newPosition = agent.transform.position;

        newPosition.y = position.y;

        // ����������� ����� �������
        character.transform.position = newPosition;

        playableDirector.stopped -= OnTimelineStopped;
    }

    private IEnumerator WaitForTextOutput()
    {
        yield return new WaitForSeconds(timelineItemsData[currentPointIndex].textDuration);
        canvas.gameObject.SetActive(false); // �������� canvas ����� ��������
    }

    private IEnumerator WaitForStartTextOutput()
    {
        yield return new WaitForSeconds(startTextOutputDuration);
        canvas.gameObject.SetActive(false); // �������� canvas ����� ��������
    }

    private void StartTextOutput()
    {
        
       
            canvas.gameObject.SetActive(true);

            // �������� ��������� ������ ��� ������
            int randomTextIndex = Random.Range(0, startTexts.Count);

            // ������� ��������� �����
            outputText.text = startTexts[randomTextIndex];

            StartCoroutine(WaitForStartTextOutput()); // ��������� ��������
    }

    private void TextOutput()
    {
        // ���������, ��� � ��� ���� ����� ��� ������
        if (timelineItemsData[currentPointIndex].texts.Count > 0)
        {
            canvas.gameObject.SetActive(true);

            // �������� ��������� ������ ��� ������
            int randomTextIndex = Random.Range(0, timelineItemsData[currentPointIndex].texts.Count);

            // ������� ��������� �����
            outputText.text = timelineItemsData[currentPointIndex].texts[randomTextIndex];

            StartCoroutine(WaitForTextOutput()); // ��������� ��������
        }
        else
        {
            Debug.LogWarning("��� ������ ��� ������!");
        }
    }
}
