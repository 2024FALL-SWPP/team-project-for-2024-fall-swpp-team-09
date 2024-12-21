using UnityEngine;

public class PauseManager : AbstractStageObserver
{
    /**********
     * fields *
     **********/

    // 캔버스 이름
    public string nameCanvas;

    // 종료를 위한 입력 시간
    public double timeExit;

    // 캔버스
    private Canvas _canvas;

    // 시간
    private double _timeKeyDown;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "PauseManager";

    // 클래스 인스턴스
    public static PauseManager Instance { get; private set; }

    /************
     * messages *
     ************/

    void Update()
    {
        double time = Time.unscaledTimeAsDouble;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            _timeKeyDown = time;
        } else if (Input.GetKey(KeyCode.Escape) && time - _timeKeyDown >= timeExit) {
            ExitGame();
        } else if (Input.GetKeyUp(KeyCode.Escape) && time - _timeKeyDown < timeExit) {
            if (GameManager.Instance.State == GameManager.GameState.Playing) {
                PauseGame();
            } else if (GameManager.Instance.State == GameManager.GameState.Paused) {
                ResumeGame();
            }
        }
    }

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

    // `Start` 메시지 용 메서드
    protected override bool Start_()
    {
        bool res = base.Start_();

        // TODO: `Start` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // TODO: 필드 초기화할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // (사실 필드 말고 초기화할 것도 넣어도 됨....)
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    /*****************************************
     * implementation: AbstractStageObserver *
     *****************************************/

    // 단계 변경 시 불리는 메서드
    public override bool UpdateStage()
    {
        GameObject obj = GameObject.Find(nameCanvas);
        bool res = base.UpdateStage();

        if (obj != null) {
            Log($"Find `{nameCanvas}` success");

            _canvas = obj.GetComponent<Canvas>();
            if (_canvas != null) {
                Log("Find `Canvas` success");
                _canvas.gameObject.SetActive(false);
            } else {
                Log("Find `Canvas` failed", mode: 1);
                res = false;
            }
        } else {
            Log($"Find `{nameCanvas}` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /***************
     * new methods *
     ***************/

    private void PauseGame()
    {
        Log("Pause the game");
        GameManager.Instance.PauseGame();
        _canvas.gameObject.SetActive(true);
    }

    private void ResumeGame()
    {
        Log("Resume the game");
        GameManager.Instance.ResumeGame();
        _canvas.gameObject.SetActive(false);
    }

    private void ExitGame()
    {
        Log("Exit the game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
