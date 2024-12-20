using System.Collections.Generic;
using UnityEngine;

public class SlideManager : AbstractStageObserver
{
    /**********
     * fields *
     **********/

    // 슬라이드 오브젝트 이름
    public string[] names;

    // 개수
    public int numSlide;
    public int numStage;

    // 난수
    private SCH_Random _random;

    // 오브젝트
    private List<GameObject> _objects;

    // 슬라이드 컨트롤러
    private List<SlideController> _controllers;

    // 슬라이드 색인 배열
    private int[] _slideList;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SlideManager";

    // 클래스 인스턴스
    public static SlideManager Instance { get; private set; }

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

        // _random
        _random = new SCH_Random();
        Log("Initialize `_random` success");

        // _objects
        _objects = new List<GameObject>();

        // _controllers
        _controllers = new List<SlideController>();

        // _slideList
        _slideList = new int[numStage];
        Log("Initialize `_slideList` success");

        return res;
    }

    /*****************************************
     * implementation: AbstractStageObserver *
     *****************************************/

    // 단계 변경 시 불리는 메서드
    public override bool UpdateStage()
    {
        int stage = GameManager.Instance.GetCurrentStage();
        bool res = true;

        if (stage == 0) {
            Log("Call `PutAwaySlides` begin");
            if (PutAwaySlides()) {
                Log("Call `PutAwaySlides` success");
            } else {
                Log("Call `PutAwaySlides` failed", mode: 1);
                res = false;
            }
        } else if (stage > 0 && stage <= numStage) {
            if (stage == 1) {
                Log("Call `GenerateList` begin");
                if (GenerateList()) {
                    Log("Call `GenerateList` success");
                } else {
                    Log("Call `GenerateList` failed", mode: 1);
                    res = false;
                }
            }

            Log("Call `UpdateSlides` begin");
            if (UpdateSlides(stage)) {
                Log("Call `UpdateSlides` success");
            } else {
                Log("Call `UpdateSlides` failed", mode: 1);
                res = false;
            }
        } else {
            Log($"Invalid stage: {stage}", mode: 1);
            res = false;
        }

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 슬라이드를 치우는 메서드
    private bool PutAwaySlides()
    {
        bool res = true;

        Log("Call `FindSlides` begin");
        if (FindSlides()) {
            Log("Call `FindSlides` success");

            foreach (GameObject obj in _objects) {
                obj.transform.Translate(Vector3.down * 100.0f);
            }

            Log("Set slide success: off");
        } else {
            Log("Call `FindSlides` failed", mode: 1);
            res = false;
        }

        return res;
    }

    // 슬라이드 색인 리스트를 생성하는 메서드
    private bool GenerateList()
    {
        bool res = true;

        _slideList = _random.Combination(numSlide, numStage);
        Log($"Generate `_slideList` success: [{string.Join(", ", _slideList)}]");

        return res;
    }

    // 슬라이드를 갱신하는 메서드
    private bool UpdateSlides(int stage)
    {
        int index = _slideList[stage - 1];
        bool res = true;

        Log("Call `FindSlides` begin");
        if (FindSlides()) {
            Log("Call `FindSlides` success");

            foreach (SlideController controller in _controllers) {
                controller.Index = index;
                controller.ResetSlide();
            }

            Log($"Set slide success: {index}");
        } else {
            Log("Call `FindSlides` failed", mode: 1);
            res = false;
        }

        return res;
    }

    // 슬라이드를 찾는 메서드
    private bool FindSlides()
    {
        bool res = true;

        _objects.Clear();
        _controllers.Clear();

        for (int idx = 0; idx < names.Length; idx++) {
            GameObject obj = GameObject.Find(names[idx]);

            if (obj != null) {
                Log($"Find `{names[idx]}` success: `{obj.name}`");

                SlideController controller = obj.GetComponent<SlideController>();

                if (controller != null) {
                    Log($"Find `SlideController` of `{obj.name}` success: `{controller.Name}`");

                    _objects.Add(obj);
                    _controllers.Add(controller);
                } else {
                    Log($"Find `SlideController` of `{obj.name}` failed", mode: 1);
                    res = false;
                }
            } else {
                Log($"Find `{names[idx]}` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }
}
