using UnityEngine;

public class LaptopScreenController : SCH_Behaviour
{
    /**********
     * fields *
     **********/

    // 텍스처
    public Texture2D screen;
    public Texture2D[] defaults;

    // 가변 수치
    public int screenXMin;
    public int screenXMax;
    public int screenYMin;
    public int screenYMax;

    public int defaultXMin;
    public int defaultXMax;
    public int defaultYMin;
    public int defaultYMax;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "LaptopScreenController";

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

    /************
     * messages *
     ************/

    // Start is called on the frame when a script is enabled just
    // before any of the Update methods are called the first time.
    void Start()
    {
        ResetScreen();
    }

    /***********
     * methods *
     ***********/

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
