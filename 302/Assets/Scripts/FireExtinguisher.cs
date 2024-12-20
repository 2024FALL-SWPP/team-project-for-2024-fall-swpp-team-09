using UnityEngine;
using UnityEngine.SceneManagement;

public class FireExtinguisher : InteractableObject
{
    [Header("Fire Extinguisher Settings")]
    [SerializeField] private Vector3 holdPosition = new Vector3(0.5f, -0.45f, 0.7f);
    [SerializeField] private Vector3 holdRotation = new Vector3(-90f, -90f, 180f);
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Extinguisher Components")]
    [SerializeField] private GameObject nozzle;
    [SerializeField] private float extinguishRadius = 3f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isHeld = false;
    private Transform playerCameraTransform;
    private Rigidbody rb;
    private Collider coll;
    private LayerMask fireLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        
        if (nozzle) nozzle.SetActive(false);
        fireLayer = LayerMask.GetMask("Fire");
    }

    private void Update()
    {
        if (isHeld)
        {
            Vector3 targetPosition = playerCameraTransform.TransformPoint(holdPosition);
            Quaternion targetRotation = playerCameraTransform.rotation * Quaternion.Euler(holdRotation);
            
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothTime);

            CheckFireCollision();

            if (Input.GetMouseButtonDown(1))
            {
                PutDown();
            }
        }
    }

    private void CheckFireCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            nozzle.transform.position, 
            extinguishRadius, 
            fireLayer
        );

        foreach (Collider col in hitColliders)
        {
            Fire fire = col.GetComponent<Fire>();
            if (fire != null)
            {
                fire.StartExtinguishing(); // Extinguish() 대신 StartExtinguishing() 호출
            }
        }
    }

    private void PickUp()
    {
        PlayerManager player = PlayerManager.Instance;
        if (player != null)
        {
            isHeld = true;
            playerCameraTransform = player.GetComponentInChildren<Camera>().transform;
            
            rb.isKinematic = true;
            coll.enabled = false;

            transform.SetParent(playerCameraTransform);
            
            if (nozzle) nozzle.SetActive(true);
            player.SetHeldItem(this);
        }
    }

    public void PutDown()
    {
        if (!isHeld) return;

        isHeld = false;
        transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        rb.isKinematic = false;
        coll.enabled = true;
        
        if (nozzle) nozzle.SetActive(false);
        
        transform.position = playerCameraTransform.position + playerCameraTransform.forward * 1f;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        
        PlayerManager player = PlayerManager.Instance;
        if (player != null)
        {
            player.SetHeldItem(null);
        }
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

    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경
    public override bool CanInteract(float distance)
    {
        return !isHeld;
    }

    private void OnDrawGizmosSelected()
    {
        if (nozzle)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(nozzle.transform.position, extinguishRadius);
        }
    }
}