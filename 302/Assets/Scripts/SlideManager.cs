using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManager : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "SlideManager";

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
    private SlideRandom _random;

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

    public static SlideManager Instance { get; private set; }

    /**********************
     * overridden methods *
     **********************/

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

    /***************
     * new methods *
     ***************/

    // 슬라이드 색인 배열을 초기화하는 메서드
    public void InitSlideList()
    {
        _slideList = _random.Combination(numSlide, numStage);
        Debug.Log($"[{NAME}] Set `_slideList`: [{string.Join(", ", _slideList)}]");
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

                Debug.Log($"[{NAME}] Set slides with No. {index} slide.");
            } else {
                _objectLeft.SetActive(false);
                _objectRight.SetActive(false);
            }
        }
    }

    // Private fields를 초기화하는 메서드
    //
    // 반환 값
    // - true: 초기화 성공
    // - false: 초기화 실패
    private bool InitFields()
    {
        bool res = true;

        // `_random` 초기화
        _random = new SlideRandom();
        Debug.Log($"[{NAME}] Initialized `_random` successfully.");

        // 슬라이드 초기화
        res &= FindSlides();

        // `_slideList` 초기화
        _slideList = new int[numStage];
        Debug.Log($"[{NAME}] Initialized `_slideList` successfully.");

        return res;
    }

    // 슬라이드를 찾는 메서드
    //
    // 반환 값
    // - true: 찾기 성공
    // - false: 찾기 실패
    private bool FindSlides()
    {
        bool res = true;

        // `_objectLeft` 찾기
        _objectLeft = GameObject.Find(nameLeft);
        if (_objectLeft != null) {
            Debug.Log($"[{NAME}] Find `_objectLeft` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectLeft`.");
            res = false;
        }

        // `_controllerLeft` 찾기
        _controllerLeft = _objectLeft.GetComponent<SlideController>();
        if (_controllerLeft != null) {
            Debug.Log($"[{NAME}] Find `_controllerLeft` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_controllerLeft`.");
            res = false;
        }

        // `_objectRight` 찾기
        _objectRight = GameObject.Find(nameRight);
        if (_objectRight != null) {
            Debug.Log($"[{NAME}] Find `_objectRight` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectRight`.");
            res = false;
        }

        // `_controllerRight` 찾기
        _controllerRight = _objectRight.GetComponent<SlideController>();
        if (_controllerRight != null) {
            Debug.Log($"[{NAME}] Find `_controllerRight` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_controllerRight`.");
            res = false;
        }

        return res;
    }
}

// 무작위 조합을 생성하는 클래스
class SlideRandom : System.Random
{
    // 무작위 조합을 생성하는 메서드
    public int[] Combination(int n, int r)
    {
        int[] candidates = new int[n];
        int[] result = new int[r];

        for (int i = 0; i < n; i++) {
            candidates[i] = i;
        }

        for (int i = 0; i < r; i++) {
            int index = Next(i, n);

            result[i] = candidates[index];
            if (index != i) {
                int tmp = candidates[i];

                candidates[i] = candidates[index];
                candidates[index] = tmp;
            }
        }

        System.Array.Sort(result);

        return result;
    }
}
