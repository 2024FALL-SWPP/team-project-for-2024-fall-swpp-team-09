using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    // fields
    public Texture2D slideTexture;
    public Texture2D[] defaultTextures;

    private MyRandom _myRandom;
    private List<int> _trickleXMinList;
    private List<int> _trickleXMaxList;
    private List<float> _trickleYPrevList;
    private List<float> _trickleYCurrList;
    private List<int> _trickleSpeedList;

    private bool _isTrickling;

    private int _inputCountMax;
    private int _inputCount;
    private int _inputIndex;

    // properties
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

    // overridden methods
    void Start()
    {
        // init. fields for trickling
        _trickleXMinList = new List<int>();
        _trickleXMaxList = new List<int>();
        _trickleYPrevList = new List<float>();
        _trickleYCurrList = new List<float>();
        _trickleSpeedList = new List<int>();
        _myRandom = new MyRandom();

        _isTrickling = false;

        // init. fields for changing slides (no need for implementing the game)
        _inputCountMax = 0;
        for (int l = defaultTextures.Length; l > 0; l /= 10) {
            _inputCountMax++;
        }

        _inputCount = 0;
        _inputIndex = 0;

        ResetSlide();
    }

    void Update()
    {
        GetNumberInput();
        UpdateTrickling();

        if (Input.GetKeyDown(KeyCode.R)) {
            ResetSlide();
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            StartTrickling();
        }
    }

    // new methods
    public void ResetSlide()
    {
        _isTrickling = false;

        slideTexture.SetPixels(defaultTextures[Index].GetPixels());
        slideTexture.Apply();
    }

    // randomly choose the location, width, speed of tricklings
    public void StartTrickling()
    {
        if (!_isTrickling) {
            _trickleXMinList.Clear();
            _trickleXMaxList.Clear();
            _trickleYPrevList.Clear();
            _trickleYCurrList.Clear();
            _trickleSpeedList.Clear();
            for (int i = 0; i < 8192; i++) {
                int x = _myRandom.Next(2048);
                int y = _myRandom.Next(8, 2048);

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

                    xMax = (int)_myRandom.TriangularDist(x, xMax, x);
                    xMin = (int)_myRandom.TriangularDist(xMin, x, x);

                    _trickleXMaxList.Add(xMax);
                    _trickleXMinList.Add(xMin);
                    _trickleYPrevList.Add(y);
                    _trickleYCurrList.Add(y);
                    _trickleSpeedList.Add((int)(_myRandom.LogNormalDist(0.0, 1.0) * (xMax - xMin)));
                }
            }

            _isTrickling = true;
        }
    }

    // update the slide trickling
    private void UpdateTrickling()
    {
        int xMin, xMax, yMin, yMax;

        if (_isTrickling) {
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

    // get input to change slides (no need for implementing the game)
    private void GetNumberInput()
    {
        int index = -1;
        bool[] keysDown = new bool[] {
            Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0),
            Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1),
            Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2),
            Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3),
            Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4),
            Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5),
            Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6),
            Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7),
            Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8),
            Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)
        };

        if ((index = System.Array.FindIndex(keysDown, x => x)) >= 0) {
            if (System.Array.FindIndex(keysDown, index + 1, x => x) >= 0) {
                index = -1;
            }
        }

        if (index >= 0) {
            _inputCount++;
            _inputIndex = _inputIndex * 10 + index;

            if (_inputIndex >= defaultTextures.Length) {
                _inputCount = 1;
                _inputIndex = index;
            }

            if (_inputCount >= _inputCountMax) {
                if (_inputIndex < defaultTextures.Length) {
                    Index = _inputIndex;
                    ResetSlide();
                }

                _inputCount = 0;
                _inputIndex = 0;
            }
        }
    }
}

// class for randomness
class MyRandom : System.Random
{
    private double NORMAL_MAGICCONST = 4.0 * System.Math.Exp(-0.5) / System.Math.Sqrt(2.0);

    // implement methods with python's implementation
    // https://github.com/python/cpython/blob/3.13/Lib/random.py

    public double TriangularDist(double low = 0.0, double high = 1.0, double mode = 0.5)
    {
        double u, c;

        if (high == low) {
            return low;
        }

        u = Sample();
        c = (mode - low) / (high - low);

        if (u > c) {
            double tmp = low;

            u = 1.0 - u;
            c = 1.0 - c;
            low = high;
            high = tmp;
        }

        return low + (high - low) * System.Math.Sqrt(u * c);
    }

    public double NormalDist(double mu = 0.0, double sigma = 0.0)
    {
        double u1, u2, z, zz;

        while (true) {
            u1 = Sample();
            u2 = 1.0 - Sample();
            z = NORMAL_MAGICCONST * (u1 - 0.5) / u2;
            zz = z * z / 4.0;
            if (zz <= -System.Math.Log(u2)) {
                break;
            }
        }

        return mu + z * sigma;
    }

    public double LogNormalDist(double mu, double sigma)
    {
        return System.Math.Exp(NormalDist(mu, sigma));
    }
}
