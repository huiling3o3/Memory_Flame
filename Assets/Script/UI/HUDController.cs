using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class HUDController : MonoBehaviour
{
    [Header("Collectables")]
    //reference to the UI variables
    [SerializeField] private TextMeshProUGUI branchTxt;
    [SerializeField] private Image Shield;

    [Header("Player's Torch Ammo")]
    //reference to the UI variables
    [SerializeField] private Image fillImg;
    [SerializeField] private TextMeshProUGUI ammoTxt;

    [Header("Fire Timebar")]
    public Slider TimerBar;
    [Header("[Gradient to adjust color val]")]
    public Gradient gradient;
    [SerializeField] Image fill;

    [Header("Player Healthbar")]
    public Slider HealthBar;
    [Header("[Gradient to adjust color val]")]
    public Gradient gradientHealth;
    [SerializeField] Image fillHealth;

    [Header("Player Hypothermia Bar")]
    public Slider ColdBar;
    [Header("[Gradient to adjust color val]")]
    [SerializeField] private Gradient gradientCold;
    [SerializeField] private Image fillCold;

    [Header("Dialogue")]
    [SerializeField] private TextMeshProUGUI instructionsText;
    [SerializeField] private GameObject popUpObj;
    [SerializeField] private float minY, maxY;
    [SerializeField] private float animDuration, stopDuration;
    private bool isShowing;

    public void Awake()
    {
        Game.SetHUDController(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        fill.color = gradient.Evaluate(1f);
        fillHealth.color = gradientHealth.Evaluate(1f);
        fillCold.color = gradientCold.Evaluate(0f); // Start with no cold
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
        //CampFireController.fireHealthChanged += UpdateFireBar;
        // Subscribe to the branchCollectChanged event from Game controller
        GameController.branchCollectedChanged += UpdateBranchCount;
        // Subscibe to memFragmentsCollected event from Game controller
        GameController.memFragmentsCollected += UpdateMemoryFragUI;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when this object is disabled or destroyed
        PlayerShoot.currentAmmoChanged -= UpdateUIAmmo;
        // Unsubscribe to the fireHealthChanged event from CampFireController
        //CampFireController.fireHealthChanged -= UpdateFireBar;
        // Unsubscribe to the branchCollectChanged event from Game controller
        GameController.branchCollectedChanged -= UpdateBranchCount;
    }

    public void ShowInstructions(string txt)
    {
        if (!isShowing)
        {
            StartCoroutine(OpenInstructions(txt));
        }
    }
    private IEnumerator CloseInstructions()
    {
        // Animate the popup sliding from minY to maxY
        float elapsedTime = 0f;
        Vector2 startPosition = popUpObj.GetComponent<RectTransform>().anchoredPosition;

        // Animate the popup sliding back from maxY to minY
        elapsedTime = 0f;
        while (elapsedTime < animDuration)
        {
            float newY = Mathf.SmoothStep(maxY, minY, elapsedTime / animDuration);
            popUpObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x, newY);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the popup is exactly at minY
        popUpObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x, minY);
        yield return null;

        //set showing flag to false
        isShowing = false;
    }
    private IEnumerator OpenInstructions(string txt)
    {
        //set showing flag to true
        isShowing = true;

        //Change the text of the gameobject
        instructionsText.text = txt;

        // Animate the popup sliding from minY to maxY
        float elapsedTime = 0f;
        Vector2 startPosition = popUpObj.GetComponent<RectTransform>().anchoredPosition;

        while (elapsedTime < animDuration)
        {
            float newY = Mathf.SmoothStep(minY, maxY, elapsedTime / animDuration);
            popUpObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x, newY);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the popup is exactly at maxY
        popUpObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x, maxY);

        // Wait for stopDuration at maxY
        yield return new WaitForSeconds(stopDuration);       
    }

    public void UpdateMemoryFragUI(MemoryFragType fragType)
    {
        // Ensure the alpha value is between 0 (fully transparent) and 1 (fully opaque)
        float alphaValue = 1f;

        if (fragType == MemoryFragType.HEADBAND)
        {
            // Get the current color of the image
            Color currentColor = Shield.color;

            // Set the alpha value while keeping the other color values unchanged
            currentColor.a = alphaValue;

            // Apply the updated color back to the image
            Shield.color = currentColor;
        }
    }

    public void UpdateBranchCount(int amt)
    {
        branchTxt.text = "x " + amt.ToString();
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
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        HealthBar.value = currentHealth / maxHealth;
        fillHealth.color = gradientHealth.Evaluate(HealthBar.normalizedValue);
    }

    public void UpdateColdBar(float currentCold, float maxCold)
    {
        ColdBar.value = currentCold / maxCold;
        fillCold.color = gradientCold.Evaluate(ColdBar.normalizedValue);
    }
}
