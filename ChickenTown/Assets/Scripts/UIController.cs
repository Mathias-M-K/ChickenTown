using System;
using GameGUI;
using GameGUI.Game;
using Michsky.UI.ModernUIPack;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class UIController : MonoBehaviour
    {
        public static UIController current;

        private void Awake()
        {
            current = this;
        }

        [Header("Player UI")] 
        public PlayerMenuController menuController;
        private bool _playerUiOpen;
        
        [Header("Other")]
        private ChickenController _chickenController;

        public GameObject messageInputObject;
        public TMP_InputField messageInput;

        private bool _messageFieldOpen;

        private void Start()
        {
            LeanTween.scale(messageInputObject, new Vector3(0, 0, 0), 0);
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_playerUiOpen)
                {
                    menuController.Close();
                }
                else
                {
                    menuController.Open();
                }

                _playerUiOpen = !_playerUiOpen;
            }
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_chickenController == null) return;
                if (!_chickenController._photonView.IsMine) return;
                
                if (_messageFieldOpen)
                {
                    if (messageInput.text != "")
                    {
                        _chickenController.SendChickenMessage(messageInput.text);
                        print("Sending message");
                    }

                    messageInput.text = "";
                    
                    LeanTween.scale(messageInputObject,new Vector3(0,0,0), 0.5f).setEase(LeanTweenType.easeInQuint).setOnComplete(()=> messageInputObject.SetActive(false));
                    _messageFieldOpen = false;
                }else{
                    messageInputObject.SetActive(true);
                    messageInput.Select();
                    messageInput.ActivateInputField();
                    LeanTween.scale(messageInputObject, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutQuint);
                    _messageFieldOpen = true;
                }
            }
        }

        public void SetChickenController(ChickenController newChickenController)
        {
            _chickenController = newChickenController;
        }
    }
}