using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Anomaly11_openeddoor : AbstractAnomalyInteractable 
{

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed;
    public float closeSpeed;  // Door movement speed

    private Transform movingPart;    // Reference to Cube.002 child object
    private AudioSource audioSource; // Reference to AudioSource component

    // 클래스 이름
    public override string Name { get; } = "Anomaly11_openeddoor"; // TODO: 클래스 이름 추가하기.

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");
   }

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // TODO: `Awake` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        fadeSpeed = 0.5f;
        closeSpeed = 1.0f;

        // Locate Cube.002 within the door_opened prefab
        movingPart = transform.Find("Cube.002");
        if (movingPart == null)
        {
            Debug.LogError("Anomaly11_openeddoor: 'Cube.002' not found as a child object.");
            res = false;
        }

        // Locate AudioSource component on the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("Anomaly11_openeddoor: AudioSource component is missing.");
            res = false;
        }

        return res;
    }

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        // TODO: 이상현상 시작하는 코드 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // Start moving the door
        if (movingPart != null)
        {
            StartCoroutine(CloseDoor());
        } 

        // Play AudioSource if available
        if (audioSource != null)
        {
            audioSource.Play();
        }

        return res;
    }

    private IEnumerator CloseDoor()
    {
        Vector3 startPosition = movingPart.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, 0.8f);
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            movingPart.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime * closeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPart.localPosition = targetPosition;  // Ensure the final position is exact
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        Debug.Log("Anomaly11_openeddoor: Door moved successfully.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Collided with Player! Fake ending entered...");
            StartCoroutine(StartFakeEnding(collision.collider.gameObject));
        }
    }

    private IEnumerator StartFakeEnding(GameObject player)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene! Please add a Canvas.");
            yield break;
        }

        // Instantiate Image and attach to Canvas
        Image instantiatedImage = Instantiate(fadeImage, canvas.transform);
        yield return new WaitForSeconds(0.5f);

        float alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (instantiatedImage != null)
            {
                Color c = instantiatedImage.color;
                c.a = alpha;
                instantiatedImage.color = c;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        PlayerManager.Instance.GameOver();
    }
}