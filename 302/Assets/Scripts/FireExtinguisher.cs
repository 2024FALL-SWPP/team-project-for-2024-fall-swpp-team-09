using UnityEngine;

public class FireExtinguisher : InteractableObject
{
    [Header("Fire Extinguisher Settings")]
    [SerializeField] private Vector3 holdPosition = new Vector3(0.5f, -0.3f, 0.7f);
    [SerializeField] private Vector3 holdRotation = new Vector3(0f, -90f, 0f);
    [SerializeField] private float smoothTime = 0.1f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isHeld = false;
    private Transform playerCameraTransform;
    private Rigidbody rb;
    private Collider coll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public override string GetInteractionPrompt()
    {
        return isHeld ? "우클릭으로 내려놓기" : "클릭하여 소화기 들기";
    }

    public override void OnInteract()
    {
        if (!isHeld)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            isHeld = true;
            playerCameraTransform = player.GetComponentInChildren<Camera>().transform;
            
            rb.isKinematic = true;
            coll.enabled = false;

            transform.SetParent(playerCameraTransform);
            
            player.SetHeldItem(this);
        }
    }

    public void PutDown()
    {
        if (!isHeld) return;

        isHeld = false;
        transform.SetParent(null);
        
        rb.isKinematic = false;
        coll.enabled = true;
        
        transform.position = playerCameraTransform.position + playerCameraTransform.forward * 1f;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SetHeldItem(null);
        }
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector3 targetPosition = playerCameraTransform.TransformPoint(holdPosition);
            Quaternion targetRotation = playerCameraTransform.rotation * Quaternion.Euler(holdRotation);
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime);

            if (Input.GetMouseButtonDown(1))
            {
                PutDown();
            }
        }
    }

    public override bool CanInteract()
    {
        return !isHeld;
    }
}