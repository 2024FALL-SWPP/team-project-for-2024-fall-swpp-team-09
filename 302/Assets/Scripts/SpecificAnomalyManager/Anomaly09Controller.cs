using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anomaly09Controller : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    [Header("Anomaly Settings")]
    public float anomalyDelay = 15f;

    [Header("Audio Settings")]
    public AudioClip anomalyMusic;
    public float musicVolume = 1f;
    public bool fadeInMusic = true;
    public float fadeInDuration = 2f;
    private AudioSource audioSource;

    [Header("Scene Light Settings")]
    public float dimmedIntensity = 0.3f;
    private Light[] sceneLights;
    private float[] originalIntensities;

    [Header("Main Spotlight Settings")]
    public GameObject mainSpotlightPrefab;  // MainSpotlight 프리팹
    public Vector3 spotlightPosition;       // 스포트라이트 생성 위치
    public float spotlightRotation;         // 스포트라이트 Y축 회전값
    private GameObject spawnedSpotlight;    // 생성된 스포트라이트 참조

    [Header("Dancing Girl Settings")]
    public GameObject dancingGirlPrefab;

    [Header("Anomaly Light Settings")]
    public GameObject lightPrefab;
    public int numberOfLights = 4;
    public float radius = 5f;
    public float lightHeight = 5f;

    [Header("Spawn Positions")]
    public Vector3 position1;
    public float rotation1;
    public Vector3 position2;
    public float rotation2;
    public Vector3 position3;
    public float rotation3;
    public Vector3 position4;
    public float rotation4;
    public Vector3 position5;
    public float rotation5;
    public Vector3 position6;
    public float rotation6;
    public Vector3 position7;
    public float rotation7;
    public Vector3 position8;
    public float rotation8;
    public Vector3 position9;
    public float rotation9;
    public Vector3 position10;
    public float rotation10;

    private List<GameObject> spawnedGirls = new List<GameObject>();
    private List<GameObject> spawnedLights = new List<GameObject>();
    private Coroutine anomalyCoroutine;

    /**************
     * properties *
     **************/

    public override string Name { get; } = "Anomaly09Controller";

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    protected override bool InitFields()
    {
        bool res = base.InitFields();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = anomalyMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        sceneLights = FindScenePointLights();
        SaveOriginalIntensities();

        return res;
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        anomalyCoroutine = StartCoroutine(StartAnomalyWithDelay());

        return res;
    }

    /***************
     * new methods *
     ***************/

    IEnumerator StartAnomalyWithDelay()
    {
        yield return new WaitForSeconds(anomalyDelay);
        
        DimSceneLights();
        SpawnLights();
        SpawnDancingGirls();
        if (mainSpotlightPrefab != null) {
            spawnedSpotlight = Instantiate(
                mainSpotlightPrefab, spotlightPosition,
                Quaternion.Euler(90f, spotlightRotation, -90f)
            );
        }
        
        if (fadeInMusic) {
            StartCoroutine(FadeInMusic());
        } else {
            PlayAnomalyMusic();
        }

        Debug.Log("이상현상 발생 완료");
    }

    Light[] FindScenePointLights()
    {
        Light[] allLights = FindObjectsOfType<Light>();
        List<Light> pointLights = new List<Light>();

        foreach (Light light in allLights) {
            if (light.type == LightType.Point) {
                pointLights.Add(light);
                Debug.Log($"Found Point Light: {light.gameObject.name}");
            }
        }

        return pointLights.ToArray();
    }

    void SaveOriginalIntensities()
    {
        if (sceneLights != null) {
            originalIntensities = new float[sceneLights.Length];
            for (int i = 0; i < sceneLights.Length; i++) {
                if (sceneLights[i] != null) {
                    originalIntensities[i] = sceneLights[i].intensity;
                }
            }
        }
    }

    void DimSceneLights()
    {
        if (sceneLights != null) {
            foreach (Light light in sceneLights) {
                if (light != null) {
                    light.intensity = dimmedIntensity;
                }
            }
        }
    }

    void RestoreOriginalLightIntensities()
    {
        if (sceneLights != null && originalIntensities != null) {
            for (int i = 0; i < sceneLights.Length; i++) {
                if (sceneLights[i] != null) {
                    sceneLights[i].intensity = originalIntensities[i];
                }
            }
        }
    }

    void SpawnLights()
    {
        if (lightPrefab == null) {
            return;
        }

        foreach (GameObject light in spawnedLights) {
            if (light != null) {
                Destroy(light);
            }
        }

        spawnedLights.Clear();

        for (int i = 0; i < numberOfLights; i++) {
            float angle = i * (360f / numberOfLights);
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            
            Vector3 position = transform.position + new Vector3(x, lightHeight, z);
            GameObject light = Instantiate(lightPrefab, position, Quaternion.identity);
            
            light.transform.LookAt(new Vector3(transform.position.x, lightHeight, transform.position.z));
            light.transform.Rotate(90f, 0f, 0f);
            
            spawnedLights.Add(light);
        }
    }

    void SpawnDancingGirls()
    {
        if (dancingGirlPrefab == null) {
            return;
        }

        foreach (GameObject girl in spawnedGirls) {
            if (girl != null) {
                Destroy(girl);
            }
        }
 
        spawnedGirls.Clear();

        SpawnGirl(position1, rotation1);
        SpawnGirl(position2, rotation2);
        SpawnGirl(position3, rotation3);
        SpawnGirl(position4, rotation4);
        SpawnGirl(position5, rotation5);
        SpawnGirl(position6, rotation6);
        SpawnGirl(position7, rotation7);
        SpawnGirl(position8, rotation8);
        SpawnGirl(position9, rotation9);
        SpawnGirl(position10, rotation10);
    }

    void SpawnGirl(Vector3 position, float yRotation)
    {
        GameObject girl = Instantiate(
            dancingGirlPrefab, position, Quaternion.Euler(0, yRotation, 0)
        );

        spawnedGirls.Add(girl);
    }

    void PlayAnomalyMusic()
    {
        if (anomalyMusic != null && audioSource != null) {
            audioSource.volume = musicVolume;
            audioSource.Play();
        }
    }

    IEnumerator FadeInMusic()
    {
        if (anomalyMusic != null && audioSource != null) {
            audioSource.volume = 0f;
            audioSource.Play();

            float currentTime = 0;
            while (currentTime < fadeInDuration) {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, musicVolume, currentTime / fadeInDuration);
                yield return null;
            }
            audioSource.volume = musicVolume;
        }
    }

    void StopAnomalyMusic()
    {
        if (audioSource != null && audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

    void CleanupSpawnedObjects()
    {
        foreach (GameObject girl in spawnedGirls) {
            if (girl != null) Destroy(girl);
        }

        spawnedGirls.Clear();

        foreach (GameObject light in spawnedLights) {
            if (light != null) Destroy(light);
        }

        spawnedLights.Clear();
    }
}

