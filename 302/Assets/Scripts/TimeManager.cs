using UnityEngine;

public class TimeManager : AbstractBehaviour, IStageObserver
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameClock;
    public string nameLaptop;

    // 컨트롤러
    private ClockController _clock;
    private LaptopScreenController _laptop;

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
        ClockController clock = FindClock();
        bool res = true;

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
}
