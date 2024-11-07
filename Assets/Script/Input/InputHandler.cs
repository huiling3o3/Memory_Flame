using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IInputReceiver activeReceiver;
    private Vector2 moveDir;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
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

        if (Input.GetButtonDown("Submit"))
        {
            //TODO: play click btn sound
            SoundManager.PlaySound(SoundType.SUBMIT, null, 0.6f);
            activeReceiver.DoSubmitAction();           
        }
        if (Input.GetButtonDown("Cancel"))
        {
            SoundManager.PlaySound(SoundType.CANCEL, null, 0.6f);
            activeReceiver.DoCancelAction();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            activeReceiver.DoDash();
        }
    }
}