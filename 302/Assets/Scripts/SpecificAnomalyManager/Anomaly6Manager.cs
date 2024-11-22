using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly6Manager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly6Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string nameGameManager;

    // 프리팹
    public GameObject prefabCake;

    // 게임 메니저
    private GameManager _manager;

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

        return res;
    }

    // 이상현상을 시작하는 메서드
    private bool SetAnomaly()
    {
        bool res = true;

        // 케이크
        if (prefabCake != null) {
            Instantiate(prefabCake).GetComponent<Anomaly6_Cake>().Manager = this;
            Debug.Log($"[{NAME}] Instantiate `prefabCake` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabCake`.");
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
