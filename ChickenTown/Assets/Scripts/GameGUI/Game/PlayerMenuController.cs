using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace GameGUI.Game
{
    public class PlayerMenuController : MonoBehaviour
    {
        [SerializeField] private Image backdrop;
        [Header("Room owner")]
        [SerializeField] private GameObject roomOwnerContent;
        
        [Header("Player")]
        [SerializeField] private GameObject playerContent;

        [Header("Animation Values")] 
        [SerializeField] private float animationTime;
        [SerializeField] private LeanTweenType easeTypeIn;
        [SerializeField] private LeanTweenType easeTypeOut;
        
        public void Open()
        {
            LeanTween.alpha(backdrop.rectTransform, 0.8f, animationTime);
            LeanTween.moveLocalX(playerContent, 0, animationTime).setEase(easeTypeIn);
            
            if (PhotonNetwork.IsConnected)
            {
                if (!PhotonNetwork.IsMasterClient) return;
            }
            
            
            LeanTween.moveLocalX(roomOwnerContent, 0, animationTime).setEase(easeTypeIn);
        }

        public void Close()
        {
            LeanTween.alpha(backdrop.rectTransform, 0, animationTime);
            LeanTween.moveLocalX(playerContent, 1920/2, animationTime).setEase(easeTypeOut);
            
            if (PhotonNetwork.IsConnected)
            {
                if (!PhotonNetwork.IsMasterClient) return;
            }
            
            LeanTween.moveLocalX(roomOwnerContent, -1920/2, animationTime).setEase(easeTypeOut);
            
        }
    }
}