using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject fireTorch;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    private Vector2 worldPos;
    private Vector2 dir;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTorchRotation();
        DoShoot();
    }

    private void HandleTorchRotation()
    {
        //rotate the torch towards the mouse pos
        worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        dir = (worldPos - (Vector2)fireTorch.transform.position).normalized;
        fireTorch.transform.right = dir;

        //Flip torch when it reach a 90 degree threshold
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector3 localscale = new Vector3(1f, 1f, 1f);
        //Vector3 localScale = firetorch.transform.localScale;
        if (angle > 90 || angle < -90)
        {
            localscale.y = -1f;
        }
        else 
        {
           localscale.y = 1f;
        }
        fireTorch.transform.localScale = localscale;
    }

    private void DoShoot()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //spawn bullet
            GameObject SpawnBullet = Instantiate(bullet, bulletSpawnPoint.position, fireTorch.transform.rotation);
        }
    }
}
