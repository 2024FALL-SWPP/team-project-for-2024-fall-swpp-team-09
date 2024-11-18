using UnityEngine;

public class Anomaly24Manager : MonoBehaviour
{
    public GameObject prefab;
    private Transform[] childTransforms;

    void Start()
    {
        GameObject sitGrils = GameObject.Find("SitGrils");
        if (sitGrils != null)
        {
            childTransforms = new Transform[sitGrils.transform.childCount];
            for (int i = 0; i < sitGrils.transform.childCount; i++)
            {
                childTransforms[i] = sitGrils.transform.GetChild(i);
            }

            Destroy(sitGrils);

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
            Debug.LogError("SitGrils object not found!");
        }
    }
}
