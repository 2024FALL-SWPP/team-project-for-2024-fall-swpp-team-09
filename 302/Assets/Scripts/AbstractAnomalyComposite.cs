using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAnomalyComposite : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름 배열
    public string[] names;

    // 프리팹 배열
    public GameObject[] prefabs;

    // 이상현상 오브젝트 리스트
    protected List<AbstractAnomalyObject> objects;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractAnomalyComposite";

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // objects
        objects = new List<AbstractAnomalyObject>();
        Log("Initialize `objects` success");

        return res;
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        Log("Call `InitObjects` begin");
        if (InitObjects()) {
            Log("Call `InitObjects` success");
        } else {
            Log("Call `InitObjects` failed", mode: 1);
            res = false;
        }

        foreach (AbstractAnomalyObject obj in objects) {
            Log($"Call `{obj.Name}.StartAnomaly` for {obj.gameObject.name} begin");
            if (obj.StartAnomaly()) {
                Log($"Call `{obj.Name}.StartAnomaly` success");
            } else {
                Log($"Call `{obj.Name}.StartAnomaly` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        foreach (AbstractAnomalyObject obj in objects) {
            Log($"Call `{obj.Name}.ResetAnomaly` for {obj.gameObject.name} begin");
            if (obj.ResetAnomaly()) {
                Log($"Call `{obj.Name}.ResetAnomaly` success");
            } else {
                Log($"Call `{obj.Name}.ResetAnomaly` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }

    /*******************
     * virtual methods *
     *******************/

    // 오브젝트를 초기화하는 메서드
    protected virtual bool InitObjects()
    {
        bool res = true;

        // 오브젝트
        foreach (string name in names) {
            GameObject gameObj = GameObject.Find(name);

            if (gameObj != null) {
                AbstractAnomalyObject obj = gameObj.GetComponent<AbstractAnomalyObject>();

                if (obj != null) {
                    obj.enabled = true;
                    objects.Add(obj);
                    Log($"Find `{name}` success: {obj.Name}");
                } else {
                    Log($"Find `{name}` failed", mode: 1);
                    res = false;
                }
            } else {
                Log($"Find `{name}` failed", mode: 1);
                res = false;
            }
        }

        // 프리팹
        foreach (GameObject prefab in prefabs) {
            GameObject gameObj = Instantiate(prefab);

            if (gameObj != null) {
                AbstractAnomalyObject obj = gameObj.GetComponent<AbstractAnomalyObject>();

                if (obj != null) {
                    objects.Add(obj);
                    Log($"Instantiate `{prefab.name}` success: {obj.Name}");
                } else {
                    Log($"Instantiate `{prefab.name}` failed", mode: 1);
                    res = false;
                }
            } else {
                Log($"Instantiate `{prefab.name}` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }
}

/* 양식: 복사해서 사용하기.

// `AbstractAnomalyComposite`은 이상현상 개체 묶음에 관한 추상 클래스입니다.
//
// 만약 `AbstractAnomalyObject`를 가지는 오브젝트가 강의실 내에 있다면 이름을 `names`에 넣습니다.
// 만약 `AbstractAnomalyObject`를 가지는 오브젝트가 프리팹으로 돼 있다면 프리팹을 `prefabs`에 넣습니다.
// 그러면 알아서 강의실 내의 개체는 찾고, 프리팹은 instantiate합니다.
// 또, `StartAnomaly`에서는 모든 개체의 `StartAnomaly`를 호출하고,
// `ResetAnomaly`에서는 모든 개체의 `ResetAnomaly`를 호출합니다. (composite pattern이 이미 구현돼 있습니다.)
// 추가적으로 뭘 해야 한다면 오버라이드하면 됩니다.

public class MyClass : AbstractAnomalyComposite // TODO: 클래스 이름 수정하기.
{
    // 클래스 이름
    public override string Name { get; } = ""; // TODO: 클래스 이름 추가하기.

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // TODO: `Awake` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // TODO: 필드 초기화할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // (사실 필드 말고 초기화할 것도 넣어도 됨....)
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        // TODO: 각 개체의 `StartAnomaly`를 호출하는 것 외에 할 게 있으면 추가하기.
        // 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // TODO: 각 개체의 `ResetAnomaly`를 호출하는 것 외에 할 게 있으면 추가하기.
        // 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 오브젝트를 초기화하는 메서드
    protected override bool InitObjects()
    {
        bool res = base.InitObjects();

        // TODO: 주석에서 말한 방식으로 초기화할 수 없는 오브젝트가 있다면 여기서 초기화하기.
        // 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }
} */
