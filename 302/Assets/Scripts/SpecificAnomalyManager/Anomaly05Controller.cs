using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly05Controller : AbstractAnomalyObject
{
    public override string Name { get; } = "Anomaly05Controller";
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject sideGirlPrefab;
    [SerializeField] private AudioSource audioSource;
    private GameObject sideGirl;

    protected override bool InitFields() 
    {
        bool res = base.InitFields();

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
        
        return res;
    }

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        StartCoroutine(PlayMusicAfterDelay());

        return res;
    }

    public override bool ResetAnomaly()
    {
        bool res = base.StartAnomaly();

        StopAnomalyMusic();

        return res;
    }

    IEnumerator PlayMusicAfterDelay() 
    {
        yield return new WaitForSeconds(15f);
        if (sideGirl != null)
        {
            // Anomaly5Manager 게임오브젝트를 사이드걸 위치로 이동
            transform.position = sideGirl.transform.position;
            audioSource.Play();
        }
    }

    public void StopAnomalyMusic() 
    {
        if (audioSource != null && audioSource.isPlaying) 
        {
            audioSource.Stop();
        }
    }
}
