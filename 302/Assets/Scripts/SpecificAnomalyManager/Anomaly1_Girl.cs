using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly1_Girl : InteractableObject
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly1_Girl";

    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameCamera;

    // 오브젝트
    private GameObject _objectCamera;

    // 상호작용 전인지 여부
    private bool _beforeInteraction;

    /**************
     * properties *
     **************/

    public Anomaly1Manager Manager { get; set; }

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
            Debug.Log($"[{NAME}] Call `Manager.InteractionSuccess`");
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

    // Private fields를 초기화하는 메서드
    private bool InitFields()
    {
        bool res = true;

        // `_objectCamera` 초기화
        _objectCamera = GameObject.Find(nameCamera);
        if (_objectCamera != null) {
            Debug.Log($"[{NAME}] Find `_objectCamera` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectCamera`.");
            res = false;
        }

        return res;
    }

    // `canInteract`를 갱신하는 메서드
    private void UpdateCanInteract()
    {
        float distance = (_objectCamera.transform.position - transform.position).magnitude;

        canInteract = distance <= interactionRange;
    }

    // 이상현상을 초기화하는 메서드
    private bool ResetAnomaly()
    {
        Animator animator = GetComponent<Animator>();
        bool res = true;

        if (animator != null) {
            Debug.Log($"[{NAME}] Find `animator` successfully.");
            animator.SetBool("Sitting_b", true);
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `animator`.");
            res = false;
        }

        return res;
    }
}
