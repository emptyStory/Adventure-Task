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
    public Transform[] points; // Массив точек, к которым будет двигаться персонаж
    public ActionItem actionsData;
    public AudioSource audioPlayer;

    // Поля для канваса и текстового элемента
    public Canvas canvas; // Ссылка на ваш канвас
    public TMP_Text outputText; // Ссылка на текстовый элемент, куда будет выводиться текст

    private List<ActionItemsData> actionItemsData = new List<ActionItemsData>();
    private Animator animator; // Аниматор персонажа
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
        // Персонаж начинает с одной и той же позиции и произносит фразу
        TextOutput();

        if (actionsData != null && actionItemsData.Count > 0)
        {
            // Получаем время ожидания из первого элемента actionItemsData
            yield return new WaitForSeconds(actionItemsData[0].waitTimeAtPoint);

            while (true)
            {
                MoveToRandomPoint();
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f); // Ждем, пока персонаж не достигнет точки

                yield return ExecutePointProcess(); // Выполняем процесс на точке
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

        yield return new WaitForSeconds(actionItemsData[randomPointIndex].waitTimeAtPoint); // Ждем время выполнения процесса
    }

    private IEnumerator PlayAnimations()
    {
        // Запускаем первую анимацию
        animator.SetTrigger(actionItemsData[randomPointIndex].animations[0]);

        // Ждем завершения анимации
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Если в списке больше одного триггера, продолжаем с остальными
        for (int i = 1; i < actionItemsData[randomPointIndex].animations.Count; i++)
        {
            animator.SetTrigger(actionItemsData[randomPointIndex].animations[i]);

            // Ждем завершения каждой последующей анимации
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    private void AnimationOutput()
    {
        StartCoroutine(PlayAnimations());
    }
    private void GameObjectPlaceOutput()
    {
        // Проверяем, что списки associatedGameObjects и gameObjectsPlace имеют одинаковую длину
        if (actionItemsData[randomPointIndex].associatedGameObjects.Count != actionItemsData[randomPointIndex].gameObjectsPlace.Count)
        {
            Debug.LogError("Количество GameObjects и мест для их размещения не совпадает!");
            return;
        }

        // Проходим по всем GameObjects и инстанцируем их в соответствующие Transforms
        for (int i = 0; i < actionItemsData[randomPointIndex].associatedGameObjects.Count; i++)
        {
            GameObject prefab = actionItemsData[randomPointIndex].associatedGameObjects[i];
            Transform place = actionItemsData[randomPointIndex].gameObjectsPlace[i];

            // Инстанцируем GameObject в указанном Transform
            Instantiate(prefab, place.position, place.rotation, place);
        }
    }

    
    private IEnumerator PlaySounds()
    {
        // Проходим по всем звукам и воспроизводим их
        for (int i = 0; i < actionItemsData[randomPointIndex].soundClips.Count; i++)
        {
            AudioClip sound = actionItemsData[randomPointIndex].soundClips[i];

            // Воспроизводим звук
            audioPlayer.PlayOneShot(sound);

            // Ждем, пока звук не завершится
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
        canvas.gameObject.SetActive(false); // Скрываем canvas после ожидания
    }

    private void TextOutput()
    {
        // Проверяем, что у нас есть текст для вывода
        if (actionItemsData[randomPointIndex].texts.Count > 0)
        {
            canvas.gameObject.SetActive(true);
            outputText.text = actionItemsData[randomPointIndex].texts[0]; // Выводим первый текст

            StartCoroutine(WaitForTextOutput()); // Запускаем корутину
        }
        else
        {
            Debug.LogWarning("Нет текста для вывода!");
        }
    }

    public void AddNewPoint(Transform newPoint)
    {
        // Добавляем новую точку в список
        List<Transform> pointList = new List<Transform>(points);
        pointList.Add(newPoint);
        points = pointList.ToArray();

        // Обновляем NavMesh, если необходимо
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }
}