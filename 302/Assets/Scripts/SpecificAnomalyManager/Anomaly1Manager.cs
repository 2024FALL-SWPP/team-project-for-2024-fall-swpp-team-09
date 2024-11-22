using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly1Manager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly1Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string nameGameManager;
    public string nameChair;

    // 프리팹
    public GameObject prefabGirl;

    // 수치
    public Vector3 positionChair;

    // 게임 매니저
    private GameManager _manager;

    // 오브젝트
    private GameObject _objectChair;

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

        if (!SetAnomaly()) {
            return;
        }
    }

    /***************
     * new methods *
     ***************/

    // 상호작용 성공 시 처리 메서드
    public void InteractionSuccess()
    {
        _manager.SetStageClear();

        if (!ResetAnomaly()) {
            return;
        }
    }

    // Private fields를 초기화하는 메서드
    private bool InitFields()
    {
        bool res = true;

        // `_manager` 초기화
        _manager = GameObject.Find(nameGameManager).GetComponent<GameManager>();
        if (_manager != null) {
            Debug.Log($"[{NAME}] Find `_manager` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_manager`.");
            res = false;
        }

        // `_objectChair` 초기화
        _objectChair = GameObject.Find(nameChair);
        if (_objectChair != null) {
            Debug.Log($"[{NAME}] Find `_objectChair` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectChair`.");
            res = false;
        }

        return res;
    }

    // 이상현상을 시작하는 메서드
    private bool SetAnomaly()
    {
        bool res = true;

        // 의자
        if (_objectChair != null) {
            _objectChair.transform.position = positionChair;
        } else {
            res = false;
        }

        // 여학생
        if (prefabGirl != null) {
            Debug.Log($"[{NAME}] Find `prefabGirl` successfully.");
            Instantiate(prefabGirl).GetComponent<Anomaly1_Girl>().Manager = this;
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabGirl`.");
            res = false;
        }

        if (res) {
            Debug.Log($"[{NAME}] Anomaly is set.");
        }

        return res;
    }

    // 이상현상을 초기화하는 메서드
    private bool ResetAnomaly()
    {
        return true;
    }
}
