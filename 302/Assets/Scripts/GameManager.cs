using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // 스테이지 관리
    [SerializeField] private int currentStage = 0;
    [SerializeField] private bool currentStageClear = false;  // 현재 스테이지 클리어 여부
    [SerializeField] private ClockController clockController;
    private const string DEFAULT_SCENE = "DefaultGameScene";
    private const string ENDING_SCENE = "GameEndingScene";
    public enum GameState
    {
        Playing,
        Sleeping,
        Paused,
        GameOver,
        GameClear
    }
    private GameState gameState;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        clockController = ClockController.Instance;
        // 시계 바늘 7:00로 초기화
        clockController.SetTime(currentStage);

        // Added by 신 채 환
        // 슬라이드 초기화
        SlideManager.Instance.UpdateStage();
    }
    private void InitializeGame()
    {
        currentStage = 0;
        currentStageClear = false;
        gameState = GameState.Playing;
        LoadDefaultScene();
    }
    public void SetCurrentStage(int stage){
        currentStage = stage;
    }

    // 스테이지 클리어 조건 달성 시 호출
    public void SetStageClear()
    {
        currentStageClear = true;
    }
    public void SetStageNoClear()
    {
        currentStageClear = false;
    }
    // 플레이어가 Sleep 선택 시 호출
    public void Sleep()
    {
        // Added by 신 채 환
        // 시계 초기화
        GameObject clock = GameObject.Find("clock");
        if (clock != null) {
            Anomaly18_Object script = clock.GetComponent<Anomaly18_Object>();
            if (script != null && script.enabled) {
                script.ResetAnomalyNow();
            }
        }

        gameState = GameState.Sleeping;
        if (currentStageClear)
        {
            // 스테이지 클리어한 상태로 잠들기
            currentStage++;

            // 7단계(8:45) 이후 클리어 시 게임 클리어
            if (currentStage >= 9)
            {
                GameClear();
                return;
            }
        }
        else
        {
            // 스테이지 클리어하지 못한 상태로 잠들기
            Debug.Log("스테이지 클리어 실패!");
            currentStage = 1;  // 스테이지 초기화
            // Added by 박 상 윤
            // 이상현상 리스트 재생성 호출
            // 스테이지 실패 시, 이상현상 리스트를 초기화, 재생성 해야 하므로
            // AnomalyManager의 Stage Failure시 작동하는 함수 호출
            AnomalyManager.Instance.ResetAnomaliesOnFailure();
        }
        currentStageClear = false;  // 클리어 상태 초기화
        LoadDefaultScene();
        // Added by 박 상 윤
        // 다음 스테이지에 대한 이상현상 확인 및 생성
        // 현재 currentStage에 맞추어 이상현상을 생성해야 하므로
        // AnomalyManager의 이상현상 Instantiate 용 함수를 호출
        StartCoroutine(InstantiateAnomalyAfterLoad());
        StartCoroutine(FindAndChangeScreen());

        // Added by 서 지 희
        // LoadDefaultScene() 호출 다음에 시계 조정
        clockController.SetTime(currentStage);

        // Added by 신 채 환
        // 슬라이드 초기화
        SlideManager.Instance.UpdateStage();
    }
    private void LoadDefaultScene()
    {
        SceneManager.LoadScene(DEFAULT_SCENE);
        StartCoroutine(WakeUpPlayerAfterLoad());
    }
    private IEnumerator WakeUpPlayerAfterLoad()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.WakeUp();
        }
    }
    private IEnumerator InstantiateAnomalyAfterLoad()
    {
        yield return new WaitForSeconds(0.1f);  // 장면 로드 완료 후 잠깐 대기
        AnomalyManager.Instance.CheckAndInstantiateAnomaly();
    }

    private void GameClear()
    {
        gameState = GameState.GameClear;
        SceneManager.LoadScene(ENDING_SCENE);
    }
    // 현재 스테이지 번호 반환
    public int GetCurrentStage()
    {
        return currentStage;
    }
    // 현재 스테이지 클리어 여부 반환
    public bool IsStageClear()
    {
        return currentStageClear;
    }
    
    public GameState GetGameState()
    {
        return gameState;
    }
    public void RestartGame()
    {
        InitializeGame();
    }
    public void PauseGame()
    {
        if (gameState == GameState.Playing)
        {
            gameState = GameState.Paused;
            Time.timeScale = 0;
        }
    }
    public void ResumeGame()
    {
        if (gameState == GameState.Paused)
        {
            gameState = GameState.Playing;
            Time.timeScale = 1;
        }
    }
    private IEnumerator FindAndChangeScreen()
    {
    yield return new WaitForSeconds(0.1f); // 0.1초 대기

    GameObject laptopObject = GameObject.FindWithTag("Laptop");
    if (laptopObject != null)
    {
        LaptopScreenController controller = laptopObject.GetComponent<LaptopScreenController>();
        if (controller != null)
        {
            controller.ChangeScreen(currentStage);
            // modified by 신채환
            // 0단계 추가에 따른 화면 색인 변경 반영
        }
    } else {
        Debug.Log("!!!!!");
    }
    }

}