
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    //reference to the fire torch object to manipulate 
    [Header("Fire Torch Settings")]
    [SerializeField] private GameObject fireTorch;

    //reference to the bullet objects to spawn the bullets
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;

    //Mouse pos variables 
    private Vector2 worldPos;
    private Vector2 dir;

    // Minimum and maximum angle constraints    
    private float angle;
    [Header("Fire Torch Angle Constraints")]
    [SerializeField] private float minAngle = -45f;
    [SerializeField] private float maxAngle = 45f;

    // Fire Ammo System
    [Header("Fire Ammo System")]
    [SerializeField] private float maxAmmo = 100f; // Maximum fire ammo
    [SerializeField] private float currentAmmo;
    [SerializeField] private float ammoDepletionRate = 10f; // Ammo depletes per second when shooting, decrease it to so slower
    //[SerializeField] private bool inSafeZone; //layers that does not affect the ammoDepletion

    //reference to the player movement to calculate the shooting direction
    PlayerMovement pm;
    PlayerController pc;
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

    // Update is called once per frame
    void Update()
    {
        HandleTorchRotation();
        DoShoot();
        DepleteAmmo();
    }

    private void HandleTorchRotation()
    {
        //rotate the torch towards the mouse pos
        worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        dir = (worldPos - (Vector2)fireTorch.transform.position).normalized;
        fireTorch.transform.right = dir;

        //Calculate the torch angle
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Clamp the angle within the min and max limits
        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        // Apply the constrained angle to the torch rotation
        fireTorch.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void DoShoot()
    {
        // Only shoot if the E button is pressed and there is enough ammo
        if (Input.GetKeyDown(KeyCode.E) && currentAmmo > 0)
        {
            // Calculate the correct bullet rotation
            Quaternion bulletRotation = fireTorch.transform.rotation;

            // If the player is flipped, adjust the bullet's rotation by 180 degrees
            if (pm.isPlayerFacingRight())
            {
                bulletRotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                bulletRotation = Quaternion.Euler(0f, 0f, angle + 180f);
            }

            //spawn bullet
            GameObject SpawnBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletRotation);          
        }
    }

    private void DepleteAmmo()
    {
        if(!pc.IsPlayerInSafeZone())
        {
            // Deplete ammo over time
            currentAmmo -= ammoDepletionRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure ammo doesn't go below 0
        }           
    }


    public void RegenerateAmmo()
    {
        //Call this method when player is back into campfire 
        currentAmmo = maxAmmo;
    }

    // function to check the current ammo for UI purposes
    public float GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
