using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly17Manager : MonoBehaviour
{
    public GameObject splitMicLinePrefab; // line_split prefab
    public GameObject normalMicLinePrefab; // line_normal prefab
    private GameObject currentMicLine; // line_split OR line_normal 중 현재 마이크 선 obejct
    public AudioClip electricSparkSoundClip;

    // 기존 line_normal 값들
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private Vector3 savedScale;
    private Transform savedParent; // mic_line_normal

    void Start()
    {
        ReplaceToSplitMic();
    }

    public void ReplaceToSplitMic()
    {
        SaveNormalLineValues(); // 차후 복원을 위한 기존 line_normal 관련 값 저장
        InstantiateSplitLine(); // line_split으로 대체

        Destroy(GameObject.Find("line_normal"));
    }

    private void SaveNormalLineValues()
    {
        currentMicLine = GameObject.Find("line_normal");
        if (currentMicLine == null) {
            return;
        }
        
        Transform oldTransform = currentMicLine.transform;
        savedPosition = oldTransform.position;
        savedRotation = oldTransform.rotation;
        savedScale = oldTransform.localScale;
        savedParent = oldTransform.parent;
    }

    private void InstantiateSplitLine()
    {
        GameObject newMicLine = Instantiate(splitMicLinePrefab, savedPosition, savedRotation);
        currentMicLine = newMicLine;

        newMicLine.transform.parent = savedParent;
        newMicLine.transform.localScale = new Vector3(-3.5f, -3.5f, -3.5f);
        newMicLine.AddComponent<BoxCollider>();
        newMicLine.layer = 3;

        // script 및 soundclip assign
        Anomaly17_mic micScript = newMicLine.AddComponent<Anomaly17_mic>();
        micScript.electricSparkSoundClip = electricSparkSoundClip;
    }

    public void ReplaceToNormalMic()
    {
        GameObject newMicLine = Instantiate(normalMicLinePrefab, savedPosition, savedRotation);

        newMicLine.transform.parent = savedParent;
        newMicLine.transform.localScale = savedScale;
        newMicLine.AddComponent<BoxCollider>();

        Destroy(currentMicLine);
        currentMicLine = newMicLine;
    }
}
