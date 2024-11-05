using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject titleText;             // 타이틀 텍스트 오브젝트
    public Button startButton;               // 시작 버튼
    public GameObject difficultyButtonGroup; // 난이도 버튼 그룹
    public Button normalButton;              // 노말 난이도 버튼
    public Button hardButton;                // 하드 난이도 버튼
    private int difficultyLevel;             // 난이도 변수

    void Start()
    {
        // 초기 설정
        difficultyButtonGroup.SetActive(false); // 난이도 선택 그룹 숨기기

        // 버튼에 이벤트 연결
        startButton.onClick.AddListener(OnStartButtonClicked);                // Start 버튼
        normalButton.onClick.AddListener(() => SetDifficultyAndStartGame(0)); // Normal 버튼
        hardButton.onClick.AddListener(() => SetDifficultyAndStartGame(1));   // Hard 버튼
    }

    // 시작 버튼 클릭 시 호출
    private void OnStartButtonClicked()
    {
        titleText.SetActive(false);                 // 타이틀 텍스트 숨기기
        startButton.gameObject.SetActive(false);    // 시작 버튼 숨기기
        difficultyButtonGroup.SetActive(true);      // 난이도 선택 그룹 표시
    }

    // 난이도 설정 및 씬 전환
    private void SetDifficultyAndStartGame(int difficulty)
    {
        difficultyLevel = difficulty; // 난이도 설정
        SceneManager.LoadScene("DefaultGameScene"); // 씬 전환
    }
}
