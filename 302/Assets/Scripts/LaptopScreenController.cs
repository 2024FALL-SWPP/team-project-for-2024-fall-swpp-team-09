using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopScreenController : MonoBehaviour
{
    // fields
    public Texture2D screen;
    public Texture2D[] defaults;

    public int screenXMin;
    public int screenXMax;
    public int screenYMin;
    public int screenYMax;

    public int defaultXMin;
    public int defaultXMax;
    public int defaultYMin;
    public int defaultYMax;

    public bool isFlipped;

    private int _inputCountMax;
    private int _inputCount;
    private int _inputIndex;

    // properties
    private int _index = 0;
    public int Index {
        get => _index;

        set
        {
            if (value >= 0 && value < defaults.Length) {
                _index = value;
            }
        }
    }
    [SerializeField] private int screenIndex = 0;

    // overridden methods
    void Start()
    {
        // init. fields for changing screen (no need for implementing the game)
        _inputCountMax = 0;
        for (int l = defaults.Length; l > 0; l /= 10) {
            _inputCountMax++;
        }

        _inputCount = 0;
        _inputIndex = 0;

        ResetScreen();
        
        //ChangeScreen(screenIndex);
    }

    void Update()
    {
        //GetNumberInput();
    }

    // new methods
    public void ResetScreen()
    {
        int screenXWidth = screenXMax - screenXMin;
        int screenYWidth = screenYMax - screenYMin;
        int defaultXWidth = defaultXMax - defaultXMin;
        int defaultYWidth = defaultYMax - defaultYMin;

        for (int x = 0; x < screenXWidth; x++) {
            for (int y = 0; y < screenYWidth; y++) {
                int xSlide = (int)((x + 0.5) * defaultXWidth / screenXWidth + defaultXMin);
                int ySlide = (int)((y + 0.5) * defaultYWidth / screenYWidth + defaultYMin);
                Color color = defaults[Index].GetPixel(xSlide, ySlide);

                screen.SetPixel(x + screenXMin, y + screenYMin, color);
            }
        }

        screen.Apply();
    }

    public void ChangeScreen(int newIndex)
    {
        Index = newIndex;
        ResetScreen();
    }

    // no need for implementing the game
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

            if (_inputIndex >= defaults.Length) {
                _inputCount = 1;
                _inputIndex = index;
            }

            if (_inputCount >= _inputCountMax) {
                if (_inputIndex < defaults.Length) {
                    _index = _inputIndex;
                    ResetScreen();
                }

                _inputCount = 0;
                _inputIndex = 0;
            }
        }
    }
}