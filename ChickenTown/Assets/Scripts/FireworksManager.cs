using System;
using UnityEngine;
using UnityEngine.VFX;

namespace DefaultNamespace
{
    public class FireworksManager : MonoBehaviour
    {
        private bool _active;

        public void Toggle()
        {
            _active = !_active;
            gameObject.SetActive(_active);
        }
    }
}