using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : AbstractStageObserver
{
    /**********
     * fields *
     **********/

    [SerializeField] private float distanceInteractionMax;
    [SerializeField] private LayerMask layer;

    private Camera playerCamera;
    private Image cursor;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "InteractionManager";

    // 클래스 인스턴스
    public static InteractionManager Instance { get; private set; }

    /************
     * messages *
     ************/

    void OnDrawGizmos()
    {
        if (playerCamera != null) {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distanceInteractionMax)) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(ray.origin, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.1f);
            } else {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(ray.origin, ray.direction * distanceInteractionMax);
            }
        }
    }

    void Update()
    {
        if (cursor != null) {
            if (PlayerManager.Instance.State == PlayerManager.PlayerState.Normal) {
                Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceInteractionMax, layer)) {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null && interactable.CanInteract(hit.distance)) {
                        cursor.gameObject.SetActive(true);
                        if (Input.GetMouseButtonDown(0)) {
                            interactable.OnInteract();
                        }

                        return;
                    }
                }
            }

            cursor.gameObject.SetActive(false);
        }
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = false;

        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            res = base.Awake_();
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera != null) {
            Log($"Find `Camera` success: {playerCamera.name}");
        } else {
            Log("Find `Camera` failed");
        }

        return res;
    }

    /*****************************************
     * implementation: AbstractStageObserver *
     *****************************************/

    // 단계 변경 시 불리는 메서드
    public override bool UpdateStage()
    {
        GameObject obj = GameObject.Find("Cursor");
        bool res = base.UpdateStage();

        if (obj != null) {
            cursor = obj.GetComponent<Image>();
            if (cursor != null) {
                Log("Find `Image` success");
            } else {
                Log("Find `Image` failed", mode: 1);
            }
        } else {
            Log("Find `Image` failed", mode: 1);
        }

        return res;
    }
}
