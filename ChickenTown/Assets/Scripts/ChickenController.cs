using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChickenController : MonoBehaviourPunCallbacks
{
    public PhotonView _photonView;
    
    public CharacterController controller;
    private float velX;
    private float velY;
    private float velZ;

    private bool outOfBounds;
    [HideInInspector]public Vector3 moveVector;
    public Vector3 externalVelocity = Vector3.zero;

    private Vector3 _actualVel;
    
    public float maxSpeed;
    public float jumpSpeed;
    public float speed;
    public float airSpeed;
    public float gravity;
    public float deAccValue;
    
    private bool _movement;
    
    private List<Color32> colors = new List<Color32>(){new Color32(255,162,162,255),new Color32(162,214,255,255),new Color32(132,255,140,255),new Color32(255,255,255,255)};
    private int colorCounter = 0;

    [Header("Message Components")] 
    public GameObject speechBubbleObj;
    public TextMeshProUGUI speechBubbleText;

    private bool _messageBeingShowed;
    private bool _messageInQue;
    
    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();

        if (_photonView.IsMine)
        {
            Camera.main.GetComponent<CustomCameraMovement>().SetChicken(gameObject);
            UIController.current.SetChickenController(this);
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        
        //Anti suicide code
        if (outOfBounds) return;
        if (transform.position.y < -4)
        {
            
            outOfBounds = true;
            LeanTween.moveLocalY(gameObject, 5, 2).setEase(LeanTweenType.easeInOutElastic).setOnComplete(() =>
                {
                    LeanTween.moveLocal(gameObject, new Vector3(7, 5, -7), 2).setEase(LeanTweenType.easeInOutExpo).setOnComplete(() => outOfBounds = false);
                });
        }

        
        
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!_photonView.IsMine) return;
            ChangeColor(_photonView.ViewID);
            
            _photonView.RPC("ChangeColor",RpcTarget.Others,_photonView.ViewID);
        }
        
        if (!_photonView.IsMine) return;

        //Ground Movement Control
        
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (velZ < 0)
            {
                velZ = 0;
            }

            if (controller.isGrounded)
            {
                velZ += speed;
            }else
            {
                velZ += airSpeed;
            }
            if (velZ > maxSpeed)
            {
                velZ = maxSpeed;
            }
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            //Breaking if we're going the other way
            if (velZ > 0)
            {
                velZ = 0;
            }
            //if we're on the ground
            if (controller.isGrounded)
            {
                velZ -= speed;
            }else
            {
                velZ -= airSpeed;
            }
            
            if (velZ < -maxSpeed)
            {
                velZ = -maxSpeed;
            }
        }
        else
        {
            //Deaccelarate 
            if (Math.Abs(velZ - 0) < 1)
            {
                velZ = 0;
            }
            
            if (velZ > 0)
            {
                velZ -= deAccValue;
            }

            if (velZ < 0)
            {   
                velZ += deAccValue;
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (velX > 0)
            {
                velX = 0;
            }
            if (controller.isGrounded)
            {
                velX -= speed;
            }else
            {
                velX -= airSpeed;
            }
            if (velX < -maxSpeed)
            {
                velX = -maxSpeed;
            }
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (velX < 0)
            {
                velX = 0;
            }
            
            if (controller.isGrounded)
            {
                velX += speed;
            }else
            {
                velX += airSpeed;
            }
            
            if (velX > maxSpeed)
            {
                velX = maxSpeed;
            }
        }
        else
        {
            if (Math.Abs(velX - 0) < 1)
            {
                velX = 0;
            }
            if (velX > 0)
            {
                velX -= deAccValue;
            }

            if (velX < 0)
            {   
                velX += deAccValue;
            }
        }
        
        if (controller.isGrounded)
        {
            velY = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velY = jumpSpeed;
            }
        }
        
        velY -= gravity * Time.deltaTime;
        
        Vector3 newPosition = new Vector3(velX, 0.0f, velZ);
        transform.LookAt(newPosition + transform.position);

        moveVector = new Vector3(velX * Time.deltaTime, velY * Time.deltaTime, velZ * Time.deltaTime) + (externalVelocity*Time.deltaTime);
        controller.Move(moveVector);

        
    }
    
    [PunRPC]
    private void ChangeColor(int viewId)
    {
        if (photonView.ViewID != viewId) return;
        GetComponent<Renderer>().material.color = colors[colorCounter];

        colorCounter++;

        if (colorCounter > 3)
        {
            colorCounter = 0;
        }
    }

    [PunRPC]
    public void ReceiveMessage(string message, int viewID)
    {
        if (viewID == _photonView.ViewID)
        {
            StartCoroutine(ShowMessageForSeconds(message,4));

            if (_messageBeingShowed)
            {
                _messageInQue = true;
            }

            _messageBeingShowed = true;
        }
    }

    private IEnumerator ShowMessageForSeconds(string message, int seconds)
    {
        speechBubbleObj.SetActive(true);
        speechBubbleText.text = message;

        yield return new WaitForSeconds(seconds);

        if (_messageInQue)
        {
            _messageInQue = false;
            yield break;
        }
        speechBubbleObj.SetActive(false);
        _messageInQue = false;
        _messageBeingShowed = false;
    }
    
    public void SendChickenMessage(string message)
    {
        _photonView.RPC("ReceiveMessage",RpcTarget.All,message,_photonView.ViewID);
    }
}
