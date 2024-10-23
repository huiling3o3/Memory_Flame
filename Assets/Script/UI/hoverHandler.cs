using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class hoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Animator buttonAnimator;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component attached to this GameObject
        button = GetComponent<Button>();
        // Get the Animator component attached to the button
        buttonAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called when the pointer enters the button's area
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Button is highlighted!");
        // Trigger the hover animation state
        if (buttonAnimator != null)
        {
            buttonAnimator.SetBool("hovered", true);
        }
    }

    // This method is called when the pointer exits the button's area
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Button is no longer highlighted.");
        if (buttonAnimator != null)
        {
            buttonAnimator.SetBool("hovered", false);
        }
    }
}
