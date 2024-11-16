using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly20_InteractableBack : Anomaly20_Interactable
{
    /**********
     * fields *
     **********/

    // `z` 좌표 범위
    public float zMin;
    public float zMax;

    // `x` 좌표
    private float _x;

    /**********************
     * overridden methods *
     **********************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float z = _objectPlayer.transform.position.z;

        if (z >= zMin && z <= zMax) {
            transform.position = new Vector3(_x, transform.position.y, z);
        } else {
            transform.position = new Vector3(_x - 100.0f, transform.position.y, z);
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

        _x = _objectBoard.transform.position.x + 0.01f;

        return res;
    }
}
