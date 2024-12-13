using UnityEngine;

public class ClockController : AbstractBehaviour
{
    /**************
     * properties *
     **************/

    public override string Name { get; } = "ClockController";

    /***************
     * new methods *
     ***************/

    public void SetTime(int stage)
    {
        int hour = 7;
        int minute = 0;
        float hourRotation, minuteRotation;

        if (stage == 0) {
            // Added by 박상윤
            // 0스테이지는 기존과 다르게 그냥 대입
            hour = 6;
            minute = 50;
        } else if (stage > 0) {
            // stage 1 at 7:00, stage 8 at 8:45
            hour = 7 + ((stage - 1) / 4);
            minute = 15 * ((stage - 1) % 4);
        } else {
            Log($"Invalid stage: {stage}", mode: 2);
        }

        hourRotation = hour * 30.0f + minute * 0.5f;  // 30 degrees per hour + 0.5 degrees per minute
        minuteRotation = minute * 6.0f;               // 6 degrees per minute

        transform.Find("H").localRotation = Quaternion.Euler(0, 0, 90 + hourRotation);
        transform.Find("M").localRotation = Quaternion.Euler(0, 0, 90 + minuteRotation);
    }
}

/* modified by 신채환
 *
 * - 유일성 삭제
 *   - `Instance` 프로퍼티 삭제
 *   - `DontDestroyOnLoad`에 넣는 코드 삭제
 *   - 유일성이 보장되지 않아도 되도록 코드 수정
 * - `AbstractBehaviour`를 상속하도록 변경
 *   - 그냥... 내 맘대로
 */
