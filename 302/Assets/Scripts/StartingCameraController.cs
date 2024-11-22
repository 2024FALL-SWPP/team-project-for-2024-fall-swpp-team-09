using UnityEngine;
using System;

public class StartingCameraController : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader; // ScreenFader 컴포넌트
    [SerializeField] private float fadeOutDuration = 2f; // 화면 어두워지는 시간
    [SerializeField] private float fadeInDuration = 2f;  // 화면 밝아지는 시간
    [SerializeField] private float waitBeforeFadeIn = 0.5f; // 화면이 어두워진 상태에서 대기 시간

    public event Action OnFadeComplete; // 페이드 완료 시 이벤트
    private bool isFading = false;

    void Start()
    {
        if (screenFader == null)
        {
            screenFader = FindObjectOfType<ScreenFader>();
            if (screenFader == null)
            {
                Debug.LogError("ScreenFader component not found in the scene!");
                return;
            }
        }
    }

    public void PlayFadeSequence()
    {
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
    }

    public void PlayFadeOut()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void PlayFadeIn()
    {
        if (!isFading)
        {
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeSequence()
    {
        isFading = true;

        // Fade Out
        yield return FadeOut();

        // 대기
        yield return new WaitForSeconds(waitBeforeFadeIn);

        // Fade In
        yield return FadeIn();

        yield return FadeOut();
        yield return new WaitForSeconds(waitBeforeFadeIn);
        yield return FadeIn();
        yield return FadeOut();

        isFading = false;

        // 애니메이션 완료 신호 전송
        OnFadeComplete?.Invoke();
    }

    private System.Collections.IEnumerator FadeOut()
    {
        isFading = true;
        screenFader.StartFade(1f, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
    }

    private System.Collections.IEnumerator FadeIn()
    {
        screenFader.StartFade(0f, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration);
        isFading = false;
    }
}
