using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class ElevatorButtonController : MonoBehaviour
    {
        public int ElevatorID;
        public PlatformDirection direction;
        public ElevatorController elevator;
        
        [Header("ElevatorBtn")]
        public GameObject elevatorBtn;
        public Vector3 buttonInactivePos;
        public LeanTweenType buttonEaseType;
        private Vector3 buttonActivePos;

        private bool elevatorActive;

        private void Start()
        {
            buttonActivePos = transform.localPosition;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (elevatorActive) return;
            
            ChickenGameController.Current.OnElevatorBtnPushed(ElevatorID);
            StartCoroutine(ElevatorActivated());
        }

        public IEnumerator ElevatorActivated()
        {
            elevatorActive = true;
            LeanTween.moveLocal(elevatorBtn, buttonInactivePos, 0.5f).setEase(buttonEaseType);
            
            elevator.DoAction(direction);
            yield return new WaitForSeconds(elevator.animationTime);

            elevatorActive = false;
            LeanTween.moveLocal(elevatorBtn, buttonActivePos, 0.5f).setEase(buttonEaseType);
            if (direction == PlatformDirection.Origin)
            {
                direction = PlatformDirection.Target;
            }
            else
            {
                direction = PlatformDirection.Origin;
            }
        }
    }
    
    public enum PlatformDirection
    {
        Target,
        Origin
    }
}