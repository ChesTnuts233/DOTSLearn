using System;
using UnityEngine;

namespace KooFrame
{
#if UNITY_EDITOR
    using UnityEditor;

    [InitializeOnLoad]
#endif
    [DefaultExecutionOrder(-50)]
    public class FrameRoot : MonoBehaviour
    {
        private FrameRoot()
        {
        }

        private static FrameRoot instance;

        public static FrameRoot Instance => instance;

        public static Transform RootTransform { get; private set; }

        public static FrameSetting Setting
        {
            get => instance.frameSetting;
        }

        // 框架层面的配置文件
        [SerializeField]
        private FrameSetting frameSetting;

        public GameObject eventSystem;

        private void Awake()
        {
            // 防止Editor下的Instance已经存在，并且是自身
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            RootTransform = transform;
            DontDestroyOnLoad(gameObject);
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            InitSystems();
            //InitManagers();
        }


        private void InitSystems()
        {
            PoolSystem.Init();
            EventSystem.Init();
            MonoSystem.Init();
            SaveSystem.Init();
            LocalizationSystem.Init();
            InputMgrSystem.Init();
            UISystem.Init();
            AudioSystem.Init();
        }

        // #region GamePlayer
        //
        // private void InitManagers()
        // {
        //     
        //     ItemDic.Init();
        //     
        //     AStarAlgorithm.Init();
        //     AStarAlgorithm.LoadSceneObstacles(SceneName.FuncTest);
        //     
        // }
        //
        // #endregion


        #region Editor

#if UNITY_EDITOR
        // 编辑器专属事件系统
        public static EventModule EditorEventModule;

        [Obsolete("Obsolete")]
        static FrameRoot()
        {
            EditorEventModule = new EventModule();
            EditorApplication.update += () => { InitForEditor(); };
        }

        [InitializeOnLoadMethod]
        public static void InitForEditor()
        {
            // 当前是否要进行播放或准备播放中
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            if (instance == null)
            {
                instance = GameObject.FindFirstObjectByType<FrameRoot>();
                if (instance == null) return;
                instance.frameSetting.InitOnEditor();
                // 场景的所有窗口都进行一次Show
                UI_WindowBase[] window = instance.transform.GetComponentsInChildren<UI_WindowBase>();
                foreach (UI_WindowBase win in window)
                {
                    win.ShowGeneralLogic(-1);
                }
            }
        }
#endif

        #endregion
    }
}