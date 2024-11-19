using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly2_Laptop : InteractableObject
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly2_Laptop";

    /**********
     * fields *
     **********/

    // 오브젝트
    public GameObject objectCamera;

    // 노트북 스크립트
    private LaptopFaceController _scriptLaptop;

    // 상호작용 전인지 여부
    private bool _beforeInteraction;

    /**************
     * properties *
     **************/

    public Anomaly2Manager Manager { get; set; }

    /**********************
     * overridden methods *
     **********************/

    // Start is called on the frame when a script is enabled just
    // before any of the Update methods are called the first time.
    void Start()
    {
        if (!InitFields()) {
            return;
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (_beforeInteraction) {
            UpdateCanInteract();
        }
    }

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        if (Manager != null) {
            Debug.Log($"[{NAME}] Call `Anomaly2Manager.InteractionSuccess`");
            Manager.InteractionSuccess();

            ResetAnomaly();
            _beforeInteraction = false;
            canInteract = false;
        } else {
            Debug.LogWarning($"[{NAME}] `Manager` is not set.");
        }
    }

    /***************
     * new methods *
     ***************/

    // 이상현상을 시작하는 메서드
    public bool SetAnomaly()
    {
        bool res = true;

        // 노트북 화면
        if (_scriptLaptop != null) {
            _scriptLaptop.StartGazing();
        } else {
            res = false;
        }

        gameObject.layer = 3;  // 상호작용 레이어

        return res;
    }

    // Private fields를 초기화하는 메서드
    private bool InitFields()
    {
        bool res = true;

        // `_scriptLaptop` 초기화
        _scriptLaptop = gameObject.GetComponent<LaptopFaceController>();
        if (_scriptLaptop != null) {
            Debug.Log($"[{NAME}] Find `_scriptLaptop` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_scriptLaptop`.");
            res = false;
        }

        return res;
    }

    // `canInteract`를 갱신하는 메서드
    private void UpdateCanInteract()
    {
        float distance = (objectCamera.transform.position - transform.position).magnitude;

        canInteract = distance <= interactionRange;
    }

    // 이상현상을 초기화하는 메서드
    private bool ResetAnomaly()
    {
        bool res = true;

        if (_scriptLaptop != null) {
            _scriptLaptop.ResetScreen();
        } else {
            res = false;
        }

        gameObject.layer = 0;  // 일반 레이어

        return res;
    }
}
