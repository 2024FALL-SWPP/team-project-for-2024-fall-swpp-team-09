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

    // 클래스 인자
    public static SlideManager Instance { get; private set; }

    // 클래스 이름
    public override string Name { get; } = "SlideManager";

    /************
     * messages *
     ************/

    // Unity calls Awake when an enabled script instance is being loaded.
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!InitFields()) {
                return;
            }

            InitSlideList();
        } else {
            Destroy(gameObject);
        }
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _random
        _random = new SCH_Random();
        Log("Initialize `_random`: success");

        // 슬라이드 초기화
        Log("Call `FindSlides` begin");
        if (FindSlides()) {
            Log("Call `FindSlides` end: success");
        } else {
            Log("Call `FindSlides` end: failed", mode: 1);
            res = false;
        }

        // _slideList
        _slideList = new int[numStage];
        Log("Initialize `_slideList`: success");

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 슬라이드 색인 배열을 초기화하는 메서드
    public void InitSlideList()
    {
        _slideList = _random.Combination(numSlide, numStage);
        Log($"Set `_slideList`: success: [{string.Join(", ", _slideList)}]");
    }

    // 슬라이드를 초기화하는 메서드
    public void SetSlide(int stage)
    {
        if (FindSlides()) {
            if (stage > 0) {
                int index = _slideList[stage - 1];

                _controllerLeft.Index = index;
                _controllerRight.Index = index;

                _controllerLeft.ResetSlide();
                _controllerRight.ResetSlide();

                Log("Set slide: success: {index}");
            } else {
                _objectLeft.SetActive(false);
                _objectRight.SetActive(false);
            }
        }
    }

    // 슬라이드를 찾는 메서드
    private bool FindSlides()
    {
        bool res = true;

        // `_objectLeft` 찾기
        _objectLeft = GameObject.Find(nameLeft);
        if (_objectLeft != null) {
            Log("Find `_objectLeft`: success");
        } else {
            Log("Find `_objectLeft`: failed", mode: 1);
            res = false;
        }

        // `_controllerLeft` 찾기
        _controllerLeft = _objectLeft.GetComponent<SlideController>();
        if (_controllerLeft != null) {
            Log("Find `_controllerLeft`: success");
        } else {
            Log("Find `_controllerLeft`: failed", mode: 1);
            res = false;
        }

        // `_objectRight` 찾기
        _objectRight = GameObject.Find(nameRight);
        if (_objectRight != null) {
            Log("Find `_objectRight`: success");
        } else {
            Log("Find `_objectRight`: failed", mode: 1);
            res = false;
        }

        // `_controllerRight` 찾기
        _controllerRight = _objectRight.GetComponent<SlideController>();
        if (_controllerRight != null) {
            Log("Find `_controllerRight`: success");
        } else {
            Log("Find `_controllerRight`: failed", mode: 1);
            res = false;
        }

        return res;
    }
}
