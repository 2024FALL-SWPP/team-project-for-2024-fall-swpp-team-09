using UnityEngine;

public class Anomaly20_InteractableFront : Anomaly20_Interactable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public float zMin;
    public float zMax;

    // 내부 수치
    private float _x;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20_InteractableFront";

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float z = objectPlayer.transform.position.z;

        if (z >= zMin && z <= zMax) {
            transform.position = new Vector3(_x, transform.position.y, z);
        } else {
            transform.position = new Vector3(_x + 100.0f, transform.position.y, z);
        }
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _x
        _x = objectBoard.transform.position.x - 0.01f;
        Log("Initialize `_x` success");

        return res;
    }
}
