using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] Transform LLB;
    [SerializeField] Transform URB;

    private Camera cam; // Reference to the camera component

    private void Start()
    {
        cam = GetComponent<Camera>();
        // Set camera position based on the initial offset
        transform.position = target.position + offset;
    }

    private void LateUpdate() // Use LateUpdate to ensure the camera moves after the player
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return; // Exit if no target is assigned
        }

        // Calculate the target position for the camera
        Vector3 targetPosition = target.position + offset;

        // Clamp the camera's position to stay within bounds
        //targetPosition.x = Mathf.Clamp(targetPosition.x, LLB.position.x, URB.position.x);
        //targetPosition.y = Mathf.Clamp(targetPosition.y, LLB.position.y, URB.position.y);

        // Smoothly move the camera towards the target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
