using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private AudioSource fireSound;
    
    private Anomaly26Manager anomalyManager;
    private bool isExtinguishing = false;

    private void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly26Manager>();
        if (anomalyManager)
        {
            anomalyManager.RegisterFire(this);
        }
    }

    public void Extinguish()
    {
        if (!isExtinguishing)
        {
            isExtinguishing = true;
            StartCoroutine(FadeOutAndDestroy());
            
            if (anomalyManager)
            {
                anomalyManager.UnregisterFire(this);
            }
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        if (fireParticles)
        {
            var emission = fireParticles.emission;
            var startRate = emission.rateOverTime.constant;
            
            for (float t = 0; t < 1; t += Time.deltaTime / 3f)
            {
                emission.rateOverTime = startRate * (1 - t);
                if (fireSound)
                {
                    fireSound.volume = 1 - t;
                }
                yield return null;
            }
        }
        
        Destroy(gameObject);
    }
}