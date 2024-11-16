using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly20_Interactable : InteractableObject
{
    /*************
     * constants *
     *************/

    private string NAME = "Anomaly20_Interactable";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string namePlayer;
    public string nameBoard;

    // 오브젝트
    protected GameObject _objectPlayer;
    protected GameObject _objectBoard;

    /**************
     * properties *
     **************/

    // 해당 오브젝트를 생성한 이상현상 매니저
    public Anomaly20Manager Manager { get; set; }

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

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        if (canInteract) {
            canInteract = false;

            base.OnInteract();

            if (Manager != null) {
                Debug.Log($"[{NAME}] Call `Anomaly20Manager.InteractionSuccess`");
                Manager.InteractionSuccess();
            } else {
                Debug.LogWarning($"[{NAME}] `Manager` is not set.");
            }
        }
    }

    /***************
     * new methods *
     ***************/

    // Protected fields를 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    protected virtual bool InitFields()
    {
        bool res = true;

        // `_objectPlayer` 초기화
        _objectPlayer = GameObject.Find(namePlayer);
        if (_objectPlayer != null) {
            Debug.Log($"[{NAME}] Find `_objectPlayer` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectPlayer`.");
            res = false;
        }

        // `_objectBoard` 초기화
        _objectBoard = GameObject.Find(nameBoard);
        if (_objectBoard != null) {
            Debug.Log($"[{NAME}] Find `_objectBoard` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectBoard`.");
            res = false;
        }

        return res;
    }
}
