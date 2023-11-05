using System.Collections.Generic;
using DG.Tweening;
using GameBuild;
using KooFrame;
using KooFrame.FrameTools;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;


public enum ShootType
{
    Normal,
    Change,
    Rush
}

public enum BubbleType
{
    Red,
    Blue
}

struct ShootRequest
{
    public ShootType Type;

    public ShootRequest(ShootType type)
    {
        Type = type;
    }
}


public class SpawnBubble : MonoBehaviour
{
    /// <summary>
    /// 派别
    /// </summary>
    [SerializeField]
    private BubbleType type;


    /// <summary>
    /// 另外一边
    /// </summary>
    [SerializeField]
    private Transform otherSide;


    /// <summary>
    /// 生成坐标
    /// </summary>
    [SerializeField]
    private Transform GeneratePos;


    /// <summary>
    /// 发送小球的请求队列
    /// </summary>
    [ShowInInspector, PropertyOrder(20)]
    private Queue<ShootRequest> _requests = new Queue<ShootRequest>();


    /// <summary>
    /// 发射CD
    /// </summary>
    [SerializeField]
    private float ShootCD = 0.5f;


    private float curCDTime;


    /// <summary>
    /// 发射力
    /// </summary>
    [SerializeField]
    private float ShootForce = 0.5f;


    /// <summary>
    /// 是否开始发射
    /// </summary>
    [SerializeField]
    private bool isStartShoot = false;

    [SerializeField]
    private Ease SendEase = Ease.InExpo;

    [SerializeField]
    private float ShootTime = 0.5f;

    [SerializeField]
    private float endSpeed = 0.2f;


    private void Update()
    {
        HandleShoot();
        if (Input.GetKeyDown(KeyCode.L) && type == BubbleType.Red)
        {
            ShootBubble(1, ShootType.Rush);
        }
    }

    /// <summary>
    /// 发射处理
    /// </summary>
    private void HandleShoot()
    {
        if (!isStartShoot)
            return;

        curCDTime -= Time.deltaTime;

        if (_requests.Count > 0 && curCDTime <= 0f)
        {
            //请求出队
            var request = _requests.Dequeue();
            GameObject bubble = null;
            for (int i = 0; i < 6; i++)
            {
                Test(request);
            }
        }
    }

    private void Test(ShootRequest request)
    {
        GameObject bubble;
        switch (request.Type)
        {
            case ShootType.Normal:
                bubble = GameObjectPool.Instance.GetObject(GameObjectPoolType.Normal);
                bubble.transform.localScale = Vector3.one * 0.2f;
                break;
            case ShootType.Change:
                bubble = ResSystem.InstantiateGameObject("NormalBubble");

                bubble.transform.localScale = Vector3.one * 1f;
                break;
            case ShootType.Rush:
                bubble = ResSystem.InstantiateGameObject("RushBubble");
                bubble.transform.localScale = Vector3.one * 0.2f;
                break;
            default:
                bubble = ResSystem.InstantiateGameObject("NormalBubble");
                break;
        }

        bubble.gameObject.transform.position = GeneratePos.position;
        bubble.GetComponent<BubbleBase>().bubbleType =
            type == BubbleType.Red ? BubbleType.Red : BubbleType.Blue;
        bubble.GetComponent<GravitateController>().AttractiveSide =
            otherSide;
        bubble.GetComponent<GravitateController>().BubbleType =
            type == BubbleType.Red ? BubbleType.Red : BubbleType.Blue;
        bubble.transform.GetComponent<SpriteRenderer>().color =
            type == BubbleType.Red ? Color.red : Color.green;
        //重置发射时间
        curCDTime = ShootCD;

        var gra = bubble.GetComponent<GravitateController>();
        //gra.Rb2D.AddForce(ShootForce * GeneratePos.up.normalized, ForceMode2D.Impulse);
        var up = GeneratePos.up;
        DOTween.To(() => gra.Rb2D.velocity, x => gra.Rb2D.velocity = x,
            (Vector2)up.normalized * endSpeed,
            ShootTime).From(ShootForce * (Vector2)up.normalized).SetEase(SendEase).OnComplete(
            () =>
            {
                gra.IsAttract = true;
                gra.AddForceTo();
            });
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.TryGetComponent(out GravitateController bubble);

        if (other.transform.CompareTag("Bubble"))
        {
            if (type != bubble.BubbleType) //碰撞的对方跟自己不是一边的
            {
                //回收对方
                other.gameObject.GameObjectPushPool();
            }
        }
    }

    [Button]
    public void ShootBubble(int num, ShootType shootType)
    {
        for (int i = 0; i < num; i++)
        {
            var request = new ShootRequest(shootType);
            _requests.Enqueue(request);
        }

        isStartShoot = true;
    }
}