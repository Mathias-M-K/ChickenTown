using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameGUI.NetworkScene
{
    public class CreateRoomPanelUIController : MonoBehaviour
    {
        public TMP_InputField roomNameField;
        public TMP_InputField roomSizeField;
        public GameObject createRoomBtn;
        public GameObject inactiveCreateRoomBtn;

        [Header("Animation values")] 
        public float flyInTime;
        public LeanTweenType flyInEase;
        public float flyOutTime;
        public LeanTweenType flyOutEase;

        private bool readyToCreateRoom;
        private bool panelActive;

        public NetworkSceneController NetworkSceneController;

        private void Update()
        {
            if (!roomNameField.text.Equals("") && !roomSizeField.text.Equals(""))
            {
                createRoomBtn.GetComponent<Button>().interactable = true;
                readyToCreateRoom = true;
                LeanTween.alpha(createRoomBtn.GetComponent<Image>().rectTransform, 1, 0.4f); //setOnComplete(() => createRoomBtn.GetComponent<Button>().interactable = true);

            }
            else
            {
                createRoomBtn.GetComponent<Button>().interactable = false;
                readyToCreateRoom = false;
                LeanTween.alpha(createRoomBtn.GetComponent<Image>().rectTransform, 0, 0.4f);//setOnComplete(() => createRoomBtn.GetComponent<Button>().interactable = false);
            }

            if (Input.GetKeyDown(KeyCode.Tab) && panelActive)
            {
                if (roomNameField.isFocused)
                {
                    roomSizeField.Select();
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

            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && readyToCreateRoom)
            {
                NetworkSceneController.CreateRoom();
            }
        }
        
        public string GetNameField()
        {
            return roomNameField.text;
        }
        public string GetSizeField()
        {
            return roomSizeField.text;
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
        
        public void RunRoomFailedAnimation()
        {
            print("RUN failed animation");
            LeanTween.rotateAroundLocal(createRoomBtn, new Vector3(0, 0, 1), 5, 0.2f)
                .setEase(LeanTweenType.easeShake)
                .setRepeat(3);
        }

        
    }
}