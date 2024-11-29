using UnityEngine;
using UnityEngine.Video;

public class Anomaly30_thunderstorm : MonoBehaviour
{
    public VideoClip videoClip; // Assign the video clip via the manager
    public AudioClip audioClip; // Assign the audio clip via the manager
    public Vector3 screenPosition = new Vector3(-2.51f, 4.72f, 18.91f); // Position for the thunderstorm screen
    public Vector3 screenScale = new Vector3(46.9397f, 10.97118f, 1.538f); // Scale for the thunderstorm screen

    private GameObject thunderstormScreen; // The Quad for the thunderstorm screen
    private AudioSource audioSource; // AudioSource to play the thunderstorm sound
    private VideoPlayer videoPlayer; // VideoPlayer to display the video

    public void CreateThunderstormScreen()
    {
        // Create a Quad for the thunderstorm screen
        thunderstormScreen = GameObject.CreatePrimitive(PrimitiveType.Quad);
        thunderstormScreen.name = "ThunderstormScreen";

        // Set position, rotation, and scale
        thunderstormScreen.transform.position = screenPosition;
        thunderstormScreen.transform.eulerAngles = new Vector3(0, 0, 0); // Ensure the screen faces upright
        thunderstormScreen.transform.localScale = screenScale;

        // Remove the MeshCollider (not needed for this Quad)
        Destroy(thunderstormScreen.GetComponent<MeshCollider>());

        // Set up the VideoPlayer for thunderstorm video
        videoPlayer = thunderstormScreen.AddComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        videoPlayer.targetMaterialRenderer = thunderstormScreen.GetComponent<Renderer>();
        videoPlayer.targetMaterialProperty = "_MainTex";
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip; // Assign the video clip
        videoPlayer.isLooping = true; // Set the video to loop
        videoPlayer.playOnAwake = false; // Prevent immediate playback until explicitly started

        // Play the video if clip is assigned
        if (videoClip != null)
        {
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("No VideoClip assigned to the thunderstorm.");
        }

        // Set up the AudioSource for thunderstorm audio
        audioSource = thunderstormScreen.AddComponent<AudioSource>();
        audioSource.clip = audioClip; // Assign the audio clip
        audioSource.loop = true; // Set the audio to loop
        audioSource.playOnAwake = false; // Prevent immediate playback until explicitly started
        audioSource.volume = 0.7f; // Adjust the volume (0.0 to 1.0)

        // Play the audio if clip is assigned
        if (audioClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioClip assigned to the thunderstorm.");
        }
    }

    public void DestroyThunderstormScreen()
    {
        // Stop and remove the VideoPlayer
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            Destroy(videoPlayer);
        }

        // Stop and remove the AudioSource
        if (audioSource != null)
        {
            audioSource.Stop();
            Destroy(audioSource);
        }

        // Destroy the thunderstorm screen
        if (thunderstormScreen != null)
        {
            Destroy(thunderstormScreen);
        }
    }
}
