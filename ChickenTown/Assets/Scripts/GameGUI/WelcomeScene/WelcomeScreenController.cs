using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameGUI
{
    public class WelcomeScreenController : MonoBehaviour
    {
        [Header("Content")] 
        public GameObject mainContent;
        public float contentAnimationTime;
        public LeanTweenType contentEaseOutType;
        public LeanTweenType contentEaseInType;
        
        [Header("Buttons")] 
        public Button startBtn;
        
        [Header("Network / Local")] 
        public GameObject networkLocalButtons;
        public float animationTime1;
        public LeanTweenType easeType1;


        private void Start()
        {
            LeanTween.moveLocalX(mainContent, -1236, 0);
            LeanTween.moveLocalX(mainContent, 0, contentAnimationTime).setEase(contentEaseInType);

            if (!GlobalValues.StartBtnInteractable)
            {
                startBtn.interactable = false;
                LeanTween.moveLocalY(networkLocalButtons, 0, 0);
            }
        }

        public void ButtonHit(Button b)
        {
            if (b.name.Equals("StartBtn"))
            {
                startBtn = b;
                b.interactable = false;
                LeanTween.moveLocalY(networkLocalButtons, 0, animationTime1).setEase(easeType1);
                GlobalValues.SetStartBtnInteractable(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (!startBtn.interactable)
                {
                    startBtn.interactable = true;
                    LeanTween.moveLocalY(networkLocalButtons, 140, animationTime1).setEase(easeType1);
                    GlobalValues.SetStartBtnInteractable(true);
                }
            }
        }

        //Btn method
        public void OnlineGame()
        {
            LeanTween.moveLocalX(mainContent, -1236, contentAnimationTime).
                setEase(contentEaseOutType).
                setOnComplete(() => SceneManager.LoadScene(GlobalValues.NetworkScene));
        }

        public void LocalGame()
        {
            SceneManager.LoadScene(GlobalValues.GameScene);
        }
    }
}