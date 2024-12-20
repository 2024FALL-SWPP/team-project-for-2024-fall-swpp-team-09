using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly5Manager : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject sideGirlPrefab;
    [SerializeField] private AudioSource audioSource;
    private GameObject sideGirl;

    void Start() 
    {
        sideGirl = GameObject.FindGameObjectWithTag("sideGirl");
        if(sideGirl != null) 
        {
            Vector3 newPosition = sideGirl.transform.position;
            newPosition.x += moveDistance;
            sideGirl.transform.position = newPosition;
        }
        
        if(sideGirlPrefab != null) 
        {
            Instantiate(sideGirlPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
        }
        
        StartCoroutine(PlayMusicAfterDelay());
    }

    IEnumerator PlayMusicAfterDelay() 
    {
        yield return new WaitForSeconds(15f);
        if(sideGirl != null)
        {
            // Anomaly5Manager 게임오브젝트를 사이드걸 위치로 이동
            transform.position = sideGirl.transform.position;
            audioSource.Play();
        }
    }

    public void StopAnomalyMusic() 
    {
        if(audioSource != null && audioSource.isPlaying) 
        {
            audioSource.Stop();
        }
    }
}