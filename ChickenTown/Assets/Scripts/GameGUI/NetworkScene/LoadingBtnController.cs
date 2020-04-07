using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace GameGUI.NetworkScene
{
    public class LoadingBtnController : MonoBehaviourPunCallbacks
    {
        private TextMeshProUGUI _loadingText;
        private Image _loadingColor;
        private Button _loadingBtn;

        private UIGradient gradient;
        private GradientColorKey[] originalColorkeys;

        private bool _loading = true;

        private void Start()
        {
            _loadingText = GetComponentInChildren<TextMeshProUGUI>();
            _loadingBtn = GetComponent<Button>();
            _loadingColor = GetComponent<Image>();
            gradient = GetComponent<UIGradient>();

            originalColorkeys = gradient.EffectGradient.colorKeys;
            
            StartCoroutine(LoadingAnimation());
        }

        private IEnumerator LoadingAnimation()
        {
            SetNewGradient(originalColorkeys);
            int counter = 0;
            while (_loading)
            {
                _loadingText.text = "Loading";
                
                for (int i = 0; i < 3; i++)
                {
                    if (!_loading) yield break;
                    _loadingText.text += ".";
                    yield return new WaitForSeconds(0.4f);
                }
                
                for (int i = 0; i < 3; i++)
                {
                    if (!_loading) yield break;
                    _loadingText.text = _loadingText.text.Remove(_loadingText.text.Length - 1);
                    yield return new WaitForSeconds(0.4f);
                }

                counter++;

                if (counter > 2)
                {
                    _loading = false;
                    SetAsRetryBtn();
                    break;
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            _loading = false;
        }

        public void StartLoadingAnimation()
        {
            _loading = true;
            StartCoroutine(LoadingAnimation());
        }

        public void StopLoadingAnimation()
        {
            _loading = false;
        }

        public void SetAsRetryBtn()
        {
            print("set retry");
            SetNewGradient(new Color32(211, 84, 0,255), new Color32(230, 126, 34,255));
            
            _loadingBtn.interactable = true;
            _loadingText.text = "Retry Connection";
        }

        public void SetText(string text)
        {
            _loadingText.text = text;
        }

        public void SetNewGradient(GradientColorKey[] keys)
        {
            gradient.EffectGradient.colorKeys = keys;
            gradient.Offset += 0.1f;
            gradient.Offset -= 0.1f;
        }
        public void SetNewGradient(Color32 colLeft, Color32 colRight)
        {
            GradientColorKey key = new GradientColorKey(colLeft,0);
            GradientColorKey key1 = new GradientColorKey(colRight, 1);
            gradient.EffectGradient.colorKeys = new[]{key,key1};
            gradient.Offset += 0.1f;
            gradient.Offset -= 0.1f;
        }
    }
}