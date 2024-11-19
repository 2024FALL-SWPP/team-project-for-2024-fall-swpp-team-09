using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopScreenController : MonoBehaviour
{
    /**********
     * fields *
     **********/

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

    /**************
     * properties *
     **************/

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

    /**********************
     * overridden methods *
     **********************/

    // Start is called on the frame when a script is enabled just
    // before any of the Update methods are called the first time.
    void Start()
    {
        ResetScreen();
    }

    /***************
     * new methods *
     ***************/

    public virtual void ResetScreen()
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

    public void ChangeScreen(int index)
    {
        Index = index;
        ResetScreen();
    }
}