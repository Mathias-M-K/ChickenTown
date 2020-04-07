using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ElevatorController : MonoBehaviour
    {
        public int ElevatorId;

        private Vector3 _originalPosition;
        public Vector3 _targetPos;

        [Header("Animation values")] 
        public float animationTime;
        public LeanTweenType easeType;

        private void Start()
        {
            _originalPosition = transform.localPosition;
        }

        public void DoAction(PlatformDirection Direction)
        {

            switch (Direction)
            {
                case PlatformDirection.Target:
                    LeanTween.moveLocal(gameObject, _targetPos, animationTime).setEase(easeType);
                    break;
                case PlatformDirection.Origin:
                    LeanTween.moveLocal(gameObject, _originalPosition, animationTime).setEase(easeType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }

    
}