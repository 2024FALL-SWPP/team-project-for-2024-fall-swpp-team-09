using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    private Camera mainCamera;
    private Material fadeMaterial;
    private float currentAlpha = 0f;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // 페이드 효과를 위한 마테리얼 생성
        Shader fadeShader = Shader.Find("Hidden/Internal-Colored");
        fadeMaterial = new Material(fadeShader);
        fadeMaterial.hideFlags = HideFlags.HideAndDontSave;
        fadeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        fadeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        fadeMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        fadeMaterial.SetInt("_ZWrite", 0);
        fadeMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.depthTextureMode = DepthTextureMode.Depth;
        }
        Camera.onPostRender += OnPostRenderCallback;
    }

    private void OnDisable()
    {
        Camera.onPostRender -= OnPostRenderCallback;
    }

    public void StartFade(float targetAlpha, float duration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = currentAlpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            // 선형 보간으로 변경하여 더 일관된 페이드 효과
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            
            yield return null;
        }

        currentAlpha = targetAlpha; // 확실하게 목표값으로 설정
        
        // 완전히 검은색일 때는 정확히 1로 설정
        if (targetAlpha == 1f)
        {
            currentAlpha = 1f;
        }
    }

    private void OnPostRenderCallback(Camera cam)
    {
        if (cam != mainCamera || fadeMaterial == null) return;

        GL.PushMatrix();
        GL.LoadOrtho();
        
        fadeMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        // 완전한 검은색으로 설정
        GL.Color(new Color(0, 0, 0, currentAlpha));
        
        // 전체 화면을 덮는 쿼드 그리기
        GL.Vertex3(-1, -1, 0);
        GL.Vertex3(1, -1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(-1, 1, 0);
        
        GL.End();
        GL.PopMatrix();
    }

    private void Update()
    {
        // 메인 카메라 참조가 깨졌을 경우 다시 찾기
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera != null)
            {
                mainCamera.depthTextureMode = DepthTextureMode.Depth;
            }
        }
    }

    private void OnDestroy()
    {
        if (fadeMaterial != null)
        {
            DestroyImmediate(fadeMaterial);
        }
    }
}