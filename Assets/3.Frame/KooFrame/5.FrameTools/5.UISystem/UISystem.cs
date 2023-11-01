using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KooFrame
{
    /// <summary>
    /// UI根节点
    /// </summary>
    public class UISystem : MonoBehaviour
    {
        private static UISystem instance;

        public static void Init() { instance = FrameRoot.RootTransform.GetComponentInChildren<UISystem>(); }


        private static Dictionary<string, UIWindowData> UIWindowDataDic => FrameRoot.Setting.UIWindowDataDic;

        [SerializeField, ShowInInspector] private UILayerBase[] uiLayers;
        [SerializeField] private RectTransform dragLayer;
        [SerializeField] private GameObject uiCamera;

        /// <summary>
        /// 拖拽层，位于所有UI的最上层
        /// </summary>
        public static RectTransform DragLayer => instance.dragLayer;

        private static UILayerBase[] UILayers => instance.uiLayers;

        [SerializeField] GameObject UITipsItemPrefab;
        [SerializeField] private RectTransform UITipsItemParent;

        public static GameObject UICamera => instance.uiCamera;

        #region 动态加载/移除窗口数据

        // UI系统的窗口数据中主要包含：预制体路径、是否缓存、当前窗口对象实例等重要信息
        // 为了方便使用，所以窗口数据必须先存放于UIWindowDataDic中，才能通过UI系统显示、关闭等
        //instantiateAtOnce指明窗口对象及其类是否要进行实例化，默认为null，会在窗口打开时加载资源进行实例化且设置为不激活，若窗口资源较大，可以提前在动态加载时就进行实例化

        /// <summary>
        /// 初始化UI元素数据
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </summary>
        /// <param name="windowKey">自定义的名称，可以是资源路径或类型名称或其他自定义</param>
        /// <param name="windowData">窗口的重要数据</param>
        /// <param name="instantiateAtOnce">是否立刻实例化，前提是有缓存必要</param>
        public static void AddUIWindowData(string windowKey, UIWindowData windowData, bool instantiateAtOnce = false)
        {
            if (!UIWindowDataDic.TryAdd(windowKey, windowData)) return; //没有得到就直接返回
            if (!instantiateAtOnce) return; //不提前实例化也直接返回
            if (windowData.isCache)
            {
                UI_WindowBase window = ResSystem.InstantiateGameObject<UI_WindowBase>(windowData.assetPath,
                    UILayers[windowData.layerNum].Root, windowKey);
                windowData.instance = window;
                window.Init();
                window.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameFrame:UIWindowData中的isCache=false，但instantiateAtOnce=true!提前实例化对于不需要缓存的窗口来说没有意义");
            }
        }

        /// <summary>
        /// 初始化UI元素数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowData"></param>
        /// <param name="instantiateAtOnce">
        /// 立刻实例化，但是：
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </param>
        public static void AddUIWindowData(Type type, UIWindowData windowData, bool instantiateAtOnce = false)
        {
            AddUIWindowData(type.FullName, windowData, instantiateAtOnce);
        }

        /// <summary>
        /// 初始化UI元素数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="windowData"></param>
        /// <param name="instantiateAtOnce">
        /// 立刻实例化，但是：
        /// 只执行OnInit，不执行OnShow
        /// 会自动SetActive(false)
        /// </param>
        public static void AddUIWindowData<T>(UIWindowData windowData, bool instantiateAtOnce = false)
        {
            AddUIWindowData(typeof(T), windowData, instantiateAtOnce);
        }

        /// <summary>
        /// 获取UI窗口数据
        /// </summary>
        /// <param name="windowKey"></param>
        /// <returns>可能为Null</returns>
        public static UIWindowData GetUIWindowData(string windowKey)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return windowData;
            }

            return null;
        }

        public static UIWindowData GetUIWindowData(Type windowType) { return GetUIWindowData(windowType.FullName); }

        public static UIWindowData GetUIWindowData<T>() { return GetUIWindowData(typeof(T)); }

        /// <summary>
        /// 尝试获取UI窗口数据
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetUIWindowData(string windowKey, out UIWindowData windowData)
        {
            return UIWindowDataDic.TryGetValue(windowKey, out windowData);
        }

        /// <summary>
        /// 移除UI窗口数据,已存在的窗口会被强行删除
        /// </summary>
        /// <param name="windowKey"></param>
        /// <returns></returns>
        public static bool RemoveUIWindowData(string windowKey)
        {
            if (TryGetUIWindowData(windowKey, out UIWindowData windowData))
            {
                if (windowData.instance != null)
                {
                    Destroy(windowData.instance.gameObject);
                }
            }

            return UIWindowDataDic.Remove(windowKey);
        }

        /// <summary>
        /// 清除所有UI窗口数据
        /// </summary>
        public static void ClearUIWindowData()
        {
            var enumerator = UIWindowDataDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Destroy(enumerator.Current.Value.instance.gameObject);
            }

            UIWindowDataDic.Clear();
        }

        #endregion

        #region UI窗口生命周期管理

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <param name="layer">层级 -1等于不设置</param>
        public static T Show<T>(int layer = -1) where T : UI_WindowBase { return Show(typeof(T), layer) as T; }

        /// <summary>
        /// 显示窗口 异步
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <param name="layer">层级 -1等于不设置</param>
        public static void ShowAsync<T>(Action<T> callback = null, int layer = -1) where T : UI_WindowBase
        {
            ShowAsync(typeof(T), (window) => { callback?.Invoke((T)window); }, layer);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <typeparam name="T">要返回的窗口类型</typeparam>
        /// <param name="windowKey">窗口的Key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static T Show<T>(string windowKey, int layer = -1) where T : UI_WindowBase
        {
            return Show(windowKey, layer) as T;
        }

        /// <summary>
        /// 显示窗口 异步
        /// </summary>
        /// <typeparam name="T">要返回的窗口类型</typeparam>
        /// <param name="windowKey">窗口的Key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static T ShowAsync<T>(string windowKey, Action<T> callback = null, int layer = -1)
            where T : UI_WindowBase
        {
            return ShowAsync(windowKey, callback, layer) as T;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="type">窗口类型</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static UI_WindowBase Show(Type type, int layer = -1) { return Show(type.FullName, layer); }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="type">窗口类型</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static void ShowAsync(Type type, Action<UI_WindowBase> callback = null, int layer = -1)
        {
            ShowAsync(type.FullName, callback, layer);
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="windowKey">窗口的key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static UI_WindowBase Show(string windowKey, int layer = -1)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return ShowWindow(windowData, windowKey, layer);
            }

            // 资源库中没有意味着不允许显示
            Debug.Log($"GameFrame:不存在{windowKey}的UIWindowData");
            return null;
        }

        /// <summary>
        /// 异步显示窗口
        /// </summary>
        /// <param name="windowKey">窗口的key</param>
        /// <param name="layer">层级 -1等于不设置</param>
        public static void ShowAsync(string windowKey, Action<UI_WindowBase> callback = null, int layer = -1)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                ShowAsync(windowData, windowKey, callback, layer);
            }
            else Debug.Log($"JKFrame:不存在{windowKey}的UIWindowData"); // 资源库中没有意味着不允许显示
        }

        private static UI_WindowBase ShowWindow(UIWindowData windowData, string windowKey, int layer = -1)
        {
            int layerNum = layer == -1 ? windowData.layerNum : layer;
            // 实例化实例或者获取到实例，保证窗口实例存在
            if (windowData.instance != null)
            {
                // 原本就激活使用状态，避免内部计数问题，进行一次层关闭
                if (windowData.instance.UIEnable)
                {
                    UILayers[windowData.layerNum].OnWindowClose();
                }

                windowData.instance.gameObject.SetActive(true);
                windowData.instance.transform.SetParent(UILayers[layerNum].Root);
                //移动transform到末尾 [Move the transform to the end of the local transform list]
                windowData.instance.transform.SetAsLastSibling();
                windowData.instance.ShowGeneralLogic(layerNum);
            }
            else
            {
                UI_WindowBase window =
                    ResSystem.InstantiateGameObject<UI_WindowBase>(windowData.assetPath, UILayers[layerNum].Root,
                        windowKey);
                windowData.instance = window;
                window.Init();
                window.ShowGeneralLogic(layerNum);
            }

            windowData.layerNum = layerNum;
            UILayers[layerNum].OnWindowShow();
            return windowData.instance;
        }

        private static void ShowAsync(UIWindowData windowData, string windowKey, Action<UI_WindowBase> callback = null,
            int layer = -1)
        {
            int layerNum = layer == -1 ? windowData.layerNum : layer;
            // 实例化实例或者获取到实例，保证窗口实例存在
            if (windowData.instance != null)
            {
                // 原本就激活使用状态，避免内部计数问题，进行一次层关闭
                if (windowData.instance.UIEnable)
                {
                    UILayers[windowData.layerNum].OnWindowClose();
                }

                windowData.instance.gameObject.SetActive(true);
                windowData.instance.transform.SetParent(UILayers[layerNum].Root);
                windowData.instance.transform.SetAsLastSibling();
                windowData.instance.ShowGeneralLogic(layerNum);
            }
            else
            {
                ResSystem.InstantiateGameObjectAsync<UI_WindowBase>(windowData.assetPath,
                    (window) =>
                    {
                        windowData.instance = window;
                        window.Init();
                        window.ShowGeneralLogic(layerNum);
                        callback?.Invoke(window);
                    }
                    , UILayers[layerNum].Root, windowKey);
            }

            windowData.layerNum = layerNum;
            UILayers[layerNum].OnWindowShow();
        }

        #endregion

        #region 获取与销毁窗口

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static UI_WindowBase GetWindow(string windowKey)
        {
            if (UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData))
            {
                return windowData.instance;
            }

            return null;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>(string windowKey) where T : UI_WindowBase { return GetWindow(windowKey) as T; }


        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>() where T : UI_WindowBase { return GetWindow(typeof(T).FullName) as T; }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns>没找到会为Null</returns>
        public static UI_WindowBase GetWindow(Type windowType) { return GetWindow(windowType.FullName); }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowKey">窗口Key</param>
        /// <returns>没找到会为Null</returns>
        public static T GetWindow<T>(Type windowType) where T : UI_WindowBase
        {
            return GetWindow(windowType.FullName) as T;
        }

        /// <summary>
        /// 尝试获取窗口
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetWindow(string windowKey, out UI_WindowBase window)
        {
            UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData);
            window = windowData?.instance;
            return window != null;
        }

        /// <summary>
        /// 尝试获取窗口
        /// </summary>
        /// <param name="windowKey"></param>
        public static bool TryGetWindow<T>(string windowKey, out T window) where T : UI_WindowBase
        {
            UIWindowDataDic.TryGetValue(windowKey, out UIWindowData windowData);
            window = windowData?.instance as T;
            return window != null;
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        public static void DestroyWindow(string windowKey)
        {
            UI_WindowBase window = GetWindow(windowKey);
            if (window != null)
            {
                //立即销毁窗口 如果不立即销毁 可能会延迟几个
                DestroyImmediate(window.gameObject);
            }
        }

        #endregion

        #region 关闭窗口

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        public static void Close<T>() { Close(typeof(T)); }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="Type">窗口类型</typeparam>
        public static void Close(Type type) { Close(type.FullName); }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="windowKey">窗口key</param>
        public static void Close(string windowKey)
        {
            if (TryGetUIWindowData(windowKey, out UIWindowData windowData))
            {
                if (windowData.instance != null && CloseWindow(windowData))
                {
                    UILayers[windowData.layerNum].OnWindowClose();
                }
                else Debug.LogWarning("GameFrame:您需要关闭的窗口不存在或已经关闭");
            }
            else Debug.LogWarning("GameFrame:未查询到UIWindowData");
        }


        private static bool CloseWindow(UIWindowData windowData)
        {
            if (windowData.instance.UIEnable)
            {
                windowData.instance.CloseGeneralLogic();
                // 缓存则隐藏
                if (windowData.isCache)
                {
                    windowData.instance.transform.SetAsFirstSibling();
                    windowData.instance.gameObject.SetActive(false);
                }
                // 不缓存则销毁
                else
                {
#if ENABLE_ADDRESSABLES
                    ResSystem.UnloadInstance(windowData.instance.gameObject);
#endif
                    DestroyImmediate(windowData.instance.gameObject);
                    windowData.instance = null;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭全部窗口
        /// </summary>
        public static void CloseAllWindow()
        {
            // 处理缓存中所有状态的逻辑
            foreach (var item in UIWindowDataDic.Values)
            {
                if (item.instance != null && item.instance.gameObject.activeInHierarchy == true)
                {
                    CloseWindow(item);
                }
            }

            for (int i = 0; i < UILayers.Length; i++)
            {
                UILayers[i].Reset();
            }
        }

        #endregion

        #region UITips

        public static void AddTips(string tips)
        {
            UITipsItem item =
                PoolSystem.GetGameObject<UITipsItem>(instance.UITipsItemPrefab.name, instance.UITipsItemParent);
            if (item == null)
                item = GameObject.Instantiate(instance.UITipsItemPrefab, instance.UITipsItemParent)
                    .GetComponent<UITipsItem>();
            item.Init(tips);
        }

        #endregion

        #region 工具

        private static List<RaycastResult> raycastResultList = new List<RaycastResult>();

        /// <summary>
        /// 检查鼠标是否在UI上,会屏蔽名称为Mask的物体
        /// </summary>
        public static bool CheckMouseOnUI()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            return CheckPositionOnUI(Input.mousePosition);
#else
            return CheckPositoinOnUI(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
#endif
        }

        private static UnityEngine.EventSystems.EventSystem eventSystem;
        private static PointerEventData pointerEventData;

        /// <summary>
        /// 检查一个坐标是否在UI上,会屏蔽名称为Mask的物体
        /// </summary>
        public static bool CheckPositionOnUI(Vector2 pos)
        {
            if (eventSystem == null)
            {
                eventSystem = UnityEngine.EventSystems.EventSystem.current;
                pointerEventData = new PointerEventData(eventSystem);
            }

            pointerEventData.position = pos;
            // 射线去检测有没有除了Mask以外的任何UI物体
            eventSystem.RaycastAll(pointerEventData, raycastResultList);
            for (int i = 0; i < raycastResultList.Count; i++)
            {
                // 是UI，同时还不是Mask作用的物体
                if (raycastResultList[i].gameObject.name != "Mask")
                {
                    raycastResultList.Clear();
                    return true;
                }
            }

            raycastResultList.Clear();
            return false;
        }

        #endregion
    }
}