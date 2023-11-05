using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase : MonoBehaviour
{
    private Dictionary<GameObjectPoolType, GameObject> attachObjDic = new Dictionary<GameObjectPoolType, GameObject>();

    public virtual void OnInit()
    {
        attachObjDic.Clear();
    }
    public virtual void OnRecycle()
    {
        RecycleAllAttach();
    }

    /// <summary>
    /// 将对象池取出来的物体附加到此物体身上（比如特效等）
    /// 等此物体被回收的时候，需要将所有附加物体统一回收
    /// </summary>
    public void Attach(GameObjectPoolType poolType, GameObject obj)
    {
        if (!attachObjDic.ContainsKey(poolType))
        {
            attachObjDic.Add(poolType, obj);
        }
    }
    public bool IsAttach(GameObjectPoolType poolType)
    {
        return attachObjDic.ContainsKey(poolType);
    }
    public void RecycleAttach(GameObjectPoolType poolType)
    {
        if (attachObjDic.ContainsKey(poolType))
        {
            GameObjectPool.Instance.StoreObject(attachObjDic[poolType]);
            attachObjDic.Remove(poolType);
        }
    }
    public void RecycleAllAttach()
    {
        foreach (var item in attachObjDic)
        {
            GameObjectPool.Instance.StoreObject(item.Value);
        }
        attachObjDic.Clear();
    }
}
