using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]

public class Anomaly21_chase : AbstractAnomalyInteractable 
{
    public float chaseSpeed;
    public float returnSpeed;
    public float chaseDistance;
    public float fadeOutDuration;
    public Transform startPoint;
    private bool isChasing = false;
    private bool canChase = true;
    private Quaternion startRotation;
    private Vector3 startPosition;

    private Transform player;
    private NavMeshAgent navAgent;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;
    private AudioSource audioSource;
    private GameObject unityIcon;
    private float elapsedTime = 0f;

    // 클래스 이름
    public override string Name { get; } = "Anomaly21_chase";


    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        chaseSpeed = 4f;
        returnSpeed = 2f;
        chaseDistance = 10f;
        fadeOutDuration = 4f;

        player = GameObject.FindWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        unityIcon = transform.Find("unity_icon").gameObject;

        startPosition = transform.position;
        startRotation = transform.rotation;

        LockPositionAndRotation();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        return res;
    }

    private void Update()
    {
        if (!canChase) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distance <= chaseDistance)
        {
            isChasing = true;
            navAgent.enabled = true;
            navAgent.speed = chaseSpeed;
            navAgent.updateRotation = true;
            rb.isKinematic = false;
            animator.SetBool("isChasing", true);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (isChasing)
        {
            navAgent.SetDestination(player.position);
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 20f)
            {
                GameManager.Instance.SetStageClear();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && isChasing)
        {
            StartCoroutine(FadeOutAudio());
            GameManager.Instance.GameOver();
        }
    }

    private IEnumerator ReturnToStart()
    {
        isChasing = false;
        canChase = false;
        navAgent.speed = returnSpeed;
        navAgent.SetDestination(startPoint.position);

        StartCoroutine(FadeOutAudio());

        DestroyUnityIcon();

        animator.SetBool("isChasing", true);

        while (Vector3.Distance(transform.position, startPoint.position) > 0.5f)
        {
            yield return null;
        }

        navAgent.enabled = false;
        LockPositionAndRotation();
        animator.SetBool("isChasing", false);

        GameManager.Instance.SetStageClear();

        elapsedTime = 0f;
    }

    private void DestroyUnityIcon()
    {
        if (unityIcon != null)
        {
            Destroy(unityIcon);
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private void LockPositionAndRotation()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        StartCoroutine(ReturnToStart());
        Debug.Log("Anomaly21_chase: Stage Cleared.");

        return res;
    }
}
