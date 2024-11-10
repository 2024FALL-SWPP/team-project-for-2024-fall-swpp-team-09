using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableLaptop : InteractableObject
{
    [Header("Laptop Settings")]
    [SerializeField] private string sleepPrompt = "노트북으로 잠자기";
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        // 상호작용 메시지 설정
        promptMessage = sleepPrompt;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        
        // playerController가 null이 아닌지 확인
        if (playerController != null)
        {
            playerController.Sleep();
        }
        else
        {
            Debug.LogError("PlayerController가 설정되지 않았습니다. Inspector에서 설정해주세요!");
        }
    }
}