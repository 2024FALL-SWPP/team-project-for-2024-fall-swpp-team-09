using UnityEngine;

public class ClockController : MonoBehaviour
{
    private Transform hourHand;
    private Transform minuteHand;

    void Start()
    {
        hourHand = transform.Find("H");
        minuteHand = transform.Find("M");

        // initial rotation to 12:00
        hourHand.localRotation = Quaternion.Euler(0, 0, 90); 
        minuteHand.localRotation = Quaternion.Euler(0, 0, 90);
    }

    public void SetTime(int hour, int minute)
    {
        // Ensure hour is in 12-hour format
        hour %= 12;

        float minuteRotation = minute * 6f; // 6 degrees per minute
        float hourRotation = (hour * 30f) + (minute * 0.5f); // 30 degrees per hour + 0.5 degrees per minute

        minuteHand.localRotation = Quaternion.Euler(0, 0, 90 + minuteRotation);
        hourHand.localRotation = Quaternion.Euler(0, 0, 90 + hourRotation);
    }
}
