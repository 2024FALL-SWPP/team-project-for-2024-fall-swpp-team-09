using UnityEngine;

public class Anomaly24Manager : MonoBehaviour
{
    public GameObject prefab;
    private Transform[] childTransforms;

    void Start()
    {
        GameObject sitGirls = GameObject.Find("SitGirls");
        if (sitGirls != null)
        {
            childTransforms = new Transform[sitGirls.transform.childCount];
            for (int i = 0; i < sitGirls.transform.childCount; i++)
            {
                childTransforms[i] = sitGirls.transform.GetChild(i);
            }

            Destroy(sitGirls);

            foreach (Transform childTransform in childTransforms)
            {
                Vector3 newPosition = new Vector3(
                    childTransform.position.x,
                    childTransform.position.y + 2, // Y 좌표에 2 추가
                    childTransform.position.z
                );

                Instantiate(prefab, newPosition, childTransform.rotation);
            }
        }
        else
        {
            Debug.LogError("SitGirls object not found!");
        }
    }
}
