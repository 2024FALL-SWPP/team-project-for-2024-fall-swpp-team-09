using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : AbstractBehaviour
{
    /*************
     * constants *
     *************/

    // 신 이름
    private const string DEFAULT_SCENE = "DefaultGameScene";
    private const string ENDING_SCENE = "GameEnding Scene";

    /****************
     * enumerations *
     ****************/

    public enum GameState
    {
        Playing,
        Ending,

        // deprecated states
        Sleeping,
        GameOver,
        Paused
    }

    /**********
     * fields *
     **********/

    // 이상현상 컨트롤러
/*  private AbstractAnomalyObject _anomalyController; */

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "GameManager";

    // 클래스 인스턴스
    public static GameManager Instance { get; private set; }

    // 게임 상태
    public int Stage { get; private set; }

    public bool IsCleared { get; private set; }

    public GameState State { get; private set; }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = false;

        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            res = base.Awake_();
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        Stage = 0;
        IsCleared = false;
        State = GameState.Playing;

        Log("Call `StartStage` asynchronously");
        StartCoroutine(StartStage());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 스테이지 완료 시 호출되는 메서드
    public bool SetStageClear()
    {
        bool res = true;

        IsCleared = true;
/*      if (_anomalyController != null) {
            Log($"Call `{_anomalyController.Name}.ResetAnomaly` begin");
            if (_anomalyController.ResetAnomaly()) {
                Log($"Call `{_anomalyController.Name}.ResetAnomaly` success");
            } else {
                Log($"Call `{_anomalyController.Name}.ResetAnomaly` failed", mode: 2);
                res = false;
            }
        } else {
            Log($"Call `ResetAnomaly` of the anomaly controller failed", mode: 2);
            res = false;
        } */

        return res;
    }

    // 잠들 시 호출되는 메서드
    public bool Sleep()
    {
        bool res = true;

/*      Log("Call `PlayerManager.Sleep` begin");
        if (PlayerManager.Instance.Sleep()) {
            Log("Call `PlayerManager.Sleep` success");
        } else {
            Log("Call `PlayerManager.Sleep` failed", mode: 2);
            res = false;
        } */

        if (IsCleared) {
            Log("Stage clear => next stage");
            Stage++;
            IsCleared = false;
        } else {
            Log("Stage failed => stage 1");
            Stage = 1;
        }

        if (Stage <= 8) {
            Log("Call `StartStage` asynchronously");
            StartCoroutine(StartStage());
        } else {
            Log("Call `StartEnding` begin");
            if (StartEnding()) {
                Log("Call `StartEnding` success");
            } else {
                Log("Call `StartEnding` failed", mode: 2);
                res = false;
            }
        }

        return res;
    }

    // 게임오버 시 호출되는 메서드
    public bool GameOver()
    {
        bool res = true;

/*      Log("Call `PlayerManager.GameOver` begin");
        if (PlayerManager.Instance.GameOver()) {
            Log("Call `PlayerManager.GameOver` success");
        } else {
            Log("Call `PlayerManager.GameOver` failed", mode: 2);
            res = false;
        } */

        Stage = 1;
        IsCleared = false;

        Log("Call `StartStage` asynchronously");
        StartCoroutine(StartStage());

        return res;
    }

    // 단계를 시작하는 메서드
    private IEnumerator StartStage()
    {
        AsyncOperation asyncOperation;
        AbstractStageObserver[] observers;

        SceneManager.LoadScene(DEFAULT_SCENE);
        asyncOperation = Resources.UnloadUnusedAssets();

        while (!asyncOperation.isDone) {
            yield return null;
        }

        observers = FindObjectsByType<AbstractStageObserver>(FindObjectsSortMode.None);
        for (int idx = 0; idx < observers.Length; idx++) {
            observers[idx].UpdateStage();
        }
        if (Stage == 1) {
            AnomalyManager.Instance.ResetAnomaliesOnFailure();
        }

/*      _anomalyController = AnomalyManager.Instance.GetAnomalyController();
        _anomalyController.StartAnomaly(); */
        AnomalyManager.Instance.CheckAndInstantiateAnomaly();

/*      Log("Call `PlayerManager.WakeUp` begin");
        if (PlayerManager.Instance.WakeUp()) {
            Log("Call `PlayerManager.WakeUp` success");
        } else {
            Log("Call `PlayerManager.WakeUp` failed", mode: 2);
            res = false;
        } */
        FindObjectOfType<PlayerController>().WakeUp();
    }

    // 엔딩을 시작하는 메서드
    private bool StartEnding()
    {
        bool res = true;

        State = GameState.Ending;
        SceneManager.LoadScene(ENDING_SCENE);

        return res;
    }

    /**********************
     * deprecated methods *
     **********************/

    public void SetStageNoClear()
    {
        IsCleared = false;
    }

    public int GetCurrentStage()
    {
        return Stage;
    }

    public void SetCurrentStage(int stage)
    {
        Stage = stage;
    }

    public bool IsStageClear()
    {
        return IsCleared;
    }

    public GameState GetGameState()
    {
        return State;
    }

    public void RestartGame()
    {

        Stage = 0;
        IsCleared = false;
        State = GameState.Playing;

        Log("Call `StartStage` asynchronously");
        StartCoroutine(StartStage());
    }

    public void PauseGame()
    {
        if (State == GameState.Playing) {
            State = GameState.Paused;
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        if (State == GameState.Paused) {
            State = GameState.Playing;
            Time.timeScale = 1;
        }
    }
}
