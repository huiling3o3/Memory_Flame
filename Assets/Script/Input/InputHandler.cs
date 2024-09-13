using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IInputReceiver activeReceiver;
    private Vector2 moveDir;
    public void SetInputReceiver(IInputReceiver inputReceiver)
    {
        //set current input receiver (to control 1 thing at a time)
        activeReceiver = inputReceiver;
    }

    void FixedUpdate()
    {
        if (activeReceiver == null) return;
        
        float horiInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        moveDir = new Vector2(horiInput, vertInput);
        activeReceiver.DoMoveDir(moveDir);       
    }

    // Update is called once per frame
    void Update()
    {
        if (activeReceiver == null) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            activeReceiver.DoLeftAction();  
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            activeReceiver.DoRightAction();
        }
        if (Input.GetButtonDown("Submit"))
        {
            activeReceiver.DoSubmitAction();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            activeReceiver.DoCancelAction();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            activeReceiver.DoDash();
        }
    }
}