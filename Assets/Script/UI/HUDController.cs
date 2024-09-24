using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{

    //[Header("Wave Txt")]
    //[SerializeField] GameObject wavePanel;
    //[SerializeField] private TextMeshProUGUI waveStatsTxt, enemiesTxt;

    [Header("Player's Torch Ammo")]
    //reference to the UI variables
    [SerializeField] private Image fillImg;
    [SerializeField] private TextMeshProUGUI ammoTxt;

    public void Awake()
    {
        Game.SetHUDController(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Subscribe to the currentAmmoChanged event from PlayerShoot
        PlayerShoot.currentAmmoChanged += UpdateUIAmmo;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when this object is disabled or destroyed
        PlayerShoot.currentAmmoChanged -= UpdateUIAmmo;
    }

    public void UpdateUIAmmo(float currentAmmo)
    {
        fillImg.fillAmount = currentAmmo / 100;
        //Debug.Log(currentAmmo);
        ammoTxt.text = ((int)currentAmmo).ToString() + "%";
    }
}
