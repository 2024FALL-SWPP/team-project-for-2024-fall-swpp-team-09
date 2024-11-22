using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject titleText;             // 타이틀 텍스트 오브젝트
    public Button startButton;               // 시작 버튼
    private StartingCameraController startingCameraController;

    private AsyncOperation sceneLoadOperation; // 비동기 씬 로드 작업

    void Start()
    {
        startingCameraController = FindObjectOfType<StartingCameraController>();
        if (startingCameraController == null)
        {
            Debug.LogError("StartingCameraController not found in the scene!");
        }
        else
        {
            startingCameraController.OnFadeComplete += OnFadeComplete; // 애니메이션 완료 이벤트 연결
        }

        startButton.onClick.AddListener(OnStartButtonClicked); // Start 버튼
    }

    private void OnStartButtonClicked()
    {
        titleText.SetActive(false);                 // 타이틀 텍스트 숨기기
        startButton.gameObject.SetActive(false);    // 시작 버튼 숨기기

        // 비동기 씬 로드 시작
        StartCoroutine(PreloadDefaultScene());

        if (startingCameraController != null)
        {
            startingCameraController.PlayFadeSequence(); // 화면 어두워졌다 밝아지는 애니메이션 실행
        }
    }

    private System.Collections.IEnumerator PreloadDefaultScene()
    {
        Debug.Log("Starting to preload DefaultGameScene...");
        sceneLoadOperation = SceneManager.LoadSceneAsync("DefaultGameScene");
        sceneLoadOperation.allowSceneActivation = false; // 씬 전환은 막아둠

        while (!sceneLoadOperation.isDone)
        {
            // 진행도 출력
            Debug.Log($"Scene Load Progress: {sceneLoadOperation.progress * 100}%");

            // 진행도가 90% 이상이면 준비 완료 상태
            if (sceneLoadOperation.progress >= 0.9f)
            {
                Debug.Log("Scene preload completed. Waiting for animation.");
                break;
            }
            yield return null;
        }
    }

    private void OnFadeComplete()
    {
        Debug.Log("Fade animation complete. Activating preloaded scene.");
        if (sceneLoadOperation != null)
        {
            sceneLoadOperation.allowSceneActivation = true; // 씬 전환 허용
        }
    }

    private void OnDestroy()
    {
        if (startingCameraController != null)
        {
            startingCameraController.OnFadeComplete -= OnFadeComplete; // 이벤트 연결 해제
        }
    }
}
