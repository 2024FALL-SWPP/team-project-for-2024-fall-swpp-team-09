using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly25Manager : MonoBehaviour
{
   public AudioSource audioSource; // Inspector에서 할당

   void Start() {
       StartCoroutine(ActivateFloatingEffect());
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

   IEnumerator StartFloating(GameObject obj) {
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