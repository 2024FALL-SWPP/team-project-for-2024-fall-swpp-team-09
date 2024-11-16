using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly20_InteractableRight : Anomaly20_Interactable
{
    /**********
     * fields *
     **********/

    // `x` 좌표 범위
    public float x1Min;
    public float x1Max;
    public float x2Min;
    public float x2Max;

    // `z` 좌표
    private float _z;

    /**********************
     * overridden methods *
     **********************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float x = _objectPlayer.transform.position.x;

        if (x >= x1Min && x <= x1Max || x >= x2Min && x <= x2Max) {
            transform.position = new Vector3(x, transform.position.y, _z);
        } else {
            transform.position = new Vector3(x, transform.position.y, _z - 100.0f);
        }
    }

    // Protected fields를 초기화하는 함수
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        _z = _objectBoard.transform.position.z + 0.01f;

        return res;
    }
}
