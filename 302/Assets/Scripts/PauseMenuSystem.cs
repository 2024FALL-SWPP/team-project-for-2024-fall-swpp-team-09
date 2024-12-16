using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuSystem : MonoBehaviour
{
    public Canvas pauseCanvas;
    public float exitPressTime = 1.5f;
    private float escHoldTime = 0f;
    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            escHoldTime += Time.unscaledDeltaTime;
            
            if (escHoldTime >= exitPressTime)
            {
                ExitGame();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (escHoldTime < exitPressTime)
            {
                TogglePause();
            }
            escHoldTime = 0f;
        }

        if (isPaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                return;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseCanvas.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    void ExitGame()
    {
        Debug.Log("Exiting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
