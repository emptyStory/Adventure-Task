using System;
using UnityEngine;

namespace Controllers.UI
{
    public class SlideChanger : MonoBehaviour
    {
        public GameObject login;
        public GameObject mainMenu;
        public GameObject characterPick;
        public GameObject levelPick;
        public GameObject loading;

        private GameObject[] _allSlides;

        private void Start()
        {
            _allSlides = new[]{login, mainMenu, characterPick, levelPick, loading};
            OpenInitSlide();
        }

        private void OpenInitSlide()
        {
            switch (ApplicationContext.InitSlide)
            {
                case ApplicationContext.Slide.Login:
                    OpenLogin();
                    break;
                case ApplicationContext.Slide.Menu:
                    OpenMainMenu();
                    break;
                case ApplicationContext.Slide.CharacterSelection:
                    OpenCharacterPick();
                    break;
                case ApplicationContext.Slide.Level:
                    OpenLevelPick();
                    break;
                case ApplicationContext.Slide.Loading:
                    OpenLoading();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OpenLogin()
        {
            DisableAll();
            login.SetActive(true);
        }
        
        public void OpenMainMenu()
        {
            DisableAll();
            mainMenu.SetActive(true);
        }
        
        public void OpenCharacterPick()
        {
            DisableAll();
            characterPick.SetActive(true);
        }
        
        public void OpenLevelPick()
        {
            DisableAll();
            levelPick.SetActive(true);
        }
        
        public void OpenLoading()
        {
            DisableAll();
            loading.SetActive(true);
        }

        private void DisableAll()
        {
            foreach (var slide in _allSlides)
            {
                slide.SetActive(false);
            }
        }
    }
}