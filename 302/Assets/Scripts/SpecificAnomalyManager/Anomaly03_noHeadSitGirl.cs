using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class Anomaly03_noHeadSitGirl : AbstractAnomalyInteractable
{
    // 클래스 이름
    public override string Name { get; } = "Anomaly03_noHeadSitGirl"; 

    private Transform cameraTransform;  // Reference to the Main Camera's transform
    private AudioSource audioSource;    // Reference to the AudioSource component
    private float distanceToCamera;     // Distance between player and anomaly object

    // 상호작용 시 실행될 메서드
    public virtual void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");

        // Code used before `GameManager` updates begin
        GameObject controllerObject = GameObject.Find("AnomalyManager (3)(Clone)");
        AbstractAnomalyObject controller = controllerObject.GetComponent<AbstractAnomalyObject>();

        Log($"Call `{controller.Name}.ResetAnomaly` begin");
        if (controller.ResetAnomaly()) {
            Log($"Call `{controller.Name}.ResetAnomaly` success");
        } else {
            Log($"Call `{controller.Name}.ResetAnomaly` failed", mode: 1);
        }
        // Code used before `GameManager` updates end
    }

    // 현재 상호작용 가능한지 여부 반환
    public virtual bool CanInteract(float distance)
    {
        bool res = base.CanInteract(distance);

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the prefab.");
            res = false;
        }

        return res;
    }

    private void Update()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found! Ensure the main camera has the 'MainCamera' tag.");
        }
        
        distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);

        CheckAndPlayAudio();
        
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        StartCoroutine(FadeOut(gameObject, 2f));  // Fades out over 2 seconds

        return res;
    }

    private void CheckAndPlayAudio()
    {
        if (distanceToCamera <= 5f)
        {
            if (!audioSource.isPlaying)  // Start playing if not already playing
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)  // Stop playing if out of range
            {
                audioSource.Stop();
            }
        }
    }

    private IEnumerator FadeOut(GameObject obj, float duration)
    {
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
