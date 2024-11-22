using UnityEngine;

public class ClockController : MonoBehaviour
{
    public static ClockController Instance { get; private set; }

    private Transform hourHand;
    private Transform minuteHand;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        hourHand = transform.Find("H");
        minuteHand = transform.Find("M");

        // initial rotation to 12:00
        hourHand.localRotation = Quaternion.Euler(0, 0, 90); 
        minuteHand.localRotation = Quaternion.Euler(0, 0, 90);
    }

    public void SetTime(int stage)
    {
        // stage 1 at 7:00, stage 8 at 8:45
        int hour = 7 + ((stage - 1) % 4); 
        int minute = 15 * ((stage - 1) % 4);
        
        // Added by 박상윤
        // 0스테이지는 기존과 다르게 그냥 대입
        if(stage == 0) {
            hour = 6;
            minute = 50;
        }

        float minuteRotation = minute * 6f; // 6 degrees per minute
        float hourRotation = (hour * 30f) + (minute * 0.5f); // 30 degrees per hour + 0.5 degrees per minute

        hourHand.localRotation = Quaternion.Euler(0, 0, 90 + hourRotation);
        minuteHand.localRotation = Quaternion.Euler(0, 0, 90 + minuteRotation);
    }
}
