using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // 스테이지 관리
    [SerializeField] private int currentStage = 1;
    [SerializeField] private bool currentStageClear = false;  // 현재 스테이지 클리어 여부
    private const string DEFAULT_SCENE = "DefaultGameScene";
    private const string ENDING_SCENE = "EndingScene";
    
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

    private void InitializeGame()
    {
        currentStage = 1;
        currentStageClear = false;
        gameState = GameState.Playing;
        LoadDefaultScene();
    }

    // 스테이지 클리어 조건 달성 시 호출
    public void SetStageClear()
    {
        currentStageClear = true;
    }

    // 플레이어가 Sleep 선택 시 호출
    public void Sleep()
    {
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
        }

        currentStageClear = false;  // 클리어 상태 초기화
        LoadDefaultScene();
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
}