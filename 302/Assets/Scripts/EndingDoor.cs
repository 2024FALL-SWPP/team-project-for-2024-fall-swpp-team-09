using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingDoor : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private GameObject creditUI;
    [SerializeField] private AudioSource endingAudioSource;
    [SerializeField] private AudioClip endingMusic;
    [SerializeField] private AudioSource doorSoundSource;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private float musicDuration = 5f; // 음악 재생 시간
    [SerializeField] private float musicFadeSpeed = 1f; // 페이드아웃 속도

    private void Start()
    {
        if (creditUI != null)
            creditUI.SetActive(false);

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StartEnding(other.gameObject));
        }
    }

    private IEnumerator StartEnding(GameObject player)
    {
        if (doorSoundSource != null && doorOpenSound != null)
        {
            doorSoundSource.clip = doorOpenSound;
            doorSoundSource.Play();
        }

        yield return new WaitForSeconds(0.5f);

        float alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Destroy(playerController);
        }

        yield return new WaitForSeconds(1f);

        if (endingAudioSource != null && endingMusic != null)
        {
            endingAudioSource.clip = endingMusic;
            endingAudioSource.volume = 1f;
            endingAudioSource.Play();
            StartCoroutine(FadeOutMusic());
        }

        if (creditUI != null)
            creditUI.SetActive(true);
    }

    private IEnumerator FadeOutMusic()
    {
        // 설정된 시간만큼 대기
        yield return new WaitForSeconds(musicDuration);

        // 페이드아웃
        float volume = 1f;
        while (volume > 0)
        {
            volume -= Time.deltaTime * musicFadeSpeed;
            endingAudioSource.volume = Mathf.Max(0, volume);
            yield return null;
        }
        ExitGame();
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