using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Anomaly14Manager : MonoBehaviour
{
    public GameObject SweaterSitGirlPrefab;
    public GameObject DifferentPrefab; // 다른 종류의 프리팹
    public float yRotation = 180f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 기존의 floatable 태그 오브젝트들 제거
        GameObject[] floatableObjects = GameObject.FindGameObjectsWithTag("floatable");
        foreach (GameObject obj in floatableObjects)
        {
            Debug.Log($"Destroying floatable object: {obj.name}");
            Destroy(obj);
        }

        // 프리팹들을 그리드 형태로 배치
        Vector3 startPosition = new Vector3(-17.5f, -.76f, -12.5f);
        float spacing = 3f;
        int rowCount = 8;    // z축 방향 줄 수
        int columnCount = 11; // x축 방향 줄 수

        int totalCount = rowCount * columnCount;
        int randomIndex = Random.Range(0, totalCount);
        
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                Vector3 position = startPosition + new Vector3(col * spacing, 0f, row * spacing);
                Quaternion rotation = Quaternion.Euler(0f, yRotation, 0f);

                // 현재 인덱스 계산
                int currentIndex = row * columnCount + col;

                // 랜덤으로 선택된 위치면 다른 프리팹을, 아니면 기본 프리팹을 생성
                if (currentIndex == randomIndex)
                {
                    Instantiate(DifferentPrefab, position, rotation);
                    Debug.Log($"Different prefab spawned at position: {position}");
                }
                else
                {
                    Instantiate(SweaterSitGirlPrefab, position, rotation);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}