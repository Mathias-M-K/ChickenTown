using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlatformAttach : MonoBehaviour
    {
        private Vector3 vel;
       /* var previous: Vector3;
        var velocity: float;*/

       private Vector3 previous;




       private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<ChickenController>().externalVelocity = vel;
        }

       private void OnTriggerStay(Collider other)
       {
           other.GetComponent<ChickenController>().externalVelocity = vel;
       }

       private void OnTriggerExit(Collider other)
        {
            other.GetComponent<ChickenController>().externalVelocity = Vector3.zero;
        }
        
        private void Update()
        {
            vel = ((transform.position - previous)) / Time.deltaTime;
            previous = transform.position;
        }
    }
}


