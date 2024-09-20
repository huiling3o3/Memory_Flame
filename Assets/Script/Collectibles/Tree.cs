using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tree : MonoBehaviour // IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static Tree instance;
    public bool interactable;
    public GameObject stickPrefab;
    public Vector3 stickSpawnOffset;
    public int health;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        interactable = false;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("Hello");
    //}
    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    //Debug.Log("Hello");
    //}
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    //Debug.Log("Hello");
    //}
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    //Debug.Log("Hello");
    //}
    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //Debug.Log("Hello");
    //}

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Vector3 spawnPosition = transform.position + stickSpawnOffset;
            Instantiate(stickPrefab, spawnPosition, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && interactable == true)
        {
            TakeDamage();
        }
    }
}
