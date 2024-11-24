using UnityEngine;

public class Anomaly18Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameWall;
    public string nameClock;

    // 프리팹
    public GameObject[] prefabs;

    // 오브젝트
    private SCH_AnomalyObject _objectWall;
    private SCH_AnomalyObject _objectClock;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly18Manager";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objectWall
        _objectWall = GameObject.Find(nameWall).GetComponent<SCH_AnomalyObject>();
        if (_objectWall != null) {
            _objects.Add(_objectWall);
            Log("Initialize `_objectWall`: success");
        } else {
            Log("Initialize `_objectWall`: failed", mode: 1);
            res = false;
        }

        // _objectClock
        _objectClock = GameObject.Find(nameClock).GetComponent<SCH_AnomalyObject>();
        if (_objectClock != null) {
            _objects.Add(_objectClock);
            Log("Initialize `_objectClock`: success");
        } else {
            Log("Initialize `_objectClock`: failed", mode: 1);
            res = false;
        }

        return res;
    }

    /**************************************
     * implementation: SCH_AnomalyManager *
     **************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        // 벽
        if (_objectWall) {
            _objectWall.enabled = true;
            _objectWall.Manager = this;
            Log("Set `_objectWall`: success");
        } else {
            Log("Set `_objectWall`: failed", mode: 1);
            res = false;
        }

        // 시계
        if (_objectClock) {
            _objectClock.enabled = true;
            _objectClock.Manager = this;
            Log("Set `_objectClock`: success");
        } else {
            Log("Set `_objectClock`: failed", mode: 1);
            res = false;
        }

        // 프리팹
        foreach (GameObject prefab in prefabs) {
            SCH_AnomalyObject obj = Instantiate(prefab).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            _objects.Add(obj);
            Log($"Set `{prefab.name}`: success");
        }

        return res;
    }
}
