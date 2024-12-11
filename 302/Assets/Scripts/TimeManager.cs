using UnityEngine;

public class TimeManager : AbstractBehaviour, IStageObserver
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameClock;
    public string nameLaptop;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "TimeManager";

    // 클래스 인스턴스
    public static TimeManager Instance { get; private set; }

    /**********************************
     * implementation: IStageObserver *
     **********************************/

    // 단계 변경 시 불리는 메서드
    public bool UpdateStage()
    {
        int stage = GameManager.Instance.GetCurrentStage();
        ClockController clock = FindClock();
        LaptopScreenController laptop = FindLaptop();
        bool res = true;

        if (clock != null) {
            Log("Find `ClockController` success");
            clock.SetTime(stage);
        } else {
            Log("Find `ClockController` failed", mode: 1);
            res = false;
        }

        if (laptop != null) {
            Log("Find `LaptopScreenController` success");
            laptop.ChangeScreen(stage);
        } else {
            Log("Find `LaptopScreenController` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = false;

        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            res = base.Awake_();
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 시계를 찾는 메서드
    private ClockController FindClock()
    {
        GameObject obj = GameObject.Find(nameClock);
        ClockController clock;

        if (obj != null) {
            clock = obj.GetComponent<ClockController>();
        } else {
            clock = null;
        }

        return clock;
    }

    // 노트북을 찾는 메서드
    private LaptopScreenController FindLaptop()
    {
        GameObject obj = GameObject.Find(nameLaptop);
        LaptopScreenController laptop;

        if (obj != null) {
            laptop = obj.GetComponent<LaptopScreenController>();
        } else {
            laptop = null;
        }

        return laptop;
    }
}
