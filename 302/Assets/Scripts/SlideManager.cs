using UnityEngine;

public class SlideManager : SCH_Behaviour
{
    /**********
     * fields *
     **********/

    // 슬라이드 오브젝트 이름
    public string nameLeft;
    public string nameRight;

    // 개수
    public int numSlide;
    public int numStage;

    // 무작위성
    private SCH_Random _random;

    // 오브젝트
    private GameObject _objectLeft;
    private GameObject _objectRight;

    // 슬라이드 컨트롤러
    private SlideController _controllerLeft;
    private SlideController _controllerRight;

    // 슬라이드 색인 배열
    private int[] _slideList;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SlideManager";

    // 클래스 인자
    public static SlideManager Instance { get; private set; }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return base.Awake_();
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _random
        _random = new SCH_Random();
        Log("Initialize `_random` success");

        _objects = new List<GameObject>();
        _controllers = new List<SlideController>();
        
        // _slideList
        _slideList = new int[numStage];
        Log("Initialize `_slideList` success");

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 슬라이드 색인 배열을 초기화하는 메서드
    public void GenerateSlideList()
    {
        _slideList = _random.Combination(numSlide, numStage);
        Log($"Generate `_slideList` success: [{string.Join(", ", _slideList)}]");
    }

    // 슬라이드를 초기화하는 메서드
    public void SetSlide(int stage)
    {
        Log("Call `FindSlides` begin");
        if (FindSlides()) {
            Log("Call `FindSlides` success");
            if (stage > 0) {
                int index = _slideList[stage - 1];

                _controllerLeft.Index = index;
                _controllerRight.Index = index;

                _controllerLeft.ResetSlide();
                _controllerRight.ResetSlide();

                Log($"Set slide success: {index}");
            } else {
                _objectLeft.transform.Translate(Vector3.down * 100.0f);
                _objectRight.transform.Translate(Vector3.down * 100.0f);
            }
        } else {
            Log("Call `FindSlides` failed", mode: 1);
        }
    }

    // 슬라이드를 찾는 메서드
    private bool FindSlides()
    {
        bool res = true;

        // `_objectLeft` 찾기
        _objectLeft = GameObject.Find(nameLeft);
        if (_objectLeft != null) {
            Log("Find `_objectLeft` success");

            // `_controllerLeft` 찾기
            _controllerLeft = _objectLeft.GetComponent<SlideController>();
            if (_controllerLeft != null) {
                Log("Find `_controllerLeft` success");
            } else {
                Log("Find `_controllerLeft` failed", mode: 1);
                res = false;
            }
        } else {
            Log("Find `_objectLeft` failed", mode: 1);
            res = false;
        }

        // `_objectRight` 찾기
        _objectRight = GameObject.Find(nameRight);
        if (_objectRight != null) {
            Log("Find `_objectRight` success");

            // `_controllerRight` 찾기
            _controllerRight = _objectRight.GetComponent<SlideController>();
            if (_controllerRight != null) {
                Log("Find `_controllerRight` success");
            } else {
                Log("Find `_controllerRight` failed", mode: 1);
                res = false;
            }
        } else {
            Log("Find `_objectRight` failed", mode: 1);
            res = false;
        }

        return res;
    }
}
