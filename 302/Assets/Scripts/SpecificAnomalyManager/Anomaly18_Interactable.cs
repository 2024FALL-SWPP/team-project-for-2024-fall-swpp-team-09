using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly18_Interactable : InteractableObject
{
    /*************
     * constants *
     *************/

    private string NAME = "Anomaly18_Interactable";

    /**************
     * properties *
     **************/

    // 해당 오브젝트를 생성한 이상현상 매니저
    public Anomaly18Manager Manager { get; set; }

    /**********************
     * overridden methods *
     **********************/

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        if (canInteract) {
            canInteract = false;

            base.OnInteract();

            if (Manager != null) {
                Debug.Log($"[{NAME}] Call `Anomaly18Manager.InteractionSuccess`");
                Manager.InteractionSuccess();
            } else {
                Debug.LogWarning($"[{NAME}] `Manager` is not initialized.");
            }
        }
    }
}
