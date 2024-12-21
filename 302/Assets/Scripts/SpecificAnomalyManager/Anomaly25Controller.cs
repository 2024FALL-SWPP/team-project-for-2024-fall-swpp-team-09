using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly25Controller : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    public AudioSource audioSource; // Inspector에서 할당

    /**************
     * properties *
     **************/

    public override string Name { get; } = "Anomaly25Controller";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        CreateInvisibleCeiling();
        StartCoroutine(ActivateFloatingEffect());

        return res;
    }

    /***************
     * new methods *
     ***************/

    void CreateInvisibleCeiling()
    {
        GameObject ceiling = new GameObject("InvisibleCeiling");

        ceiling.transform.position = new Vector3(0, 7.5f, 0);
        ceiling.transform.localScale = new Vector3(40f, 0.1f, 40f);
        ceiling.AddComponent<BoxCollider>();
    }

    IEnumerator ActivateFloatingEffect()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("floatable");

        yield return new WaitForSeconds(15.0f);

        GameManager.Instance.SetStageClear();

        // 음악 재생
        if(audioSource != null) {
            audioSource.Play();
        }

        foreach (GameObject obj in objects) {
            Rigidbody rb = obj.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.mass = 10.0f;
            StartCoroutine(StartFloating(obj));
        }
    }

    IEnumerator StartFloating(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        Vector3 randomDirection;
        float duration = 5.0f;
        float timeStart = Time.time;
        float time;
        float currentSpeed = 0.0f;
        float targetSpeed = 0.3f;

        randomDirection = new Vector3(
            Random.Range(-1.0f, 1.0f),
            Random.Range(0.5f, 1.0f),
            Random.Range(-1.0f, 1.0f)
        ).normalized;

        while ((time = Time.time - timeStart) < duration) {
            currentSpeed = Mathf.Lerp(0f, targetSpeed, time / duration);
            if(obj.transform.position.y >= 6f) {
                randomDirection.y = 0.0f;
            }

            rb.velocity = randomDirection * currentSpeed;

            yield return new WaitForFixedUpdate();
        }
    }
}
