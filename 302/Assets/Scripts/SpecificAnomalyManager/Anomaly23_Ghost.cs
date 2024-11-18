using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly23_Ghost : MonoBehaviour
{
    /*************
     * constants *
     *************/

    private const string NAME = "Anomaly23Manager";

    /**********
     * fields *
     **********/

    // 오브젝트의 이름
    public string namePlayer;
    public string nameCamera;

    // 수치
    public Vector3 position;
    public float speedInit;
    public float speedDelta;
    public float durationChase;
    public float durationFade;

    // 애니메이터
    private Animator _animator;

    // 오브젝트
    private GameObject _objectPlayer;
    private GameObject _objectCamera;

    // 수치
    private float _timeStart;

    // 플래그
    private bool _isChasing;

    /**************
     * properties *
     **************/

    public Anomaly23Manager Manager { get; set; }

    /**********************
     * overridden methods *
     **********************/

    // Start is called on the frame when a script is enabled just
    // before any of the Update methods are called the first time.
    void Start()
    {
        if (!InitFields()) {
            return;
        }

        transform.position = position;
    }

    // OnCollisionEnter is called when this collider/rigidbody
    // has begun touching another rigidbody/collider.
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && _isChasing) {
            PlayerController scriptPlayer = _objectPlayer.GetComponent<PlayerController>();

            if (scriptPlayer != null) {
                Debug.Log($"[{NAME}] Find `scriptPlayer` successfully.");
                scriptPlayer.GameOver();
            } else {
                Debug.LogWarning($"[{NAME}] Cannot find `scriptPlayer`.");
            }
        }
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        float time = Time.time - _timeStart;

        if (_isChasing) {
            if (time < durationChase) {
                Vector3 positionPlayer = _objectPlayer.transform.position;
                Vector3 positionCamera = _objectCamera.transform.position;
                Vector3 positionTarget = (positionPlayer + positionCamera) / 2.0f;
                float speed = speedInit + speedDelta * time / durationChase;

                transform.LookAt(positionTarget);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                _animator.SetFloat("Speed", speed);
            } else {
                _isChasing = false;
                Manager.InteractionSuccess();
                StartCoroutine(FadeAsync());
            }
        }
    }

    /***************
     * new methods *
     ***************/

    // Private fields를 초기화하는 메서드
    private bool InitFields()
    {
        bool res = true;

        // `_animator` 초기화
        _animator = gameObject.GetComponent<Animator>();
        if (_animator != null) {
            Debug.Log($"[{NAME}] Find `_animator` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_animator`.");
            res = false;
        }

        // `_objectPlayer` 초기화
        _objectPlayer = GameObject.Find(namePlayer);
        if (_objectPlayer != null) {
            Debug.Log($"[{NAME}] Find `_objectPlayer` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectPlayer`.");
            res = false;
        }

        // `_objectCamera` 초기화
        _objectCamera = GameObject.Find(nameCamera);
        if (_objectCamera != null) {
            Debug.Log($"[{NAME}] Find `_objectCamera` successfully.");
        } else {
            Debug.LogWarning($"[{NAME}] Cannot find `_objectCamera`.");
            res = false;
        }

        // `_timeStart` 초기화
        _timeStart = Time.time;
        Debug.Log($"[{NAME}] Initialize `_timeStart` successfully: {_timeStart}.");

        // `_isChasing` 초기화
        _isChasing = true;
        Debug.Log($"[{NAME}] Initialize `_isChasing` successfully: {_isChasing}.");

        return res;
    }

    // 소멸 메서드
    private IEnumerator FadeAsync()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        GhostRandom myRandom = new GhostRandom();
        float timeStart = Time.time;
        float time;
        float scale;

        yield return new WaitForSeconds(0.1f);

        while ((time = Time.time - timeStart) < durationFade) {
            scale = (float)(myRandom.LogNormalDist(0.0, 1.0) * 2.0);

            transform.rotation = Random.rotation;
            transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}

// 난수 클래스
class GhostRandom : System.Random
{
    private double NORMAL_MAGICCONST = 4.0 * System.Math.Exp(-0.5) / System.Math.Sqrt(2.0);

    // implement methods with python's implementation
    // https://github.com/python/cpython/blob/3.13/Lib/random.py

    public double NormalDist(double mu = 0.0, double sigma = 0.0)
    {
        double u1, u2, z, zz;

        while (true) {
            u1 = Sample();
            u2 = 1.0 - Sample();
            z = NORMAL_MAGICCONST * (u1 - 0.5) / u2;
            zz = z * z / 4.0;
            if (zz <= -System.Math.Log(u2)) {
                break;
            }
        }

        return mu + z * sigma;
    }

    public double LogNormalDist(double mu, double sigma)
    {
        return System.Math.Exp(NormalDist(mu, sigma));
    }
}
