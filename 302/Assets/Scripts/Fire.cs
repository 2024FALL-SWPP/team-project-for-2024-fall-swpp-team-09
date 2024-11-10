using UnityEngine;

public class Fire : MonoBehaviour
{
   [SerializeField] private float extinguishTime = 2f;
   
   private Anomaly26Manager anomalyManager;
   private float currentExtinguishTime = 0f;
   private bool isBeingExtinguished = false;

   private void Start()
   {
       
       anomalyManager = FindObjectOfType<Anomaly26Manager>();
       if (anomalyManager)
       {
       }
   }

   private void Update()
   {
       if (isBeingExtinguished)
       {
           currentExtinguishTime -= Time.deltaTime;
           isBeingExtinguished = false;

           if (currentExtinguishTime <= 0)
           {
               Invoke("DestroyFire", 0.1f);  // Destroy를 약간 지연
           }
       }
   }

   public void StartExtinguishing()
   {
       isBeingExtinguished = true;
       
       if (currentExtinguishTime <= 0)
       {
           currentExtinguishTime = extinguishTime;
       }
   }

   private void DestroyFire()
   {
       Destroy(gameObject);
   }
}