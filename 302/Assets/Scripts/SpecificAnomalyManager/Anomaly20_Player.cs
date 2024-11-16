using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly20_Player : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private string NAME = "Anomaly20_Player";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string namePlayer;

    // 플레이어 오브젝트
    private GameObject _objectPlayer;

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
        Transform playerTransform = _objectPlayer.transform;

        transform.position = playerTransform.position;
        transform.rotation = Quaternion.Euler(0.0f, playerTransform.rotation.eulerAngles.y, 0.0f);
    }

    /***************
     * new methods *
     ***************/

    // 지속시간 동안 투명해지다가 사라지는 함수
    public IEnumerator FadeAsync(float duration)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Color color;
        float timeStart = Time.time;
        float time, alpha;

        yield return null;

        while ((time = Time.time - timeStart) < duration) {
            alpha = 1.0f - time / duration;

            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    color = material.color;
                    color.a = alpha;
                    material.color = color;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }

    // Private fields를 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    private bool InitFields()
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

        return res;
    }
}
