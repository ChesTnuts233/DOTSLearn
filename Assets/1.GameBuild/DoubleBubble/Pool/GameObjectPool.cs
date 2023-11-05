using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum GameObjectPoolType
{
    Normal //普通
}

[System.Serializable]
public class PoolTypePrefab
{
    public GameObjectPoolType poolType;
    public string prefabPath;
    public GameObject prefab;

    public PoolTypePrefab(GameObjectPoolType poolType, string prefabPath)
    {
        this.poolType = poolType;
        this.prefabPath = prefabPath;
    }
}

public class GameObjectPool : MonoBehaviour
{
    public static GameObjectPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("是否预生成所有预制体")]
    public bool IsPreInstantiate = false;

    [ShowIf("IsPreInstantiate")] [Header("预生成个数")]
    public int InitCreateCount;

    private List<PoolTypePrefab> poolTypePrefabList = new List<PoolTypePrefab>();

    private Dictionary<GameObjectPoolType, Stack<GameObject>> poolDic =
        new Dictionary<GameObjectPoolType, Stack<GameObject>>();

    /// <summary>
    /// 从对象池取走的物体与对应的PoolType
    /// </summary>
    private Dictionary<GameObject, GameObjectPoolType>
        objPoolTypeDic = new Dictionary<GameObject, GameObjectPoolType>();

    void Start()
    {
        SetPrefabPath();
        InitPool();
    }

    private void SetPrefabPath()
    {
        poolTypePrefabList.Add(new PoolTypePrefab(GameObjectPoolType.Normal, "NormalBubble"));
    }

    private void InitPool()
    {
        foreach (var item in poolTypePrefabList)
        {
            poolDic.Add(item.poolType, new Stack<GameObject>());
            if (IsPreInstantiate == false) continue;
            for (int i = 0; i < InitCreateCount; i++)
            {
                GameObject temp = Instantiate(GetPrefabByPoolType(item.poolType));
                temp.transform.SetParent(transform, false);
                objPoolTypeDic.Add(temp, item.poolType);
                StoreObject(temp);
            }
        }
    }

    public GameObject GetObject(GameObjectPoolType poolType)
    {
        GameObject result = null;
        if (!poolDic.ContainsKey(poolType))
        {
            Debug.LogError(poolType.ToString() + "类型，对象池不存在！！");
        }
        else
        {
            if (poolDic[poolType].Count > 0)
            {
                var obj = poolDic[poolType].Pop();
                result = obj;
            }
            else
            {
                GameObject prefab = GetPrefabByPoolType(poolType);
                if (prefab != null)
                {
                    GameObject temp = Instantiate(prefab);
                    temp.transform.SetParent(transform, false);
                    result = temp;
                }
            }
        }

        if (result)
        {
            objPoolTypeDic.Add(result, poolType);
            result.GetComponent<PoolBase>()?.OnInit();
            result.SetActive(true);
        }

        return result;
    }

    public GameObject GetObject(GameObjectPoolType poolType, Vector3 pos, Vector3 euler, Transform par = null)
    {
        var obj = GetObject(poolType);
        if (par)
            obj.transform.SetParent(par);
        obj.transform.localPosition = pos;
        obj.transform.localEulerAngles = euler;

        return obj;
    }

    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="go"></param>
    /// <param name="poolType"></param>
    public void StoreObject(GameObject go)
    {
        if (go == null || transform == null) return;

        if (!objPoolTypeDic.ContainsKey(go)) return;
        GameObjectPoolType poolType = objPoolTypeDic[go];
        objPoolTypeDic.Remove(go);

        if (!poolDic.ContainsKey(poolType))
        {
            Debug.LogError(poolType.ToString() + "类型，对象池不存在！！");
            return;
        }

        if (poolDic[poolType].Contains(go)) return;

        go.GetComponent<PoolBase>()?.OnRecycle();
        go.transform.SetParent(transform, false);
        go.SetActive(false);
        poolDic[poolType].Push(go);
    }

    /// <summary>
    /// 根据对象池物体类型获取对应的预制体
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
    private GameObject GetPrefabByPoolType(GameObjectPoolType poolType)
    {
        foreach (var item in poolTypePrefabList)
        {
            if (item.poolType == poolType)
            {
                if (item.prefab != null)
                    return item.prefab;
                else
                {
                    return Resources.Load<GameObject>(item.prefabPath);
                }
            }
        }

        Debug.LogError(poolType.ToString() + "对应的预制体不存在");
        return null;
    }
}