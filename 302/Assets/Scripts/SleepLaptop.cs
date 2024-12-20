using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepLaptop : InteractableObject
{
    [Header("Laptop Settings")]
    [SerializeField] private string sleepPrompt = "노트북으로 잠자기";
    public bool setStageClear = false;

    private void Awake()
    {
        // 상호작용 메시지 설정
        promptMessage = sleepPrompt;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        
        if (setStageClear) {
            GameManager.Instance.SetStageClear();
        }

        PlayerManager.Instance.Sleep();
    }
}