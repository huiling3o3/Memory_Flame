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

    [Header("Fire Timebar")]
    public Slider TimerBar;
    [Header("[Gradient to adjust color val]")]
    public Gradient gradient;
    [SerializeField] Image fill;

    public void Awake()
    {
        Game.SetHUDController(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        fill.color = gradient.Evaluate(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        // Subscribe to the currentAmmoChanged event from PlayerShoot
        PlayerShoot.currentAmmoChanged += UpdateUIAmmo;
        // Subscribe to the fireHealthChanged event from CampFireController
        CampFireController.fireHealthChanged += UpdateFireBar;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when this object is disabled or destroyed
        PlayerShoot.currentAmmoChanged -= UpdateUIAmmo;
        // Unsubscribe to the fireHealthChanged event from CampFireController
        CampFireController.fireHealthChanged += UpdateFireBar;
    }

    public void UpdateUIAmmo(float currentAmmo)
    {
        fillImg.fillAmount = currentAmmo / 100;
        //Debug.Log(currentAmmo);
        ammoTxt.text = ((int)currentAmmo).ToString() + "%";
    }
    public void UpdateFireBar(float val)
    {
        TimerBar.value = val / 100;
        fill.color = gradient.Evaluate(TimerBar.normalizedValue);
    }
}
