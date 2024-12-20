using UnityEngine;
using System.Collections;

public class Anomaly10_abnormalTile : AbstractAnomalyInteractable 
{
    private AudioSource audioSource;  // Reference to the AudioSource component

    // 클래스 이름
    public override string Name { get; } = "Anomaly10_abnormalTile"; 

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // Try to get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing! Please attach an AudioSource to this object.");
            res = false;
        }

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        StartCoroutine(FadeOut(gameObject, 2f));  // Fades out over 2 seconds and then clears stage
    
        return res;
    }

    // Coroutine to gradually fade out, destroy the object, and set the stage clear
    private IEnumerator FadeOut(GameObject obj, float duration)
    {
        // Play the sound once when the object starts fading out
        if (audioSource != null)
        {
            audioSource.Play();  // Play the audio clip attached to the AudioSource
        }
        
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = alpha;
                        mat.color = color;
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);  // Remove the object completely after fading out

    }
}