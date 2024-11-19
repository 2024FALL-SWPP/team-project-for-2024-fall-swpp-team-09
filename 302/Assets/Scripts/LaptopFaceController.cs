using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopFaceController : LaptopScreenController
{
    /**********
     * fields *
     **********/

    public GameObject player;

    private int[] X0DATA = new int[] {
        394, 391, 389, 387, 386, 385, 384, 383, 382, 381, 380, 379, 379, 378, 378, 377, 377,
        376, 376, 376, 376, 375, 375, 375, 375, 375, 375, 375, 375, 375, 376, 376, 376, 376,
        377, 377, 378, 378, 379, 379, 380, 381, 382, 382, 383, 385, 386, 387, 389, 391, 394
    };
    private int[] X1DATA = new int[] {
        410, 412, 414, 416, 417, 418, 420, 421, 422, 422, 423, 424, 425, 425, 426, 426, 426,
        427, 427, 427, 428, 428, 428, 428, 428, 428, 428, 428, 428, 428, 428, 427, 427, 427,
        427, 426, 426, 425, 425, 424, 423, 422, 422, 421, 420, 418, 417, 416, 414, 412, 410
    };
    private int[] X2DATA = new int[] {
        536, 534, 532, 530, 529, 528, 526, 525, 525, 524, 523, 522, 522, 521, 520, 520, 520,
        519, 519, 519, 519, 518, 518, 518, 518, 518, 518, 518, 518, 518, 518, 519, 519, 519,
        520, 520, 520, 521, 522, 522, 523, 524, 525, 525, 526, 528, 529, 530, 532, 534, 536
    };
    private int[] X3DATA = new int[] {
        553, 555, 557, 559, 560, 561, 563, 564, 564, 565, 566, 567, 567, 568, 569, 569, 569,
        570, 570, 570, 570, 571, 571, 571, 571, 571, 571, 571, 571, 571, 570, 570, 570, 570,
        569, 569, 568, 568, 567, 567, 566, 565, 564, 564, 563, 561, 560, 559, 557, 555, 552
    };
    private int[] Y_DATA = new int[] {
        272, 273, 274, 275, 276, 277, 278, 279, 280, 281, 282, 283, 284, 285, 286, 287, 288,
        289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 301, 302, 303, 304, 305,
        306, 307, 308, 309, 310, 311, 312, 313, 314, 315, 316, 317, 318, 319, 320, 321, 322
    };

    private int TANGENT_MAX = 25;
    private float TANGENT_CONST = 12.5f;
    private int ORIGIN_X = 38;
    private int ORIGIN_Y = 242;

    private Color _colour;
    private bool _isGazing;
    private int _tangent;

    /**********************
     * overridden methods *
     **********************/

    // Start is called on the frame when a script is enabled just
    // before any of the Update methods are called the first time.
    void Start()
    {
        _colour = defaults[0].GetPixel(407, 297);
        _isGazing = false;
        _tangent = 0;

        ResetScreen();
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (_isGazing) {
            UpdateGazing();
        }
    }

    /***************
     * new methods *
     ***************/

    // 화면을 초기화하는 메서드
    public override void ResetScreen()
    {
        int screenXWidth = screenXMax - screenXMin;
        int screenYWidth = screenYMax - screenYMin;
        int defaultXWidth = defaultXMax - defaultXMin;
        int defaultYWidth = defaultYMax - defaultYMin;

        _isGazing = false;

        for (int x = 0; x < screenXWidth; x++) {
            for (int y = 0; y < screenYWidth; y++) {
                int xSlide = (int)((x + 0.5) * defaultXWidth / screenXWidth + defaultXMin);
                int ySlide = (int)((y + 0.5) * defaultYWidth / screenYWidth + defaultYMin);
                Color color = defaults[0].GetPixel(xSlide, ySlide);

                screen.SetPixel(x + screenXMin, y + screenYMin, color);
            }
        }

        screen.Apply();
    }

    // 쳐다보기 시작하는 메서드
    public void StartGazing()
    {
        _isGazing = true;
    }

    // 쳐다보기 화면을 갱신하는 메서드
    private void UpdateGazing()
    {
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 direction = Vector3.ProjectOnPlane(player.transform.position - transform.position, Vector3.up);
        float dotProd = Vector3.Dot(direction, forward);
        float crossProd = Vector3.Cross(direction, forward).y;
        int tangentNew;

        if (dotProd > 0.0f) {
            tangentNew = (int)(crossProd / dotProd * TANGENT_CONST);
            if (tangentNew > TANGENT_MAX) {
                tangentNew = TANGENT_MAX;
            } else if (tangentNew < -TANGENT_MAX) {
                tangentNew = -TANGENT_MAX;
            }
        } else if (crossProd >= 0) {
            tangentNew = TANGENT_MAX;
        } else {
            tangentNew = -TANGENT_MAX;
        }

        if (tangentNew > _tangent) {
            int diff = tangentNew - _tangent;
            int x0, x1, x2, x3, y;

            for (int i = 0; i < Y_DATA.Length; i++) {
                x0 = ORIGIN_X + X0DATA[i] + _tangent;
                x1 = ORIGIN_X + X1DATA[i] + _tangent;
                x2 = ORIGIN_X + X2DATA[i] + _tangent;
                x3 = ORIGIN_X + X3DATA[i] + _tangent;
                y = ORIGIN_Y + Y_DATA[i];
                for (int j = 0; j <= diff; j++) {
                    screen.SetPixel(x3++, y, _colour);
                    screen.SetPixel(x2++, y, Color.white);
                    screen.SetPixel(x1++, y, _colour);
                    screen.SetPixel(x0++, y, Color.white);
                }
            }
        } else if (tangentNew < _tangent) {
            int diff = _tangent - tangentNew;
            int x0, x1, x2, x3, y;

            for (int i = 0; i < Y_DATA.Length; i++) {
                x0 = ORIGIN_X + X0DATA[i] + _tangent;
                x1 = ORIGIN_X + X1DATA[i] + _tangent;
                x2 = ORIGIN_X + X2DATA[i] + _tangent;
                x3 = ORIGIN_X + X3DATA[i] + _tangent;
                y = ORIGIN_Y + Y_DATA[i];
                for (int j = 0; j <= diff; j++) {
                    screen.SetPixel(x0--, y, _colour);
                    screen.SetPixel(x1--, y, Color.white);
                    screen.SetPixel(x2--, y, _colour);
                    screen.SetPixel(x3--, y, Color.white);
                }
            }
        }

        screen.Apply();
        _tangent = tangentNew;
    }
}
