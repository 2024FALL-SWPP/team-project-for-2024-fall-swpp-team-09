using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anomaly26Controller : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    [Header("Fire Settings")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private int totalFiresToSpawn = 30;
    [SerializeField] private int maxConcurrentFires = 15;
    [SerializeField] private float timeLimit = 45.0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private float musicFadeTime = 1.0f;

    [Header("Spawn Settings")]
    [SerializeField] private float initialDelay = 10.0f;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float spawnRadius = 15.0f;

    private int totalFiresCreated = 0;

    /**************
     * properties *
     **************/

    public override string Name { get; } = "Anomaly26Controller";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        StartCoroutine(SpawnFireAsync());

        return res;
    }

    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        StopAllCoroutines();
        StartCoroutine(FadeOutMusic());

        return res;
    }

    /***************
     * new methods *
     ***************/

    private IEnumerator SpawnFireAsync()
    {
        float time;
        float timeStart;
        float timeNext;

        yield return new WaitForSeconds(initialDelay);

        timeStart = Time.time;
        timeNext = spawnInterval;

        StartCoroutine(FadeInMusic());
        CreateNewFire(GetRandomSpawnPosition());

        while ((time = Time.time - timeStart) < timeLimit) {
            int numFire = GameObject.FindObjectsByType<Fire>(FindObjectsSortMode.None).Length;

            if (totalFiresCreated < totalFiresToSpawn) {
                if (time >= timeNext) {
                    timeNext += spawnInterval;

                    if (numFire < maxConcurrentFires) {
                        CreateNewFire(GetRandomSpawnPosition());
                    }
                }
            } else {
                if (numFire == 0) {
                    GameManager.Instance.SetStageClear();

                    yield break;
                }
            }

            yield return null;
        }

        PlayerManager.Instance.GameOver();
        StartCoroutine(FadeOutMusic());
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
        RaycastHit hit;
        randomPos.y = 0;

        if (Physics.Raycast(randomPos + Vector3.up * 5f, Vector3.down, out hit, 10f)) {
            return hit.point;
        }

        return randomPos;
    }

    private IEnumerator FadeInMusic()
    {
        if (backgroundMusic != null) {
            float startTime = Time.time;
            float time;

            backgroundMusic.volume = 0f;
            backgroundMusic.Play();

            while ((time = Time.time - startTime) < musicFadeTime) {
                float progress = time / musicFadeTime;

                backgroundMusic.volume = Mathf.Lerp(0.0f, 0.4f, progress);

                yield return null;
            }

            backgroundMusic.volume = 0.4f;
        }
    }

    private IEnumerator FadeOutMusic()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying) {
            float startTime = Time.time;
            float time;

            backgroundMusic.volume = 0.4f;

            while ((time = Time.time - startTime) < musicFadeTime) {
                float progress = time / musicFadeTime;

                backgroundMusic.volume = Mathf.Lerp(0.4f, 0.0f, progress);

                yield return null;
            }

            backgroundMusic.Stop();
            backgroundMusic.volume = 0.4f;
        }
    }

    public void CreateNewFire(Vector3 position)
    {
        if (totalFiresCreated < totalFiresToSpawn) {
            GameObject newFire = Instantiate(firePrefab, position, Quaternion.identity);

            newFire.transform.parent = transform;
            totalFiresCreated++;
        }
    }
}
