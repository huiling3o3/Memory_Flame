
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShoot : MonoBehaviour, IInteractReciever
{
    //reference to the fire torch object to manipulate 
    [Header("Fire Torch Settings")]
    [SerializeField] private GameObject fireTorch;

    //reference to the bullet objects to spawn the bullets
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    // Fire Ammo System
    [Header("Fire Ammo System")]
    [SerializeField] private float maxAmmo = 100f; // Maximum fire ammo
    [SerializeField] private float currentAmmo; 
    [SerializeField] private float ammoDepletionRate = 10f; // Ammo depletes per second when shooting, decrease it to so slower

    //reference to the player movement to calculate the shooting direction
    PlayerMovement pm;
    PlayerController pc;

    // Event that notifies subscribers when the current ammo changes
    public static event Action<float> currentAmmoChanged;
    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        pc = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ammo
        currentAmmo = maxAmmo;
    }
    //Rest function
    public void Reset()
    {
        // Initialize ammo
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.GetGameController().gameIsActive && !Game.GetGameController().gameOver)
        {
            HandleAim();
            DepleteAmmo();
        }
        
    } 

    private void HandleAim()
    {
        //Receive input for mouse direction from world space and convert it to 2d
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.nearClipPlane;

        //Calculate direction from player to mouse
        Vector3 aimDir = (mousePos - transform.position).normalized;

        if (pm.isPlayerFacingRight())
        {
            fireTorch.transform.right = new Vector3(Mathf.Clamp(aimDir.x, 0.45f, 0.8f), aimDir.y, 0);
            fireTorch.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {            
            fireTorch.transform.right = new Vector3(Mathf.Clamp(aimDir.x, -0.45f, -0.8f), aimDir.y, 0);
            // When facing left, flip the torch by adjusting the local scale on the X-axis
            fireTorch.transform.localScale = new Vector3(-1, 1, 1);
        }

        //Debug.Log(aimDir + ": " + fireTorch.transform.right);
    }

    private void DepleteAmmo()
    {
        if(!pc.IsPlayerInSafeZone())
        {
            // Deplete ammo over time
            currentAmmo -= ammoDepletionRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure ammo doesn't go below 0
        }

        // Trigger the event with the updated ammo percentage
        currentAmmoChanged?.Invoke(currentAmmo);
    }

    public void RegenerateAmmo()
    {
        //Call this method when player is back into campfire 
        currentAmmo = maxAmmo;
    }

    #region interact handling
    public void DoShoot()
    {
        //Debug.Log("PUPU");

        //there is enough ammo
        if (currentAmmo > 0 && !Game.GetGameController().gameOver)
        {
            // Calculate the correct bullet rotation
            Quaternion bulletRotation = fireTorch.transform.rotation;
            //spawn bullet
            GameObject SpawnBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletRotation);
            
            Vector2 shootDirection;

            if (pm.isPlayerFacingRight())
            {
                shootDirection = SpawnBullet.transform.right;
            }
            else
            {
                shootDirection = -SpawnBullet.transform.right;
            }
            SpawnBullet.GetComponent<Rigidbody2D>().velocity = shootDirection;         
        }
    }

    public void StartInteract()
    {
        //do nothing
    }
    public void StopInteract()
    { 
        //do nothing
    }
    #endregion
}
