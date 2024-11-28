using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Anomaly27_magnifyingclock : MonoBehaviour
{
    [SerializeField] private float scaleIncreaseRate = 0.1f; // 초당 증가할 스케일 크기
    [SerializeField] private float scaleStartDelay = 5f;     // 스케일 증가 시작 딜레이
    private bool isScaling = false;                         // 스케일 증가 여부
    private bool isStageClear = true;
    private Collider objectCollider;

    void Start()
    {
        GameManager.Instance.SetStageClear(); // 시작 시 스테이지 클리어 설정
        isStageClear = true;
        objectCollider = GetComponent<Collider>();
        objectCollider.isTrigger = false; // Trigger 대신 물리 충돌로 처리

        // 5초 후 스케일 증가 시작
        Invoke(nameof(StartScaling), scaleStartDelay);
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
                GameManager.Instance.SetStageNoClear();
                isStageClear = false; // 상태를 변경해 중복 호출 방지

                // PlayerController의 GameOver 호출
                var playerController = collision.collider.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.GameOver();
                    Debug.Log("PlayerController.GameOver() called successfully.");
                }
                else
                {
                    Debug.LogError("PlayerController not found on Player object!");
                }

                // 씬 강제 전환 확인을 위한 보조 처리
                StartCoroutine(ForceSceneTransition());
            }
        }
    }

    private System.Collections.IEnumerator ForceSceneTransition()
    {
        yield return new WaitForSeconds(5f); // PlayerController 애니메이션 실행 시간 대기
        Debug.Log("ForceSceneTransition: Manually triggering GameManager.Instance.Sleep().");

        // GameManager.Sleep() 강제 호출
        GameManager.Instance.Sleep();
    }
}
