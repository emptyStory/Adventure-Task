using UnityEngine;

namespace Controllers.UI
{
    public class CharacterPickController : MonoBehaviour
    {
        public GameObject[] characters;

        private void Start()
        {
            if (characters == null || characters.Length == 0)
            {
                enabled = false;
            }
        }

        public void PickCharacter(int index)
        {
            if (index < 0 || index >= characters.Length)
            {
                Debug.Log("Wrong character index. Got " + index + " Length: " + characters.Length);
                return;
            }

            for (var i = 0; i < characters.Length; i++)
            {
                characters[i].SetActive(index == i);
            }
        }
    }
}