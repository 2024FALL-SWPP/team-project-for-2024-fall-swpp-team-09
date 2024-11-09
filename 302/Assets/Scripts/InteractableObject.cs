using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] protected string promptMessage = "클릭하여 상호작용";
    [SerializeField] protected float interactionRange = 2f;
    protected bool canInteract = true;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;  // 레이캐스트 감지를 위한 일반 콜라이더
    }

    public virtual string GetInteractionPrompt()
    {
        return promptMessage;
    }

    public virtual void OnInteract()
    {
        Debug.Log($"Interacting with {gameObject.name}");
    }

    public virtual bool CanInteract()
    {
        return canInteract;
    }
}