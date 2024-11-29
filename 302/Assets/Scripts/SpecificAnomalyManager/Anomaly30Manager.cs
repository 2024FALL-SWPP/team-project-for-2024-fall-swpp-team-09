using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class Anomaly30Manager : MonoBehaviour
{
    public GameObject[] windows; // Array to hold window objects
    private GameObject player;
    private GameManager gameManager; // Reference to GameManager
    private Camera mainCamera; // Reference to the Main Camera
    public VideoClip videoClip; // Thunderstorm video
    public AudioClip audioClip; // Thunderstorm MP3
    public AudioClip openingSound; // Opening sound for windows
    public AudioClip closingSound; // Closing sound for windows
    private Anomaly30_thunderstorm thunderstorm; // Thunderstorm handler

    private float intervalMin = 2f; // Minimum interval between script application
    private float intervalMax = 5f; // Maximum interval between script application
    private bool stopAnomaly = false; // Flag to stop attaching scripts
    private float startDelay = 2f; // Delay before starting the anomaly
    public bool isWindowsOpening = true; // Boolean to track if windows are opening

    void Start()
    {
        // Find GameManager, Player, and Main Camera
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found or missing GameManager component.");
            return;
        }

        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player object not found.");
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found.");
            return;
        }

        // Locate the windows parent object
        Transform windowsParent = GameObject.Find("Classroom/WallLeft/windows")?.transform;
        if (windowsParent == null)
        {
            Debug.LogError("Windows parent object not found: Classroom/WallLeft/windows");
            return;
        }

        // Get all child objects of the "windows" parent
        windows = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            windows[i] = windowsParent.GetChild(i + 2).gameObject; // Child indices start at 2 for window_001 (3)
            if (windows[i] == null)
            {
                Debug.LogError($"Window object not found at index {i + 2} under Classroom/WallLeft/windows.");
            }
        }

        // Create the thunderstorm screen
        thunderstorm = gameObject.AddComponent<Anomaly30_thunderstorm>();
        thunderstorm.videoClip = videoClip;
        thunderstorm.audioClip = audioClip;
        thunderstorm.screenPosition = new Vector3(-2.51f, 4.72f, 18.91f);
        thunderstorm.screenScale = new Vector3(46.9397f, 10.97118f, 1.538f);
        thunderstorm.CreateThunderstormScreen();

        // Start the coroutine to apply scripts and colliders
        StartCoroutine(ApplyAnomalyWindowScriptsAndCollider());

        // Start a timer to end the anomaly after 20 seconds
        StartCoroutine(EndWindowsOpeningAfterTime(20f));
    }

    private IEnumerator ApplyAnomalyWindowScriptsAndCollider()
    {
        yield return new WaitForSeconds(startDelay);

        while (!stopAnomaly)
        {
            float interval = Random.Range(intervalMin, intervalMax);
            int randomIndex = Random.Range(0, windows.Length);

            GameObject randomWindow = windows[randomIndex];
            Transform antaTransform = randomWindow.transform.Find("window/Anta.003");
            if (antaTransform != null)
            {
                GameObject anta = antaTransform.gameObject;
                anta.layer = 3;

                // Skip if the Anomaly30_window script is already attached
                if (anta.GetComponent<Anomaly30_window>() != null)
                {
                    Debug.Log($"Script already attached to {anta.name}, skipping.");
                    yield return new WaitForSeconds(interval);
                    continue;
                }

                // Add a BoxCollider if not already present
                if (anta.GetComponent<BoxCollider>() == null)
                {
                    BoxCollider boxCollider = anta.AddComponent<BoxCollider>();
                    boxCollider.isTrigger = true; // Set to trigger if required
                    Debug.Log($"BoxCollider added to {anta.name}");
                }

                // Add Anomaly30_window component and assign sounds
                Anomaly30_window windowScript = anta.AddComponent<Anomaly30_window>();
                windowScript.openingSound = openingSound;
                windowScript.closingSound = closingSound;
            }
            else
            {
                Debug.LogWarning($"Anta.003 not found under {randomWindow.name}");
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator EndWindowsOpeningAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        isWindowsOpening = false;
        StopAnomaly();

        if (gameManager != null)
        {
            gameManager.SetStageClear();
        }

        // Destroy the thunderstorm screen
        thunderstorm.DestroyThunderstormScreen();

        // Close all windows with the Anomaly30_window script attached
        CloseAllWindows();
    }

    private void CloseAllWindows()
    {
        foreach (GameObject window in windows)
        {
            Transform antaTransform = window.transform.Find("window/Anta.003");
            if (antaTransform != null)
            {
                Anomaly30_window windowScript = antaTransform.GetComponent<Anomaly30_window>();
                if (windowScript != null)
                {
                    windowScript.CloseWindow();
                }
            }
        }

        Debug.Log("All windows closed.");
    }

    public void PlayerDieFromStorm(Vector3 windowPosition)
    {
        StopAnomaly();

        // Rotate the camera to look at the window
        mainCamera.transform.LookAt(windowPosition);

        // Remove BoxCollider from the player
        BoxCollider playerCollider = player.GetComponent<BoxCollider>();
        if (playerCollider != null)
        {
            Destroy(playerCollider);
        }

        // Move the player to z = -15
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, -15f);
        StartCoroutine(MovePlayerToPosition(targetPosition));

        // Call PlayerController's Sleep() method
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.Sleep();
        }

        // Wait 3 seconds before calling GameManager's Sleep()
        StartCoroutine(CallGameManagerSleepAfterDelay(3f));
    }

    private IEnumerator MovePlayerToPosition(Vector3 targetPosition)
    {
        float duration = 1f; // Time to move the player
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
            gameManager.Sleep();
        }
    }

    public void StopAnomaly()
    {
        stopAnomaly = true;
    }
}
