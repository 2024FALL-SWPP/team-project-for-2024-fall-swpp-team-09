using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
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
    public float durationFade;

    // 애니메이터
    private Animator _animator;

    // 오브젝트
    private GameObject _objectPlayer;
    private GameObject _objectCamera;

    // 내부 수치
    private float _timeStart;
    private bool _isChasing;

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

            if (script != null) {
                Log("Call `script.GameOver` begin");
                script.GameOver();
                Log("Call `script.GameOver` end");
            } else {
                Log("Call `script.GameOver`: failed", mode: 1);
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

                Log("Call `Manager.InteractionSuccess` begin");
                Manager.InteractionSuccess();
                Log("Call `Manager.InteractionSuccess` end");

                Log("Call `BlowAsync` asynchronously");
                StartCoroutine(BlowAsync());
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
            Log("Initialize `_animator`: success");
        } else {
            Log("Initialize `_animator`: failed", mode: 1);
            res = false;
        }

        // _objectPlayer
        _objectPlayer = GameObject.Find(namePlayer);
        if (_objectPlayer != null) {
            Log("Initialize `_objectPlayer`: success");
        } else {
            Log("Initialize `_objectPlayer`: failed", mode: 1);
            res = false;
        }

        // _objectCamera
        _objectCamera = GameObject.Find(nameCamera);
        if (_objectCamera != null) {
            Log("Initialize `_objectCamera`: success");
        } else {
            Log("Initialize `_objectCamera`: failed", mode: 1);
            res = false;
        }

        // _timeStart
        _timeStart = Time.time;
        Log($"Initialize `_timeStart`: success: {_timeStart}");

        // _isChasing
        _isChasing = true;
        Log("Initialize `_isChasing`: success");

        return res;
    }

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        transform.position = position;
        Log("Set position: success");

        return res;
    }

    /***********
     * methods *
     ***********/

    // 지속시간 동안 바람빠지다가 사라지는 메서드
    private IEnumerator BlowAsync()
    {
        SCH_Random random = new SCH_Random();
        float timeStart = Time.time;
        float time;

        yield return new WaitForSeconds(0.1f);

        while ((time = Time.time - timeStart) < durationFade) {
            float scale = (float)(random.LogNormalDist(0.0, 1.0) * 1.5);

            transform.rotation = Random.rotation;
            transform.localScale = new Vector3(scale, scale, scale);

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
