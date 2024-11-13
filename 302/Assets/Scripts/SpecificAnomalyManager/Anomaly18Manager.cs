using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly18Manager : MonoBehaviour
{
    /*************
     * Constants *
     *************/

    private string NAME = "Anomaly18Manager";

    /**********
     * fields *
     **********/

    // 게임매니저, 벽, 시계 오브젝트의 이름
    public string nameGameManager;
    public string nameWall;
    public string nameClock;

    // 벽, 시계 오브젝트의 위치
    public Vector3 positionWall;
    public Vector3 positionClock;

    // 벽, 시계 오브젝트의 이동 방향
    // (길이가 `1.0f`이어야 한다!)
    public Vector3 directionWall;
    public Vector3 directionClock;

    // 오브젝트 이동 위치
    public float positionNormal;
    public float positionAnomaly;

    // 책상, 의자 프리팹
    public GameObject prefabDesks;
    public GameObject prefabChairs;
    public GameObject[] prefabInteractables;

    // 초기화 소요 시간
    public float duration;

    // 게임 매니저
    private GameManager _manager;

    // 벽, 시계 오브젝트
    private GameObject _objectWall;
    private GameObject _objectClock;

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

        if (!SetWallLocation(positionAnomaly)) {
            return;
        }

        if (!InstantiatePrefabs()) {
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

        // `_objectWall` 초기화
        _objectWall = GameObject.Find(nameWall);
        if (_objectWall != null) {
            Debug.Log($"[{NAME}] Find `_objectWall` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectWall`.");
            res = false;
        }

        // `_objectClock` 초기화
        _objectClock = GameObject.Find(nameClock);
        if (_objectClock != null) {
            Debug.Log($"[{NAME}] Find `_objectClock` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectClock`.");
            res = false;
        }

        return res;
    }

    // 벽의 위치를 설정하는 함수
    //
    // 반환 값
    // - true: 설정 성공
    // - false: 설정 실패
    private bool SetWallLocation(float position)
    {
        _objectWall.transform.position = positionWall + directionWall * position;
        _objectClock.transform.position = positionClock + directionClock * position;

        return true;
    }

    // 프리팹을 생성하는 함수
    //
    // 반환 값
    // - true: 생성 성공
    // - false: 생성 실패
    private bool InstantiatePrefabs()
    {
        bool res = true;

        if (prefabDesks != null) {
            Instantiate(prefabDesks);
            Debug.Log($"[{NAME}] Instantiate `prefabDesks` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabDesks`.");
            res = false;
        }

        foreach (GameObject prefab in prefabInteractables) {
            if (prefab != null) {
                Instantiate(prefab);
                prefab.GetComponent<Anomaly18_Interactable>().Manager = this;
                Debug.Log($"[{NAME}] Instantiate a prefab in `prefabInteractable` successfully.");
            } else {
                Debug.LogWarning($"[{NAME}] Cannot find prefab in `prefabInteractables`.");
                res = false;
            }
        }

        if (prefabChairs != null) {
            Instantiate(prefabChairs);
            Debug.Log($"[{NAME}] Instantiate `prefabChairs` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabChairs`.");
            res = false;
        }

        return res;
    }

    // 이상현상을 초기화하는 함수
    //
    // 반환 값
    // - ture: 초기화 성공
    // - false: 초기화 실패
    private bool ResetAnomaly()
    {
        Anomaly18_Object scriptWall, scriptClock;
        Anomaly18_Prefab scriptDesks, scriptInteractable, scriptChairs;
        bool res = true;

        // `_objectWall` 초기화
        scriptWall = _objectWall.GetComponent<Anomaly18_Object>();
        if (scriptWall != null) {
            Debug.Log($"[{NAME}] Find `Anomaly18_Object` in `_objectWall` successfully.");
            StartCoroutine(
                scriptWall.MoveAsync(
                    positionWall + directionWall * positionAnomaly,
                    directionWall * (positionNormal - positionAnomaly),
                    duration
                )
            );
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `Anomaly18_Object` in `_objectWall`.");
            res = false;
        }

        // `_objectClock` 초기화
        scriptClock = _objectClock.GetComponent<Anomaly18_Object>();
        if (scriptClock != null) {
            Debug.Log($"[{NAME}] Find `Anomaly18_Object` in `_objectClock` successfully.");
            StartCoroutine(
                scriptClock.MoveAsync(
                    positionClock + directionClock * positionAnomaly,
                    directionClock * (positionNormal - positionAnomaly),
                    duration
                )
            );
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `Anomaly18_Object` in `_objectClock`.");
            res = false;
        }

        // `prefabDesks` 초기화
        if (prefabDesks != null) {
            scriptDesks = prefabDesks.GetComponent<Anomaly18_Prefab>();
            if (scriptDesks != null) {
                Debug.Log($"[{NAME}] Find `Anomaly18_Prefab` in `prefabDesks` successfully.");
                StartCoroutine(scriptDesks.FadeAsync(duration));
            } else {
                Debug.LogWarning($"[{NAME}] Cannot find `Anomaly18_Prefab` in `prefabDesks`.");
                res = false;
            }
        } else {
            res = false;
        }

        // `prefabInteractables` 초기화
        foreach (GameObject prefab in prefabInteractables) {
            if (prefab != null) {
                scriptInteractable = prefab.GetComponent<Anomaly18_Prefab>();
                if (scriptInteractable != null) {
                    Debug.Log($"[{NAME}] Find `Anomaly18_Prefab` in `prefabInteractable` successfully.");
                    StartCoroutine(scriptInteractable.FadeAsync(duration));
                } else {
                    Debug.LogWarning($"[{NAME}] Cannot find `Anomaly18_Prefab` in `prefabInteractable`.");
                    res = false;
                }
            }
        }

        // `prefabChairs` 초기화
        if (prefabChairs != null) {
            scriptChairs = prefabChairs.GetComponent<Anomaly18_Prefab>();
            if (scriptChairs != null) {
                Debug.Log($"[{NAME}] Find `Anomaly18_Prefab` in `prefabChairs` successfully.");
                StartCoroutine(scriptChairs.FadeAsync(duration));
            } else {
                Debug.LogWarning($"[{NAME}] Cannot find `Anomaly18_Prefab` in `prefabChairs`.");
                res = false;
            }
        } else {
            res = false;
        }

        return res;
    }
}
