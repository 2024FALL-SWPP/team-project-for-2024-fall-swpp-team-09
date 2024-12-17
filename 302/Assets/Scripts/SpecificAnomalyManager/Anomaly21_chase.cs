using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class Anomaly21_chase : MonoBehaviour
{
    public float chaseSpeed = 4f;
    public float returnSpeed = 2f;
    public float chaseDistance = 10f;
    public float fadeOutDuration = 4f;
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

    void Start()
    {
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
    }

    void Update()
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
                StartCoroutine(ReturnToStart());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && isChasing)
        {
            StartCoroutine(FadeOutAudio());
            var playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerController.GameOver();
            }
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
}
