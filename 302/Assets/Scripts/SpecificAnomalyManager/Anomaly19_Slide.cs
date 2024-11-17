using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly19_Slide : InteractableObject
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly19_Slide";

    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameCamera;

    // 기준 거리
    public float thresholdDistance;

    // 오브젝트
    private GameObject _objectCamera;

    // 슬라이드 컨트롤러
    private SlideController _controller;

    // 흐르기 시작했는지 여부
    private bool _isTrickling;

    /**************
     * properties *
     **************/

    public Anomaly19Manager Manager { get; set; }

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

        gameObject.layer = 3;
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (!_isTrickling && DistanceToCamera() <= thresholdDistance) {
            _controller.StartTrickling();
            _isTrickling = true;
        }
    }

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        if (canInteract) {
            canInteract = false;

            base.OnInteract();

            if (Manager != null) {
                Debug.Log($"[{NAME}] Call `Anomaly19Manager.InteractionSuccess`");

                Manager.InteractionSuccess();
                gameObject.layer = 0;
                _controller.ResetSlide();
                enabled = false;
            } else {
                Debug.LogWarning($"[{NAME}] `Manager` is not set.");
            }
        }
    }

    /***************
     * new methods *
     ***************/

    // Private fields를 초기화하는 메서드
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
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

        // `_controller` 초기화
        _controller = gameObject.GetComponent<SlideController>();

        if (_controller != null) {
            Debug.Log($"[{NAME}] Find `_controller` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_controller`.");
            res = false;
        }

        // `_isTrickling` 초기화
        _isTrickling = false;
        Debug.Log($"[{NAME}] Initialize `_isTrickling` successfully.");

        return res;
    }

    // 카메라와의 거리를 구하는 메서드
    //
    // 반환값
    // - 카메라 `_objectCamera`와의 거리
    private float DistanceToCamera()
    {
        return (_objectCamera.transform.position - transform.position).magnitude;
    }
}
