using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly23Manager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly23Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string nameGameManager;
    public string nameLaptop;

    // 프리팹
    public GameObject prefabGhost;

    // 게임 메니저
    private GameManager _manager;

    // 노트북 스크립트
    private LaptopScreenController _scriptLaptop;

    // 노트북 화면 색인
    private int _indexLaptop;

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

        // `_scriptLaptop` 초기화
        _scriptLaptop = GameObject.Find(nameLaptop).GetComponent<LaptopScreenController>();
        if (_scriptLaptop != null) {
            Debug.Log($"[{NAME}] Find `_scriptLaptop` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_scriptLaptop`.");
            res = false;
        }

        return res;
    }

    // 이상현상을 시작하는 메서드
    private bool SetAnomaly()
    {
        bool res = true;

        // 노트북 화면
        if (_scriptLaptop != null) {
            _indexLaptop = _scriptLaptop.Index;
            _scriptLaptop.ChangeScreen(12);
        } else {
            res = false;
        }

        // 유령
        if (prefabGhost != null) {
            Debug.Log($"[{NAME}] Find `prefabGhost` successfully.");
            Instantiate(prefabGhost).GetComponent<Anomaly23_Ghost>().Manager = this;
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabGhost`.");
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
        _scriptLaptop.ChangeScreen(_indexLaptop);

        return true;
    }
}
