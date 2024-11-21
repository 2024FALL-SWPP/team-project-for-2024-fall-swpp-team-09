using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject clockCamera;
    [SerializeField] private AudioSource endingAudioSource;
    [SerializeField] private AudioClip endingMusic;
    [SerializeField] private float moveSpeed = 0.001f;
    [SerializeField] private GameObject player;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 1f;

    private Vector3 targetPosition = new Vector3(-15.3f, 7.25f, 0f);
    private bool isMoving = false;
    private float lerpTime = 0f;

    public void OnEnable()
    {
        if(fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
        StartEnding();
    }

    public void StartEnding()
    {
        isMoving = true;
        lerpTime = 0f;

        if (endingAudioSource != null && endingMusic != null)
        {
            endingAudioSource.clip = endingMusic;
            endingAudioSource.Play();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            lerpTime += Time.deltaTime * moveSpeed;
            clockCamera.transform.position = Vector3.Lerp(clockCamera.transform.position, targetPosition, lerpTime);

            if (Vector3.Distance(clockCamera.transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                clockCamera.transform.position = targetPosition;
                StartCoroutine(SwitchToPlayerWithFade());
            }
        }
    }

    private IEnumerator SwitchToPlayerWithFade()
    {
        // 완전히 페이드 아웃
        float alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if(fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }

        // 완전히 어두워진 후에 전환
        if(player != null)
            player.SetActive(true);
        if(clockCamera != null)
            clockCamera.SetActive(false);

        yield return new WaitForSeconds(1f);

        // 페이드 인
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            if(fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }
            yield return null;
        }
    }
}