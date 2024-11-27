using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private float interactionRange = 6f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Image interactionCursor;

    private Camera playerCamera;
    private IInteractable currentInteractable;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if(playerCamera == null)
            Debug.LogError("Camera not found!");
            
        if(interactionCursor == null)
            Debug.LogError("Cursor image not assigned!");
        else
            interactionCursor.gameObject.SetActive(false);

        // 레이어마스크 값 확인
        Debug.Log($"LayerMask value: {interactableLayer.value}");
    }

    private void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        // 모든 레이어에 대한 레이캐스트 먼저 체크
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Debug.Log($"Hit something: {hit.collider.gameObject.name} on layer: {hit.collider.gameObject.layer}");
        }

        // Interactable 레이어에 대한 레이캐스트
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            // Debug.Log($"Hit interactable: {hit.collider.gameObject.name}");
            
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            // modified by 신채환
            // CanInteract 메서드가 거리를 인자로 받도록 변경
            if (interactable != null && interactable.CanInteract(hit.distance))
            {
                currentInteractable = interactable;
                interactionCursor.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    currentInteractable.OnInteract();
                }
                return;
            }
        }

        // 아무것도 못 찾았을 때
        if (currentInteractable != null || interactionCursor.gameObject.activeSelf)
        {
            currentInteractable = null;
            interactionCursor.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * interactionRange);
        }
    }
}