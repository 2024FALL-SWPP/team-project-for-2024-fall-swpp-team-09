using UnityEngine;

public class Anomaly20_Interactable : SCH_AnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string namePlayer;
    public string nameBoard;

    // 오브젝트
    protected GameObject objectPlayer;
    protected GameObject objectBoard;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20_Interactable";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // objectPlayer
        objectPlayer = GameObject.Find(namePlayer);
        if (objectPlayer != null) {
            Log("Initialize `objectPlayer` success");
        } else {
            Log("Initialize `objectPlayer` failed", mode: 1);
            res = false;
        }

        // objectBoard
        objectBoard = GameObject.Find(nameBoard);
        if (objectBoard != null) {
            Log("Initialize `objectBoard` success");
        } else {
            Log("Initialize `objectBoard` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Destroy(gameObject);
        Log("Destroy game object success");

        return res;
    }
}
