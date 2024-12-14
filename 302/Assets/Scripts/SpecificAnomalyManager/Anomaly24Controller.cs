using UnityEngine;

public class Anomaly24Controller : AbstractAnomalyComposite
{
    public GameObject prefab;
    private Transform[] childTransforms;
    GameObject sitGirls;

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // Code used before `GameManager` updates begin
        Log("Call `StartAnomaly` begin");
        if (StartAnomaly()) {
            Log("Call `StartAnomaly` success");
        } else {
            Log("Call `StartAnomaly` failed", mode: 1);
            res = false;
        }
        // Code used before `GameManager` updates end

        return res;
    }

     // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

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

        return res;
    }
    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        Destroy(sitGirls);

        return res;
    }
    
}
