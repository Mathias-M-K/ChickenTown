using System;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
    public class ChickenGameController : MonoBehaviourPunCallbacks
    {
        public static ChickenGameController Current;
        
        private void Awake()
        {
            Current = this;
        }

        public ElevatorButtonController[] elevators;
        private PhotonView _myPhotonView;
        
        private void Start()
        {
            _myPhotonView = GetComponent<PhotonView>();
            _myPhotonView.Group = 2;
            PhotonNetwork.SetInterestGroups(2,true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _myPhotonView.RPC("PrintOut",RpcTarget.Others,"Test message");
            }
        }

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
        
    }
}