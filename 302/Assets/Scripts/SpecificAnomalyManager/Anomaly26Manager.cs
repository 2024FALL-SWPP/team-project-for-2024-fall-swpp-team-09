using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anomaly26Manager : MonoBehaviour
{
   [Header("Fire Settings")]
   [SerializeField] private GameObject firePrefab;
   [SerializeField] private int totalFiresToSpawn = 30;  
   [SerializeField] private int maxConcurrentFires = 15;  
   [SerializeField] private float timeLimit = 30f;    
   
   [Header("Audio Settings")]
   [SerializeField] private AudioSource backgroundMusic;  
   [SerializeField] private float musicFadeTime = 1f;    
   
   [Header("Spawn Settings")]
   [SerializeField] private float initialDelay = 10f;    
   [SerializeField] private float spawnInterval = 1f;    
   [SerializeField] private float spawnRadius = 15f;     
   
   private bool isSpawningFires = false;
   private int totalFiresCreated = 0;
   private float timeSinceFirstFire = 0f;
   private bool timerStarted = false;

   private void OnEnable()
   {
       StartCoroutine(InitialFireSpawnDelay());
   }

   private void OnDisable()
   {
       StopAllCoroutines();
       isSpawningFires = false;
       StartCoroutine(FadeOutMusic());
   }

   private void Update()
   {
       if (totalFiresCreated > 0 && !timerStarted)
       {
           timerStarted = true;
       }

       if (timerStarted)
       {
           timeSinceFirstFire += Time.deltaTime;

           if (timeSinceFirstFire >= timeLimit && GameObject.FindObjectsOfType<Fire>().Length > 0)
           {
               GameOver();
           }
       }

       if (totalFiresCreated >= totalFiresToSpawn && 
           GameObject.FindObjectsOfType<Fire>().Length == 0)
       {
           StartCoroutine(FadeOutMusic());
           GameManager.Instance.SetStageClear();
           this.enabled = false;
       }
   }

    private IEnumerator InitialFireSpawnDelay()
    {
    yield return new WaitForSeconds(initialDelay);
    
    StartCoroutine(FadeInMusic());  // 첫 불과 함께 음악 시작
    CreateNewFire(GetRandomSpawnPosition());
    StartCoroutine(SpawnFireRoutine());
    }

   private IEnumerator SpawnFireRoutine()
   {
       isSpawningFires = true;
       
       while (isSpawningFires && totalFiresCreated < totalFiresToSpawn)
       {
           yield return new WaitForSeconds(spawnInterval);
           
           if (GameObject.FindObjectsOfType<Fire>().Length < maxConcurrentFires && 
               totalFiresCreated < totalFiresToSpawn)
           {
               CreateNewFire(GetRandomSpawnPosition());
           }
       }
   }

   private Vector3 GetRandomSpawnPosition()
   {
       Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
       randomPos.y = 0;

       RaycastHit hit;
       if (Physics.Raycast(randomPos + Vector3.up * 5f, Vector3.down, out hit, 10f))
       {
           return hit.point;
       }
       
       return randomPos;
   }

   private IEnumerator FadeInMusic()
   {
       if (backgroundMusic != null)
       {
           backgroundMusic.volume = 0f;
           backgroundMusic.Play();
           
           float startTime = Time.time;
           while (Time.time - startTime < musicFadeTime)
           {
               float progress = (Time.time - startTime) / musicFadeTime;
               backgroundMusic.volume = Mathf.Lerp(0f, 1f, progress);
               yield return null;
           }
           backgroundMusic.volume = 1f;
       }
   }

   private IEnumerator FadeOutMusic()
   {
       if (backgroundMusic != null && backgroundMusic.isPlaying)
       {
           float startVolume = backgroundMusic.volume;
           float startTime = Time.time;
           
           while (Time.time - startTime < musicFadeTime)
           {
               float progress = (Time.time - startTime) / musicFadeTime;
               backgroundMusic.volume = Mathf.Lerp(startVolume, 0f, progress);
               yield return null;
           }
           
           backgroundMusic.Stop();
           backgroundMusic.volume = startVolume;
       }
   }

   public void CreateNewFire(Vector3 position)
   {
       if (totalFiresCreated >= totalFiresToSpawn)
           return;

       GameObject newFire = Instantiate(firePrefab, position, Quaternion.identity);
       newFire.transform.parent = transform;
       totalFiresCreated++;
   }

   private void GameOver()
   {
       StartCoroutine(FadeOutMusic());
       //GameManager.Instance.GameOver();
       this.enabled = false;
   }
}