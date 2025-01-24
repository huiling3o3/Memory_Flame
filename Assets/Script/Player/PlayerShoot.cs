
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerShoot : MonoBehaviour, IInteractReciever
{
    //reference to the fire torch object to manipulate 
    [Header("Fire Torch Settings")]
    [SerializeField] private GameObject fireTorch;
    [SerializeField] private Sprite burnOutTorch, burningTorch; //to switch the torch state when the ammo is depleted
    private SpriteRenderer torchSprite;
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
    bool haveAmmo;

    void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        pc = GetComponent<PlayerController>();
        torchSprite = fireTorch.GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ammo
        Reset();
    }

    //Rest function
    public void Reset()
    {
        // Initialize ammo
        currentAmmo = maxAmmo;
        haveAmmo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Game.GetGameController().isPaused && !Game.GetGameController().isGameOver && Game.GetGameController().HaveFireTorch())
        {
            HandleAim();
            DepleteAmmo();
            UpdateApperance();
        }
        else
        {
            fireTorch.SetActive(false);
        }
    }

    private void UpdateApperance()
    {
        fireTorch.SetActive(true);
        if (currentAmmo <= 0)
        {            
            torchSprite.sprite = burnOutTorch;
        }
        else
        {
            torchSprite.sprite = burningTorch;
        }
    }

    private void HandleAim()
    {
        //Receive input for mouse direction from world space and convert it to 2d
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.nearClipPlane;

        //Calculate direction from player to mouse
        Vector2 aimDir = (mousePos - transform.position).normalized;

        //flip player sprite based on the the input mouse position
        pc.sr.flipX = Input.mousePosition.x < Screen.width/2;

        fireTorch.transform.right = aimDir;

        Debug.Log(aimDir + ": " + pc.sr.flipX);
    }

    private void DepleteAmmo()
    {
        if (!pc.IsPlayerInSafeZone() && haveAmmo)
        {
            // Deplete ammo over time
            currentAmmo -= ammoDepletionRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure ammo doesn't go below 0

            if (currentAmmo <= 0)
            {
                pc.IncreaseColdnessRate(); //increase coldness faster
                haveAmmo = false;
            }
        }

        // Trigger the event with the updated ammo percentage
        currentAmmoChanged?.Invoke(currentAmmo);
    }

    public void RegenerateAmmo()
    {
        //increase ammo over time
        currentAmmo += ammoDepletionRate * Time.deltaTime;
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure ammo doesn't go below 0
        //Call this method when player is back into campfire
        if (pc.IsPlayerInSafeZone())
        {
            //increase ammo over time
            currentAmmo += ammoDepletionRate * Time.deltaTime;
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo); // Ensure ammo doesn't go below 0
        }          
        //currentAmmo = maxAmmo;
    }

    #region interact handling
    public void StartInteract()
    {
        //Debug.Log("PUPU");

        //there is enough ammo
        if (currentAmmo > 0 && !Game.GetGameController().isGameOver && !Game.GetGameController().isPaused)
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

            SpawnBullet.GetComponent<BulletBehaviour>().Init(shootDirection);
        }
    }

    public void HoldInteract()
    {
        //do nothing
    }
    public void StopInteract()
    { 
        //do nothing
    }
    #endregion
}
