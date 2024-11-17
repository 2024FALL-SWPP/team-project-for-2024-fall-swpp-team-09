using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly19Manager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly19Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string nameGameManager;
    public string nameSlide;

    // 게임 매니저
    private GameManager _manager;

    // 슬라이드 스크립트
    private Anomaly19_Slide _scriptSlide;

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

    // 상호작용 성공 시 처리 함수
    //
    // 반환 값
    // - true: 처리 성공
    // - false: 처리 실패
    public void InteractionSuccess()
    {
        _manager.SetStageClear();

        if (!ResetAnomaly()) {
            return;
        }
    }

    // Private fields를 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
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

        // `_scriptSlide` 초기화
        _scriptSlide = GameObject.Find(nameSlide).GetComponent<Anomaly19_Slide>();
        if (_scriptSlide != null) {
            Debug.Log($"[{NAME}] Find `_scriptSlide` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_scriptSlide`.");
            res = false;
        }

        return res;
    }

    // 이상현상을 시작하는 함수
    //
    // 반환 값
    // - true: 시작 성공
    // - false: 시작 실패
    private bool SetAnomaly()
    {
        _scriptSlide.enabled = true;
        _scriptSlide.Manager = this;
        Debug.Log($"[{NAME}] Anomaly is set.");

        return true;
    }

    // 이상현상을 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    private bool ResetAnomaly()
    {
        return true;
    }
}
