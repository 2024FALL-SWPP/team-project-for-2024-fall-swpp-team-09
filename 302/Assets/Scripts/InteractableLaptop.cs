using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableLaptop : InteractableObject
{
    private LaptopScreenController laptopController;
    private GameObject screenUI;
    private bool isScreenVisible = false;
    private Texture2D screenTexture;
    private bool canCloseScreen = false;  // 추가: 화면을 닫을 수 있는지 체크하는 변수

    [SerializeField] private RawImage screenImage;
    private readonly Vector2 SCREEN_RESOLUTION = new Vector2(1280, 1080);

    void Awake()
    {
        laptopController = transform.parent.parent.GetComponent<LaptopScreenController>();
        if (laptopController == null)
        {
            Debug.LogError("LaptopScreenController not found in grandparent object!");
            return;
        }

        screenTexture = laptopController.screen;
        if (screenTexture == null)
        {
            Debug.LogError("Screen texture is null!");
            return;
        }

        if (screenImage == null)
        {
            Debug.LogError("Screen Image reference is missing!");
            return;
        }

        screenUI = screenImage.gameObject;

        RectTransform rectTransform = screenImage.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = SCREEN_RESOLUTION;
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public override void OnInteract()
    {
        if (!isScreenVisible)
        {
            ShowScreen();
            canCloseScreen = false;  // 화면을 보여줄 때 닫기 불가능하도록 설정
            Invoke("EnableScreenClosing", 0.1f);  // 0.1초 후에 닫기 가능하도록 설정
        }
    }

    private void EnableScreenClosing()
    {
        canCloseScreen = true;
    }

    private void ShowScreen()
    {
        if (screenTexture != null && screenUI != null)
        {
            screenImage.texture = screenTexture;
            screenUI.SetActive(true);
            isScreenVisible = true;
        }
    }

    private void HideScreen()
    {
        screenUI.SetActive(false);
        isScreenVisible = false;
        canCloseScreen = false;
    }

    void Update()
    {
        if (isScreenVisible && canCloseScreen && Input.GetMouseButtonDown(0))
        {
            HideScreen();
        }
    }
}