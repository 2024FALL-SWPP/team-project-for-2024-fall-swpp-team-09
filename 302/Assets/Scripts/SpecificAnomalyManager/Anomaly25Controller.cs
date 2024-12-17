using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly25Controller : AbstractAnomalyComposite
{
    public override string Name { get; } = "Anomaly25Controller";
    public AudioSource audioSource; // Inspector에서 할당

    void Start() {
        StartAnomaly();
    }

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        StartCoroutine(ActivateFloatingEffect());
        CreateInvisibleCeiling();

        return res;
    }

   IEnumerator ActivateFloatingEffect() {
       yield return new WaitForSeconds(15f);
       
       // 음악 재생
       if(audioSource != null) {
           audioSource.Play();
       }
       
       GameObject[] objects = GameObject.FindGameObjectsWithTag("floatable");
       foreach(GameObject obj in objects) {
           Rigidbody rb = obj.AddComponent<Rigidbody>();
           rb.useGravity = false;
           rb.mass = 10f;
           StartCoroutine(StartFloating(obj));
       }
   }
    void CreateInvisibleCeiling() {
    GameObject ceiling = new GameObject("InvisibleCeiling");
   ceiling.transform.position = new Vector3(0, 7.5f, 0);
   ceiling.transform.localScale = new Vector3(40f, 0.1f, 40f);
   BoxCollider ceilingCollider = ceiling.AddComponent<BoxCollider>();
   }

   IEnumerator StartFloating(GameObject obj) {
       GameManager.Instance.SetStageClear();
       Rigidbody rb = obj.GetComponent<Rigidbody>();
       Vector3 randomDirection = new Vector3(
           Random.Range(-1f, 1f),
           Mathf.Abs(Random.Range(0.5f, 1f)),
           Random.Range(-1f, 1f)
       ).normalized;
       
       float currentSpeed = 0f;
       float targetSpeed = 0.3f;
       float duration = 5f;
       float elapsedTime = 0f;
       
       while (elapsedTime < duration) {
           currentSpeed = Mathf.Lerp(0f, targetSpeed, elapsedTime / duration);
           if(obj.transform.position.y >= 6f) {
               randomDirection.y = 0f;
           }
           rb.velocity = randomDirection * currentSpeed;
           elapsedTime += Time.deltaTime;
           yield return new WaitForFixedUpdate();
       }
   }
}