using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameGUI.NetworkScene
{
    public class JoinRoomPanelUIController : MonoBehaviour
    {
        public TMP_InputField roomNameField;
        public TMP_InputField nicknameField;
        public GameObject joinRoomBtn;
        public GameObject inactiveJoinRoomBtn;

        [Header("Animation values")] 
        public float flyInTime;
        public LeanTweenType flyInEase;
        public float flyOutTime;
        public LeanTweenType flyOutEase;

        private bool readyToJoinRoom;
        private bool panelActive;

        public NetworkSceneController NetworkSceneController;

        private void Update()
        {
            if (!roomNameField.text.Equals("") && !nicknameField.text.Equals(""))
            {
                LeanTween.alpha(joinRoomBtn.GetComponent<Image>().rectTransform, 1, 0.4f).setOnComplete(() => joinRoomBtn.GetComponent<Button>().interactable = true);
                readyToJoinRoom = true;
            }
            else
            {
                LeanTween.alpha(joinRoomBtn.GetComponent<Image>().rectTransform, 0, 0.4f).setOnComplete(() => joinRoomBtn.GetComponent<Button>().interactable = false);
                readyToJoinRoom = false;
            }

            if (Input.GetKeyDown(KeyCode.Tab) && panelActive)
            {
                if (roomNameField.isFocused)
                {
                    nicknameField.Select();
                }
                else
                {
                    roomNameField.Select();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FlyOut();
            }

            if (Input.GetKeyDown(KeyCode.Return) && readyToJoinRoom)
            {
                NetworkSceneController.JoinRoom();
            }
        }
        
        public string GetNameField()
        {
            return roomNameField.text;
        }
        public string GetNicknameField()
        {
            return nicknameField.text;
        }

        public void FlyIn()
        {
            LeanTween.moveLocalY(gameObject, 0, flyInTime).setEase(flyInEase);
            panelActive = true;
        }

        public void FlyOut()
        {
            LeanTween.moveLocalY(gameObject, -238, flyOutTime).setEase(flyOutEase);
            panelActive = false;
        }
        
        public void RunFailedJoinAnimation()
        {
            print("RUN failed animation");
            LeanTween.rotateAroundLocal(joinRoomBtn, new Vector3(0, 0, 1), 5, 0.2f)
                .setEase(LeanTweenType.easeShake)
                .setRepeat(3);
        }

        
    }
}