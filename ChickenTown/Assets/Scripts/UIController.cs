using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace DefaultNamespace
{
    public class UIController : MonoBehaviour
    {
        public static UIController current;

        private void Awake()
        {
            current = this;
        }

        private ChickenController chickenController;

        public GameObject messageInputObject;
        public TMP_InputField messageInput;

        private bool _messageFieldOpen;

        private void Start()
        {
            LeanTween.scale(messageInputObject, new Vector3(0, 0, 0), 0);
        }

        private void Update()
        {
            if (chickenController == null) return;
            if (!chickenController._photonView.IsMine) return;
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (_messageFieldOpen)
                {
                    if (messageInput.text != "")
                    {
                        chickenController.SendChickenMessage(messageInput.text);
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
            chickenController = newChickenController;
        }
    }
}