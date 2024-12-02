using System.Collections;
using UnityEngine;

public class Anomaly20_Player : SCH_AnomalyObject
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string namePlayer;

    // 가변 수치
    public float duration;

    // 오브젝트
    private GameObject _objectPlayer;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20_Player";

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        Transform playerTransform = _objectPlayer.transform;

        transform.position = playerTransform.position;
        transform.rotation = Quaternion.Euler(0.0f, playerTransform.rotation.eulerAngles.y, 0.0f);
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objectPlayer
        _objectPlayer = GameObject.Find(namePlayer);
        if (_objectPlayer != null) {
            Log("Initialize `_objectPlayer` success");
        } else {
            Log("Initialize `_objectPlayer` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Log("Call `FadeAsync` asynchronously");
        StartCoroutine(FadeAsync());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 지속시간 동안 투명해지다가 사라지는 메서드
    private IEnumerator FadeAsync()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        float timeStart = Time.time;
        float time;

        foreach (Renderer renderer in renderers) {
            foreach (Material material in renderer.materials) {
                material.renderQueue = 3000;
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.SetFloat("_Mode", 2);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_ZWrite", 0);
            }
        }

        yield return null;

        while ((time = Time.time - timeStart) < duration) {
            float alpha = 1.0f - time / duration;

            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    Color color = material.color;

                    color.a = alpha;
                    material.color = color;
                }
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
