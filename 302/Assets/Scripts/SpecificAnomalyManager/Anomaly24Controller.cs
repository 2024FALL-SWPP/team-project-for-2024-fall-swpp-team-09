using UnityEngine;

public class Anomaly24Controller : AbstractAnomalyObject
{
    public GameObject prefab;
    private Transform[] childTransforms;
    GameObject sitGirls;

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        sitGirls = GameObject.Find("SitGirls");

        if (sitGirls != null)
        {
            childTransforms = new Transform[sitGirls.transform.childCount];
            for (int i = 0; i < sitGirls.transform.childCount; i++)
            {
                childTransforms[i] = sitGirls.transform.GetChild(i);
            }

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
            res = false;
        }

        Destroy(sitGirls);

        return res;
    }
    
}
