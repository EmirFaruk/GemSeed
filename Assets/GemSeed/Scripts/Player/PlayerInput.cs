using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Variables

    //Vectors
    public Vector3 inputVector { get; private set; }
    //Mobile
    private Touch touch;
    private Vector3 touchDown;
    private Vector3 touchUp;

    public enum InputMode { Mobile, Pc }
    public InputMode inputMode;

    #endregion


    private void Update()
    {        
        if (inputMode == InputMode.Mobile)
        { 
            if (Input.touchCount > 0)
            {                
                touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchUp = touchDown = touch.position;
                        break;
                    case TouchPhase.Moved:
                        touchDown = touch.position;
                        break;                
                    case TouchPhase.Ended:
                        touchUp = touchDown = touch.position;
                        break;                    
                    default:
                        break;
                }

                Vector2 moveDirection = touchDown - touchUp;
                inputVector = new Vector3(moveDirection.x, 0, moveDirection.y);
            }
        }
        else 
            inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }
}

/**/