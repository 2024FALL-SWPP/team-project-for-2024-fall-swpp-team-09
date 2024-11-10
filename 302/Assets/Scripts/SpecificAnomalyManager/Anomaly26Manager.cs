using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anomaly26Manager : MonoBehaviour
{
   [Header("Fire Settings")]
   [SerializeField] private GameObject firePrefab;
   [SerializeField] private int totalFiresToSpawn = 20;  
   [SerializeField] private int maxConcurrentFires = 5;  
   
   [Header("Audio Settings")]
   [SerializeField] private AudioSource backgroundMusic;  
   [SerializeField] private float musicFadeTime = 1f;    
   
   [Header("Spawn Settings")]
   [SerializeField] private float initialDelay = 10f;    
   [SerializeField] private float spawnInterval = 3f;    
   [SerializeField] private float spawnRadius = 10f;     
   
   private List<Fire> activeFires = new List<Fire>();
   private bool isSpawningFires = false;
   private int totalFiresCreated = 0;                    
   private int totalFiresExtinguished = 0;               

   private void OnEnable()
   {
       totalFiresCreated = 0;
       totalFiresExtinguished = 0;
       StartCoroutine(InitialFireSpawnDelay());
   }

   private void OnDisable()
   {
       StopAllCoroutines();
       isSpawningFires = false;
       StartCoroutine(FadeOutMusic());
   }

   private IEnumerator InitialFireSpawnDelay()
   {
       yield return new WaitForSeconds(initialDelay);
       StartCoroutine(FadeInMusic());
       CreateNewFire(GetRandomSpawnPosition());
       StartCoroutine(SpawnFireRoutine());
   }

   private IEnumerator SpawnFireRoutine()
   {
       isSpawningFires = true;
       
       while (isSpawningFires && totalFiresCreated < totalFiresToSpawn)
       {
           yield return new WaitForSeconds(spawnInterval);
           
           if (activeFires.Count < maxConcurrentFires && totalFiresCreated < totalFiresToSpawn)
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
           backgroundMusic.volume = startVolume;  // 볼륨 초기화
       }
   }

   public void RegisterFire(Fire fire)
   {
       if (!activeFires.Contains(fire))
       {
           activeFires.Add(fire);
       }
   }

   public void UnregisterFire(Fire fire)
   {
       activeFires.Remove(fire);
       totalFiresExtinguished++;
       
       // 모든 불이 생성되었고, 모든 불이 꺼졌을 때 스테이지 클리어
       if (totalFiresExtinguished >= totalFiresToSpawn)
       {
           StartCoroutine(FadeOutMusic());
           GameManager.Instance.SetStageClear();
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

   private void OnDrawGizmosSelected()
   {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, spawnRadius);
   }
}