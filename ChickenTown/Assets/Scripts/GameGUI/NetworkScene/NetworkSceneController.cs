using System;
using Michsky.UI.ModernUIPack;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameGUI.NetworkScene
{
    public class NetworkSceneController : MonoBehaviourPunCallbacks
    {
        [Header("Assistant Classes")] 
        public CreateRoomPanelUIController CreateRoomUI;
        public JoinRoomPanelUIController JoinRoomUI;

        [Header("Content")] 
        public GameObject mainContent;
        public float contentAnimationTime;
        public LeanTweenType contentEaseInType;
        public LeanTweenType contentEaseOutType;

        [Header("Buttons")] 
        public GameObject backBtn;
        public GameObject createRoomBtn;
        public GameObject loadingBtn;
        
        public float buttonAnimationTime;
        public LeanTweenType buttonEaseType;

        private void Awake()
        {
            LeanTween.moveLocalX(mainContent, 1243, 0);
            if (Application.internetReachability == NetworkReachability.NotReachable)           
            {
                print("No network detected");
            }
            if (!PhotonNetwork.IsConnected)
            {
                LeanTween.moveLocalY(backBtn, backBtn.transform.localPosition.y + 55+10, 0);
                LeanTween.moveLocalY(createRoomBtn, createRoomBtn.transform.localPosition.y + 55 + 10, 0);
            }
            else
            {
                loadingBtn.SetActive(false);
            }
        }

        private void Start()
        {
            LeanTween.moveLocalX(mainContent, 0, contentAnimationTime).setEase(contentEaseInType);
        }

        public void Back()
        {
            LeanTween.moveLocalX(mainContent, 1243, contentAnimationTime).setEase(contentEaseOutType).setOnComplete(
                () => SceneManager.LoadScene(0));
        }

        private void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (!GlobalValues.Connected) return;
                OnConnectionLost();
            }
        }


        /*
         * Network Stuff
         */
        public void RetryConnection()
        {
            loadingBtn.GetComponent<LoadingBtnController>().StartLoadingAnimation();
            PhotonNetwork.ConnectUsingSettings();
        }

        public void OnConnectionLost()
        {
            GlobalValues.SetConnected(false);
            Image img = loadingBtn.GetComponent<Image>();
            LeanTween.color(img.rectTransform, new Color32(255, 255, 255, 255), 1);
            loadingBtn.GetComponent<LoadingBtnController>().SetAsRetryBtn();
            
            LeanTween.moveLocalY(backBtn, backBtn.transform.localPosition.y + 55 + 10, buttonAnimationTime)
                .setEase(buttonEaseType);

            LeanTween.moveLocalY(createRoomBtn, createRoomBtn.transform.localPosition.y + 55 + 10, buttonAnimationTime)
                .setEase(buttonEaseType);

            loadingBtn.SetActive(true);
            LeanTween.alpha(loadingBtn.GetComponent<Image>().rectTransform, 1, buttonAnimationTime)
                .setEase(buttonEaseType);
                
            TextMeshProUGUI loadingBtnText = loadingBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            LeanTween.value(loadingBtnText.gameObject, a =>
            {
                Color32 textColor = loadingBtnText.color;
                textColor.a = (byte) a;
                loadingBtnText.color = textColor;
            }, 0, 255, 0.4f);
        }
        
        public override void OnConnectedToMaster()
        {
            if (GlobalValues.Connected) return;
            
            GlobalValues.SetConnected(true);
            
            PhotonNetwork.AutomaticallySyncScene = true;
            
            loadingBtn.GetComponent<LoadingBtnController>().StopLoadingAnimation();
            loadingBtn.GetComponent<LoadingBtnController>().SetText("Connected!");
            Image img = loadingBtn.GetComponent<Image>();

            LeanTween.color(img.rectTransform, new Color32(0,250,126,255), 0.5f).setOnComplete(() =>
            {
                LeanTween.moveLocalY(backBtn, backBtn.transform.localPosition.y - 55 - 10, buttonAnimationTime)
                    .setEase(buttonEaseType);

                LeanTween.moveLocalY(createRoomBtn, createRoomBtn.transform.localPosition.y - 55 - 10, buttonAnimationTime)
                    .setEase(buttonEaseType);

                LeanTween.alpha(loadingBtn.GetComponent<Image>().rectTransform, 0, buttonAnimationTime)
                    .setEase(buttonEaseType).setOnComplete(() => loadingBtn.SetActive(false));
                
                TextMeshProUGUI loadingBtnText = loadingBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                LeanTween.value(loadingBtnText.gameObject, a =>
                {
                    Color32 textColor = loadingBtnText.color;
                    textColor.a = (byte) a;
                    loadingBtnText.color = textColor;
                }, 255, 0, 0.2f);
            });
        }
        
        
        public void CreateRoom()
        {
            int.TryParse(CreateRoomUI.GetSizeField(), out int roomSize);
            RoomOptions roomOps = new RoomOptions(){IsVisible = true,IsOpen = true,MaxPlayers = (byte) roomSize};

            PhotonNetwork.LocalPlayer.NickName = "Mr. Host";
            PhotonNetwork.CreateRoom(CreateRoomUI.GetNameField(), roomOps);
        }
        
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            CreateRoomUI.RunRoomFailedAnimation();  
        }

        public void JoinRoom()
        {
            Debug.Log("Joining Room...");

            PhotonNetwork.LocalPlayer.NickName = JoinRoomUI.GetNicknameField();
            PhotonNetwork.JoinRoom(JoinRoomUI.GetNameField());
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            JoinRoomUI.RunFailedJoinAnimation();
        }

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene(GlobalValues.WaitingRoomScene);
        }
        
        
    }
}