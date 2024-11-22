using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SetStageClear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
