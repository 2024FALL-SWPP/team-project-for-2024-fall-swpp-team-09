using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Anomaly27_magnifyingclock : AbstractAnomalyInteractable
{

    public override string Name { get; } = "Anomaly27_magnifyingclock";

    [SerializeField] private float scaleIncreaseRate; // 초당 증가할 스케일 크기
    [SerializeField] private float scaleStartDelay;     // 스케일 증가 시작 딜레이
    private bool isScaling;                         // 스케일 증가 여부
    private bool isStageClear = true;
    private Collider objectCollider;

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        scaleIncreaseRate = -100f;
        scaleStartDelay = 5f;
        isScaling = false;

        GameManager.Instance.SetStageClear(); // 시작 시 스테이지 클리어 설정
        isStageClear = true;
        objectCollider = GetComponent<Collider>();
        objectCollider.isTrigger = false; // Trigger 대신 물리 충돌로 처리

        // 5초 후 스케일 증가 시작
        Invoke(nameof(StartScaling), scaleStartDelay);
        
        return res;
    }

    void Update()
    {
        if (isScaling)
        {
            // 시간에 따라 점점 오브젝트와 콜라이더의 스케일을 증가
            float scaleIncrease = scaleIncreaseRate * Time.deltaTime;
            transform.localScale += new Vector3(scaleIncrease, scaleIncrease, scaleIncrease);
        }
    }

    private void StartScaling()
    {
        isScaling = true;
        Debug.Log("MagnifyingClock: Scaling has started after delay.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player와 충돌했을 때
        if (collision.collider.CompareTag("Player"))
        {
            if (isStageClear)
            {
                Debug.Log("Collision with Player detected. Setting StageNoClear and calling GameOver.");
                isStageClear = false; // 상태를 변경해 중복 호출 방지

                PlayerManager.Instance.GameOver();
            }
        }
    }
}
