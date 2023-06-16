using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 inputVector { get; set; }
    public bool inputOpen { get; set; }

    //Mobile Input
    public bool mobileInput;
    Touch touch;
    Vector3 touchDown;
    Vector3 touchUp;

    //Geçici
    public float pcSpeed, mobileSpeed;
    public float speed { get; private set; }
    [SerializeField] float smoothVelocity;
    [SerializeField] float rotationTime = .1f;

    private void Awake()
    {
        speed = mobileInput ? mobileSpeed : pcSpeed;
    }

    public PlayerInput()
    {
        inputOpen = true;
    }

    private void Update()
    {        
        if (mobileInput)
        {
            if (Input.touchCount > 0)
            {
                print("MOBILE INPUT");

                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) // down
                {
                    touchDown = touch.position;
                    touchUp = touch.position; //silinmeli mi
                }
                if (touch.phase == TouchPhase.Ended) // down
                {                    
                    touchUp = touch.position;
                    //animator.SetBool(GameController.Parameters.moving, false);
                }
                else
                {
                    touchDown = touch.position;
                    SetTransform((touchDown - touchUp).normalized, true, true);
                    //animator.SetBool(GameController.Parameters.moving, true);
                }
            }
        }
        else
        {
            inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            SetTransform(new Vector3(inputVector.x, inputVector.z, 0).normalized, true, true);
        }
    }

    void SetTransform(Vector3 movement, bool setRotation, bool setPosition)
    {
        if (setRotation)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, rotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (setPosition)
        {
            
            transform.position += Vector3.forward * movement.y * (mobileInput ? mobileSpeed : pcSpeed) * Time.deltaTime;
            transform.position += Vector3.right * movement.x * (mobileInput ? mobileSpeed : pcSpeed) * Time.deltaTime;
        }
    }
}
