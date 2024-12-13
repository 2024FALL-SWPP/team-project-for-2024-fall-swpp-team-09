using UnityEngine;
using System.Collections;

public class Anomaly08_micsound : AbstractAnomalyInteractable
{

    private Transform playerTransform;
    private AudioSource audioSource;

    public float maxVolumeDistance = 5f;      // Distance at which the audio reaches max volume
    public float startDistance = 30f;         // Maximum distance where the audio starts being audible
    public float fadeOutDuration = 2f;        // Duration for the fade-out effect after interaction
    public float audioStartDelay = 5f;        // Delay in seconds before the audio starts playing

    private bool isAudioActive = false;

    // 클래스 이름
    public override string Name { get; } = "Anomaly08_micsound";

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");

        // Code used before `GameManager` updates begin
        GameObject controllerObject = GameObject.Find("AnomalyManager (8)(Clone)");
        AbstractAnomalyObject controller = controllerObject.GetComponent<AbstractAnomalyObject>();

        Log($"Call `{controller.Name}.ResetAnomaly` begin");
        if (controller.ResetAnomaly()) {
            Log($"Call `{controller.Name}.ResetAnomaly` success");
        } else {
            Log($"Call `{controller.Name}.ResetAnomaly` failed", mode: 1);
        }
        // Code used before `GameManager` updates end

    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        GameObject player = GameObject.FindWithTag("MainCamera");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found! Make sure the main camera has the 'MainCamera' tag.");
            res = false;
        }

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;  // Ensure the sound loops continuously until interaction
            audioSource.volume = 0f;
        }
        else
        {
            Debug.LogError("AudioSource component is missing on Anomaly8_micsound.");
            res = false;
        }
        return res;
    }

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        StartCoroutine(DelayedAudiosStart());

        return res;
    }

    private void Update()
    {
        // Calculate the distance between the player and this object
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Adjust the volume based on the distance, from 0 at `startDistance` to 1 at `maxVolumeDistance`
        if (distanceToPlayer <= startDistance)
        {
            audioSource.volume = Mathf.Clamp01(1 - ((distanceToPlayer - maxVolumeDistance) / (startDistance - maxVolumeDistance)));
        }
        else
        {
            audioSource.volume = 0f;  // Ensure volume is zero if outside the starting distance
        }
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        StartCoroutine(FadeOutAndStop());  // Start fade-out coroutine upon interaction
        

        return res;
    }

    private IEnumerator DelayedAudiosStart()
    {
        yield return new WaitForSeconds(audioStartDelay);
        isAudioActive = true;
        audioSource.Play();
        Debug.Log("Audio playback started after delay.");
        
    }

    // Coroutine to gradually fade out the volume and stop the audio
    private IEnumerator FadeOutAndStop()
    {
        Debug.Log("Microphone sound interaction triggered: Sound fading out.");
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);  // Gradually reduce volume
            yield return null;
        }

        audioSource.Stop();  // Stop the audio playback completely
        //audioSource.volume = startVolume;  // Reset volume for potential reuse of this script
    }
}