using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    public int zoomLevel;
    [SerializeField] Transform LLB;
    [SerializeField] Transform URB;
    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, LLB.position.x, URB.position.x), Mathf.Clamp(targetPosition.y, LLB.position.y, URB.position.y), zoomLevel);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
