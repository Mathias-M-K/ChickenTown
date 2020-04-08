using GameGUI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace DefaultNamespace
{
    public class ChickenGameController : MonoBehaviourPunCallbacks
    {
        public static ChickenGameController Current;
        
        private void Awake()
        {
            Current = this;
        }

        [Header("Lights")] 
        [SerializeField] private Light mainLight;
        private float _lightValue;

        [Header("Fireworks")] 
        [SerializeField] private VisualEffect fireworksEffect;

        [Header("Elevators")]
        public ElevatorButtonController[] elevators;
        
        [Header("Photon")]
        private PhotonView _myPhotonView;
        
        private void Start()
        {
            if (!PhotonNetwork.IsConnected) return;
            _myPhotonView = GetComponent<PhotonView>();
            _myPhotonView.Group = 2;
            PhotonNetwork.SetInterestGroups(2,true);
        }
        
        /*
         * Scene light
         */
        public void OnSceneLightChange(float f)
        {
            _lightValue = f;
        }
        public void OnSceneLightSelected()
        {
            _myPhotonView.RPC("RPC_SceneLightChange",RpcTarget.All,_lightValue);
        }
        [PunRPC]
        public void RPC_SceneLightChange(float lightValue)
        {
            float mainLightFrom = mainLight.intensity;
            LeanTween.value(gameObject, a => { mainLight.intensity = a; }, mainLightFrom, lightValue, 0.4f);
        }
        
        /*
         * Firework
         */
        public void OnToggleFireworks(bool toggleValue)
        {
            _myPhotonView.RPC("RPC_ToggleFireworks",RpcTarget.All,toggleValue);
        }
        [PunRPC]
        private void RPC_ToggleFireworks(bool toggleValue)
        {
            float spawnRate = toggleValue ? 5 : 0;
            
            print($"Spawn rate is: {spawnRate}");
            fireworksEffect.SetFloat("SpawnRate",spawnRate);
        }

        /*
         * Elevator
         */
        public void OnElevatorBtnPushed(int elevatorId)
        {
            _myPhotonView.RPC("RPC_ElevatorEvent",RpcTarget.Others,elevatorId);
        }
        [PunRPC]
        public void RPC_ElevatorEvent(int elevatorId)
        {
            foreach (ElevatorButtonController elevator in elevators)
            {
                if (elevator.ElevatorID == elevatorId)
                {
                    StartCoroutine(elevator.ElevatorActivated());
                }
            }
        }
        
        
        
        /*
         * Other
         */
        public void Disconnect()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(GlobalValues.NetworkScene);
        }
        public void Exit()
        {
            Application.Quit();
        }
    }
}