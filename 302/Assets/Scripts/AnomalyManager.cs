using UnityEngine;
using System.Collections.Generic;

public class AnomalyManager : AbstractStageObserver
{
    private HashSet<int> ANOMALIES_NOT_YET = new HashSet<int> { 4, 5, 7 };

    /**********
     * fields *
     **********/

    // 이상현상 컨트롤러 프리팹
    public GameObject[] prefabs;

    // 개수
    public int numStage;
    public int numAnomaly;

    // 테스트용 수치
    public int testMode;
    public int testAnomalyID;

    // 난수
    private SCH_Random _random;

    // 이상현상 색인 배열
    private int[] _anomalyList;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AnomalyManager";

    // 클래스 인스턴스
    public static AnomalyManager Instance { get; private set; }

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

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        _random = new SCH_Random();

        _anomalyList = new int[numStage];

        return res;
    }

    /*****************************************
     * implementation: AbstractStageObserver *
     *****************************************/

    // 단계 변경 시 불리는 메서드
    public override bool UpdateStage()
    {
        bool res = base.UpdateStage();

        if (GameManager.Instance.Stage == 1) {
            Log("Call `GenerateList` begin");
            if (GenerateList()) {
                Log("Call `GenerateList` sucess");
            } else {
                Log("Call `GenerateList` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 이상현상 컨트롤러를 생성 후 반환하는 메서드
    public AbstractAnomalyObject GetAnomalyController()
    {
        GameObject obj;
        AbstractAnomalyObject controller = null;
        int stage = GameManager.Instance.Stage;

        if (stage == 0) {
            obj = Instantiate(prefabs[0]);
            if (obj != null) {
                controller = obj.GetComponent<AbstractAnomalyObject>();
            } else {
                Log($"Find `AbstractAnomalyObject` for {obj.name} failed", mode: 2);
            }
        } else if (stage > 0 && stage <= numStage) {
            obj = Instantiate(prefabs[_anomalyList[stage - 1]]);
            if (obj != null) {
                controller = obj.GetComponent<AbstractAnomalyObject>();
            } else {
                Log($"Find `AbstractAnomalyObject` for {obj.name} failed", mode: 2);
            }
        } else {
            Log($"Invalid stage: {stage}", mode: 2);
        }

        return controller;
    }

    // 이상현상 색인 리스트를 생성하는 메서드
    private bool GenerateList()
    {
        bool res = true;

        switch (testMode) {
            case 0:
                GenerateAnomalyListNormal();
                break;
            case 1:
                GenerateAnomalyListTest1();
                break;
            case 2:
                GenerateAnomalyListTest2();
                break;
            default:
                Log("Generate `_anomalyList` failed", mode: 1);
                res = false;
                break;
        }

        return res;
    }

    // 일반 이상현상 색인 리스트를 생성하는 메서드
    private void GenerateAnomalyListNormal()
    {
        bool hasHighAnomaly = false;

        while (!hasHighAnomaly) {
            int idxLastZero = -2;

            _anomalyList = _random.Permutation(numAnomaly - 1, numStage);
            for (int idx = 0; idx < numStage; idx++) {
                if (idx - idxLastZero > 1 && _random.UniformDist(0.0, 1.0) < 0.2) {
                    // 직전 단계가 제0번이 아닌 경우 20 %의 확률로 제0번 생성
                    _anomalyList[idx] = 0;
                    idxLastZero = idx;
                } else {
                    // 0번을 위한 색인 조정
                    _anomalyList[idx] += 1;

                    if (ANOMALIES_NOT_YET.Contains(_anomalyList[idx])) {
                        _anomalyList[idx] = 0;
                    }

                    // 적대적 이상현상 포함 확인
                    if (_anomalyList[idx] > 20) {
                        hasHighAnomaly = true;
                    }
                }
            }
        }

        Log($"Generate `_anomalyList` success: [{string.Join(", ", _anomalyList)}]");
    }

    // 테스트용 이상현상 색인 리스트를 생성하는 메서드 1
    private void GenerateAnomalyListTest1()
    {
        for (int idx = 0; idx < numStage; idx++) {
            // 원하는 이상현상이 계속 나오도록
            _anomalyList[idx] = testAnomalyID;
        }

        Log($"Generate `_anomalyList` success: [{string.Join(", ", _anomalyList)}]");
    }

    // 테스트용 이상현상 색인 리스트를 생성하는 메서드 2
    private void GenerateAnomalyListTest2()
    {
        for (int idx = 0; idx < numStage; idx++) {
            // 원하는 이상현상과 제0번이 번갈아 나오도록
            if (idx % 2 == 0) {
                _anomalyList[idx] = testAnomalyID;
            } else {
                _anomalyList[idx] = 0;
            }
        }

        Log($"Generate `_anomalyList` success: [{string.Join(", ", _anomalyList)}]");
    }
}
