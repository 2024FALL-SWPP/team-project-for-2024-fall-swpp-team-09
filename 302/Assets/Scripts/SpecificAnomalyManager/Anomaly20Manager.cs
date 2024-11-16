using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly20Manager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private string NAME = "Anomaly20Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string nameGameManager;

    // 이상현상 플레이어, 상호작용 프리팹
    public GameObject prefabPlayer;
    public GameObject[] prefabsInteractable;

    // 초기화 소요 시간
    public float duration;

    // 게임 매니저
    private GameManager _manager;

    // 이상현상 플레이어, 상호작용 오브젝트
    private GameObject _objectPlayer;
    private List<GameObject> _objectsInteractable;

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

        // `_objectsInteractable` 초기화
        _objectsInteractable = new List<GameObject>();
        Debug.Log($"[{NAME}] Initialize `_objectsInteractable` successfully.");

        return res;
    }

    // 프리팹을 생성하는 함수
    //
    // 반환 값
    // - true: 생성 성공
    // - false: 생성 실패
    private bool InstantiatePrefabs()
    {
        GameObject obj;
        bool res = true;

        _objectsInteractable.Clear();

        if (prefabPlayer != null) {
            _objectPlayer = Instantiate(prefabPlayer);
            Debug.Log($"[{NAME}] Instantiate `prefabPlayer` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `prefabPlayer`.");
            res = false;
        }

        foreach (GameObject prefab in prefabsInteractable) {
            if (prefab != null) {
                obj = Instantiate(prefab);
                obj.GetComponent<Anomaly20_Interactable>().Manager = this;
                _objectsInteractable.Add(obj);
                Debug.Log($"[{NAME}] Instantiate a prefab in `prefabsInteractable` successfully.");
            } else {
                Debug.LogWarning($"[{NAME}] Cannot find prefab in `prefabsInteractable`.");
                res = false;
            }
        }

        return res;
    }

    // 이상현상을 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    private bool ResetAnomaly()
    {
        Anomaly20_Player scriptPlayer;
        bool res = true;

        // `_objectPlayer` 초기화
        scriptPlayer = _objectPlayer.GetComponent<Anomaly20_Player>();
        if (scriptPlayer != null) {
            Debug.Log($"[{NAME}] Find `Anomaly20_Player` in `_objectPlayer` successfully.");
            StartCoroutine(scriptPlayer.FadeAsync(duration));
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `Anomaly20_Player` in `_objectPlayer`.");
            res = false;
        }

        // `_objectsInteractable` 초기화
        foreach (GameObject obj in _objectsInteractable) {
            Destroy(obj);
            Debug.Log($"[{NAME}] Destroy an object in `_objectsInteractable` successfully.");
        }

        return res;
    }
}
