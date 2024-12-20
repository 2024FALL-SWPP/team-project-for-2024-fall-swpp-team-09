using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideGirl : InteractableObject
{
   [Header("Side Girl Settings")]
   
   private MeshRenderer meshRenderer;
   private bool hasInteracted = false; 
   private Anomaly5Manager anomalyManager;

   private void Awake()
   {
       meshRenderer = GetComponent<MeshRenderer>();
       // Anomaly5Manager 찾기
       anomalyManager = FindObjectOfType<Anomaly5Manager>();
   }

   public override void OnInteract()
   {
        if (hasInteracted) return;
        if (anomalyManager != null){
        anomalyManager.StopAnomalyMusic();
        }
        hasInteracted = true;
        StartCoroutine(FadeOutAllMeshRenderers(gameObject));
        GameManager.Instance.SetStageClear();
   }

   private IEnumerator FadeOutAllMeshRenderers(GameObject target)
    {
        // 모든 하위 MeshRenderer 컴포넌트들을 찾음
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
        float fadeTime = 2f; // 페이드 아웃 시간
        float elapsedTime = 0f;

        // 시작할 때의 모든 머티리얼 저장
        Material[] originalMaterials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            // 각 MeshRenderer의 머티리얼을 복제하여 저장
            originalMaterials[i] = new Material(meshRenderers[i].material);
            // 투명도를 사용하기 위해 셰이더 모드 변경
            meshRenderers[i].material.SetFloat("_Mode", 3f);
            meshRenderers[i].material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            meshRenderers[i].material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            meshRenderers[i].material.SetInt("_ZWrite", 0);
            meshRenderers[i].material.DisableKeyword("_ALPHATEST_ON");
            meshRenderers[i].material.EnableKeyword("_ALPHABLEND_ON");
            meshRenderers[i].material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            meshRenderers[i].material.renderQueue = 3000;
        }

        // 페이드 아웃 실행
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - (elapsedTime / fadeTime);

            // 모든 MeshRenderer의 알파값 업데이트
            foreach (MeshRenderer renderer in meshRenderers)
            {
                Color color = renderer.material.color;
                color.a = alpha;
                renderer.material.color = color;
            }

            yield return null;
        }

        // 완전히 투명해지면 오브젝트 비활성화
        target.SetActive(false);

    }
}
