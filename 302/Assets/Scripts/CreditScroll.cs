using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 50f;    
    [SerializeField] private float startY = -800f;       
    [SerializeField] private float endY = 800f;          
    [SerializeField] private Image fadeImage;            // 페이드아웃용 검은 이미지
    [SerializeField] private float fadeSpeed = 1f;       // 페이드 속도

    private RectTransform rectTransform;
    private bool isEnding = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // 시작 위치 설정
        Vector3 pos = rectTransform.anchoredPosition;
        pos.y = startY;
        rectTransform.anchoredPosition = pos;

        // 페이드 이미지 초기화
        if(fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if(!isEnding)
        {
            // 위로 스크롤
            Vector3 pos = rectTransform.anchoredPosition;
            pos.y += scrollSpeed * Time.deltaTime;
            rectTransform.anchoredPosition = pos;

            // 끝까지 올라갔는지 체크
            if(pos.y >= endY && !isEnding)
            {
                isEnding = true;
                StartCoroutine(EndCredits());
            }
        }
    }

    private IEnumerator EndCredits()
    {
        // 잠시 대기
        yield return new WaitForSeconds(1f);

        // 페이드 아웃 (검은색)
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

        // 잠시 대기
        yield return new WaitForSeconds(1f);

        // GameStartingScene으로 전환
        SceneManager.LoadScene("GameStartingScene");
    }
}