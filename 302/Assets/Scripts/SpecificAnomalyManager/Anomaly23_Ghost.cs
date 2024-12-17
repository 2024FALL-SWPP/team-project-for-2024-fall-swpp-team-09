using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Anomaly23_Ghost : SCH_AnomalyObject
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string namePlayer;
    public string nameCamera;

    // 가변 수치
    public Vector3 position;
    public float speedInit;
    public float speedDelta;
    public float durationChase;
    public float durationBlow;
    public float timeAudioStart;
    public float durationFade;
    public int numRotate;
    public float speedBlow;

    // 애니메이터
    private Animator _animator;

    // 오디오 소스
    private AudioSource _audioSource;

    // 오브젝트
    private GameObject _objectPlayer;
    private GameObject _objectCamera;

    // 내부 수치
    private float _timeStart;
    private bool _isChasing;
    private bool _isCatched;

    /**************
     * properties *
     **************/

    public override string Name { get; } = "Anomaly23_Ghost";

    /************
     * messages *
     ************/

    // OnCollisionEnter is called when this collider/rigidbody
    // has begun touching another rigidbody/collider.
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player") && _isChasing) {
            PlayerController script = _objectPlayer.GetComponent<PlayerController>();

            _isCatched = true;
            if (script != null) {
                Log("Call `script.GameOver` begin");
                script.GameOver();
                Log("Call `script.GameOver` end");

                Log("Call `FadeAudioAsync` asynchronously");
                StartCoroutine(FadeAudioAsync());
            } else {
                Log("Call `script.GameOver` failed", mode: 1);
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
            } else if (!_isCatched) {
                _isChasing = false;

                Log("Call `Manager.InteractionSuccess` begin");
                Manager.InteractionSuccess();
                Log("Call `Manager.InteractionSuccess` end");
            }
        }
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _animator
        _animator = GetComponent<Animator>();
        if (_animator != null) {
            Log("Initialize `_animator` success");
        } else {
            Log("Initialize `_animator` failed", mode: 1);
            res = false;
        }

        // _audioSource
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null) {
            Log("Initialize `_audioSource` success");
        } else {
            Log("Initialize `_audioSource` failed", mode: 1);
            res = false;
        }

        // _objectPlayer
        _objectPlayer = GameObject.Find(namePlayer);
        if (_objectPlayer != null) {
            Log("Initialize `_objectPlayer` success");
        } else {
            Log("Initialize `_objectPlayer` failed", mode: 1);
            res = false;
        }

        // _objectCamera
        _objectCamera = GameObject.Find(nameCamera);
        if (_objectCamera != null) {
            Log("Initialize `_objectCamera` success");
        } else {
            Log("Initialize `_objectCamera` failed", mode: 1);
            res = false;
        }

        // _timeStart
        _timeStart = Time.time;
        Log($"Initialize `_timeStart` success: {_timeStart}");

        // _isChasing
        _isChasing = true;
        Log("Initialize `_isChasing` success");

        // _isCatched
        _isCatched = false;
        Log("Initialize `_isChasing` success");

        return res;
    }

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        transform.position = position;
        Log("Set position success");

        Log("Call `StartAudioAsync` asynchronously");
        StartCoroutine(StartAudioAsync());

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Log("Call `BlowAsync` asynchronously");
        StartCoroutine(BlowAsync());

        Log("Call `FadeAudioAsync` asynchronously");
        StartCoroutine(FadeAudioAsync());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 시작하고 일정 시간 후 오디오 소스를 시작하는 메서드
    private IEnumerator StartAudioAsync()
    {
        yield return new WaitForSeconds(timeAudioStart);

        _audioSource.enabled = true;
    }

    // 오디오 소스 볼륨을 서서히 줄이는 메서드
    private IEnumerator FadeAudioAsync()
    {
        float timeStart = Time.time;
        float time;

        yield return null;

        while ((time = Time.time - timeStart) < durationFade) {
            _audioSource.volume = 1.0f - time / durationFade;

            yield return null;
        }

        _audioSource.enabled = false;
    }

    // 지속시간 동안 바람빠지다가 사라지는 메서드
    private IEnumerator BlowAsync()
    {
        SCH_Random random = new SCH_Random();
        Vector3 originStart = transform.position;
        Vector3 origin;
        float timeStart = Time.time;
        float time = 0.0f;

        yield return null;

        for (int i = 1; i <= numRotate; i++) {
            Vector3 position;
            Quaternion a;
            Quaternion b;

            origin = originStart + Vector3.up * time / durationBlow;
            do {
                position = new Vector3(
                    (float)random.NormalDist(),
                    (float)random.NormalDist(),
                    (float)random.NormalDist()
                );
                position += origin - transform.position;
            } while (position.magnitude == 0.0f);

            a = transform.rotation;
            b = transform.rotation;
            b.SetLookRotation(position, position);

            while ((time = Time.time - timeStart) < durationBlow / numRotate * i) {
                float scale = 1.0f - time / durationBlow;
                float t = time / durationBlow * numRotate % 1.0f;

                transform.localScale = new Vector3(scale, scale, scale);
                transform.rotation = Quaternion.Lerp(a, b, t);
                transform.Translate(Vector3.forward * speedBlow * Time.deltaTime);

                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
