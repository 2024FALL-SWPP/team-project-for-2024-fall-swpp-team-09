using UnityEngine;

public class Anomaly20_InteractableRight : Anomaly20_Interactable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public float x1Min;
    public float x1Max;
    public float x2Min;
    public float x2Max;

    // 내부 수치
    private float _z;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20_InteractableRight";

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float x = objectPlayer.transform.position.x;

        if (x >= x1Min && x <= x1Max || x >= x2Min && x <= x2Max) {
            transform.position = new Vector3(x, transform.position.y, _z);
        } else {
            transform.position = new Vector3(x, transform.position.y, _z - 100.0f);
        }
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _z
        _z = objectBoard.transform.position.z + 0.01f;
        Log("Initialize `_z`: success");

        return res;
    }
}
