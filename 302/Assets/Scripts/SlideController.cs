using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : SCH_Behaviour
{
    /**********
     * fields *
     **********/

    // 텍스처
    public Texture2D slideTexture;
    public Texture2D[] defaultTextures;

    // 난수
    private SCH_Random _random;

    // 내부 수치
    private List<int> _trickleXMinList;
    private List<int> _trickleXMaxList;
    private List<float> _trickleYPrevList;
    private List<float> _trickleYCurrList;
    private List<int> _trickleSpeedList;

    private bool _isTrickling;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SlideController";

    // 화면 색인
    private int _index = 0;
    public int Index {
        get => _index;

        set
        {
            if (value >= 0 && value < defaultTextures.Length) {
                _index = value;
            }
        }
    }

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        UpdateTrickling();
    }

    /********************************
     * implmentation: SCH_Behaviour *
     ********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // 난수
        _random = new SCH_Random();
        Log("Initialize `_random`: success");

        // 흘러내림 관련 필드
        _trickleXMinList = new List<int>();
        Log("Initialize `_trickleXMinList`: success");

        _trickleXMaxList = new List<int>();
        Log("Initialize `_trickleXMinList`: success");

        _trickleYPrevList = new List<float>();
        Log("Initialize `_trickleYPrevList`: success");

        _trickleYCurrList = new List<float>();
        Log("Initialize `_trickleYCurrList`: success");

        _trickleSpeedList = new List<int>();
        Log("Initialize `_trickleSpeedList`: success");

        _isTrickling = false;
        Log("Initialize `_isTrickling`: success");

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 슬라이드를 초기화하는 메서드
    public void ResetSlide()
    {
        _isTrickling = false;

        slideTexture.SetPixels(defaultTextures[Index].GetPixels());
        slideTexture.Apply();
    }

    // 흘러내림을 시작하는 메서드
    public void StartTrickling()
    {
        if (!_isTrickling) {
            _trickleXMinList.Clear();
            _trickleXMaxList.Clear();
            _trickleYPrevList.Clear();
            _trickleYCurrList.Clear();
            _trickleSpeedList.Clear();
            for (int i = 0; i < 8192; i++) {
                int x = _random.Next(2048);
                int y = _random.Next(8, 2048);

                if (slideTexture.GetPixel(x, y) == Color.black
                    && slideTexture.GetPixel(x, y - 8) == Color.white)
                {
                    int xMin = x, xMax = x;

                    for (int j = 0; j < 20; j++) {
                        if (slideTexture.GetPixel(xMin, y) == Color.black) {
                            xMin--;
                        }

                        if (slideTexture.GetPixel(xMax, y) == Color.black) {
                            xMax++;
                        }
                    }

                    xMax = (int)_random.TriangularDist(x, xMax, x);
                    xMin = (int)_random.TriangularDist(xMin, x, x);

                    _trickleXMaxList.Add(xMax);
                    _trickleXMinList.Add(xMin);
                    _trickleYPrevList.Add(y);
                    _trickleYCurrList.Add(y);
                    _trickleSpeedList.Add((int)(_random.LogNormalDist(0.0, 1.0) * (xMax - xMin)));
                }
            }

            _isTrickling = true;
        }
    }

    // 흘러내림 상태를 갱신하는 메서드
    private void UpdateTrickling()
    {
        if (_isTrickling) {
            int xMin, xMax, yMin, yMax;

            for (int idx = 0; idx < _trickleXMinList.Count; idx++) {
                _trickleYPrevList[idx] = _trickleYCurrList[idx];
                _trickleYCurrList[idx] -= _trickleSpeedList[idx] * Time.deltaTime;
                if (_trickleYCurrList[idx] < 0.0f) {
                    _trickleYCurrList[idx] = 0.0f;
                }

                xMax = _trickleXMaxList[idx];
                xMin = _trickleXMinList[idx];
                yMax = (int)Mathf.Ceil(_trickleYPrevList[idx]);
                yMin = (int)Mathf.Floor(_trickleYCurrList[idx]);
                for (int x = xMin; x < xMax; x++) {
                    for (int y = yMin; y < yMax; y++) {
                        slideTexture.SetPixel(x, y, Color.black);
                    }
                }
            }

            slideTexture.Apply();
        }
    }

    // 슬라이드를 서서히 초기화하는 메서드
    public IEnumerator ResetAsync(float duration)
    {
        Color[] defaults = defaultTextures[Index].GetPixels();
        int length = defaults.Length;
        float timeStart = Time.time;
        float time;

        _isTrickling = false;

        while ((time = Time.time - timeStart) < duration) {
            Color[] colours = new Color[length];
            float t = time / duration;

            for (int idx = 0; idx < length; idx++) {
                colours[idx] = Color.Lerp(Color.white, defaults[idx], t);
            }

            slideTexture.SetPixels(colours);
            slideTexture.Apply();

            yield return null;
        }
    }
}
