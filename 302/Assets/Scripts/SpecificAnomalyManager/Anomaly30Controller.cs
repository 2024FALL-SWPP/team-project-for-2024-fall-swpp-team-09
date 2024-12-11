using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class Anomaly30Controller : MonoBehaviour
{
    public GameObject[] windows;
    private GameObject player;
    private GameManager gameManager;
    private Camera mainCamera;
    public VideoClip stormVideoClip;
    public AudioClip stormAudioClip;
    public AudioClip openingSound;
    public AudioClip closingSound;

    // 창문 열리는 interval의 Min/Max
    private float intervalMin = 2f;
    private float intervalMax = 5f;
    private bool isAnomalyStopped = false; // Flag to stop attaching scripts
    private Anomaly30_thunderstorm thunderstorm;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        mainCamera = Camera.main;
        
        Transform windowsParent = GameObject.Find("Classroom/WallLeft/windows").transform;
        windows = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            windows[i] = windowsParent.GetChild(i + 2).gameObject; // window_001 (3)부터 window_001 (12)까지
        }

        // 창문 밖에 폭풍우 비디오 재생 
        thunderstorm = gameObject.AddComponent<Anomaly30_thunderstorm>();
        thunderstorm.videoClip = stormVideoClip;
        thunderstorm.audioClip = stormAudioClip;
        thunderstorm.screenPosition = new Vector3(-2.51f, 4.72f, 18.91f);
        thunderstorm.screenScale = new Vector3(46.9397f, 10.97118f, 1.538f);
        thunderstorm.CreateThunderstormScreen();

        // Script를 붙여 창문들 여는 Coroutine
        StartCoroutine(ApplyAnomalyWindowScriptsAndCollider());

        // 20초 후 창문 열기 중지
        StartCoroutine(EndWindowsOpeningAfterTime(20f));
    }

    private IEnumerator ApplyAnomalyWindowScriptsAndCollider()
    {
        yield return new WaitForSeconds(2f); // 2초 후 시작

        while (!isAnomalyStopped)
        {
            float interval = Random.Range(intervalMin, intervalMax);
            int randomIndex = Random.Range(0, windows.Length);

            GameObject randomWindow = windows[randomIndex];
            GameObject anta = randomWindow.transform.Find("window/Anta.003").gameObject;
            anta.layer = 3;

            // 이미 열려 있으면 패스
            if (anta.GetComponent<Anomaly30_window>() != null)
            {
                Debug.Log($"Script already attached to {anta.name}, skipping.");
                yield return new WaitForSeconds(interval);
                continue;
            }
            
            anta.AddComponent<BoxCollider>();
            Anomaly30_window windowScript = anta.AddComponent<Anomaly30_window>();
            windowScript.openingSound = openingSound;
            windowScript.closingSound = closingSound;


            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator EndWindowsOpeningAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        isAnomalyStopped = true;
        StopAnomaly();
        gameManager.SetStageClear();

        thunderstorm.DestroyThunderstormScreen(); // 비디오 화면 제거
        CloseAllWindows();
    }

    private void CloseAllWindows()
    {
        foreach (GameObject window in windows)
        {
            Transform antaTransform = window.transform.Find("window/Anta.003");
            Anomaly30_window windowScript = antaTransform.GetComponent<Anomaly30_window>();
            if (windowScript != null)
            {
                windowScript.CloseWindow();
            }
        }
    }

    public void PlayerDieFromStorm(Vector3 windowPosition)
    {
        StopAnomaly();

        // 열려 있는 창문으로 화면 전환
        mainCamera.transform.LookAt(windowPosition);

        // 책상/의자 뚫고 오른쪽으로 날아갈 수 있도록 player BoxCollider 제거
        BoxCollider playerCollider = this.player.GetComponent<BoxCollider>();
        if (playerCollider != null)
        {
            Destroy(playerCollider);
        }

        // 오른쪽 벽 z = -15
        Vector3 targetPosition = new Vector3(this.player.transform.position.x, this.player.transform.position.y, -15f);
        StartCoroutine(MovePlayerToPosition(targetPosition));

        PlayerController playerController = this.player.GetComponent<PlayerController>();
        playerController.Sleep();

        StartCoroutine(CallGameManagerSleepAfterDelay(3f));
    }

    private IEnumerator MovePlayerToPosition(Vector3 targetPosition)
    {
        float duration = 1f;
        Vector3 initialPosition = player.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            player.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPosition;
    }

    private IEnumerator CallGameManagerSleepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameManager != null)
        {
            gameManager.SetStageNoClear(); // 혹시 날라갈동안 Anomaly time이 끝나서 성공할 수도 있으니
            gameManager.Sleep();
        }
    }

    public void StopAnomaly()
    {
        isAnomalyStopped = true;
    }
}
