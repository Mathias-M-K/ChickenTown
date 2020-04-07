using System;
using System.Security.AccessControl;
using UnityEngine;

public class CustomCameraMovement : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject chicken;

    public float offsetX;
    public float offsetY;
    public float offsetZ;


    public float smoothingFactor;
    public float jumpSmoothingFactor;

    private bool begin;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (!begin || chicken == null) return;
        
        float xPos = Mathf.Lerp(transform.position.x, chicken.transform.position.x+offsetX, Time.deltaTime * smoothingFactor);
        float zPos = Mathf.Lerp(transform.position.z, chicken.transform.position.z+offsetZ, Time.deltaTime * smoothingFactor);
        
        float yPos = Mathf.Lerp(transform.position.y, chicken.transform.position.y+offsetY, Time.deltaTime * jumpSmoothingFactor);

        mainCamera.transform.position = new Vector3(xPos, yPos, zPos);
    }

    public void SetChicken(GameObject gameObject)
    {
        chicken = gameObject;
        begin = true;
    }
    


}


