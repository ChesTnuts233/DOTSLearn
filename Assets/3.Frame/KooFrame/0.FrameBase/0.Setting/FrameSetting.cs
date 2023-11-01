using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KooFrame
{
    /// <summary>
    /// 资源系统类型
    /// </summary>
    public enum ResourcesSystemType
    {
        Resources,
        Addressables
    }

    /// <summary>
    /// 存档系统类型
    /// </summary>
    public enum SaveSystemType
    {
        Binary,
        Json
    }

    /// <summary>
    /// 框架的设置SO文件
    /// </summary>
    public class FrameSetting : SerializedScriptableObject
    {
        #region UI

        [LabelText("UI窗口数据(无需手动填写)")]
        public Dictionary<string, UIWindowData> UIWindowDataDic = new Dictionary<string, UIWindowData>();

        #endregion

        #region 资源管理

        [SerializeField, LabelText("资源管理方式")]
#if UNITY_EDITOR
        [OnValueChanged(nameof(SetResourcesSystemType))]
#endif
        private ResourcesSystemType resourcesSystemType = ResourcesSystemType.Resources;

        #endregion

        #region 存档

#if UNITY_EDITOR
        [LabelText("存档方式"), Tooltip("修改类型会导致之前的存档丢失"), OnValueChanged(nameof(SetSaveSystemType))]
#endif
        public SaveSystemType SaveSystemType = SaveSystemType.Binary;

        #endregion

        public static int DefaultMaxGOPCount = -1; //默认初始化的对象池上限


#if UNITY_EDITOR


        public static void CreateFrameSetting()
        {
            FrameSetting setting = ScriptableObject.CreateInstance<FrameSetting>();
            string[] selectedGUIDs = Selection.assetGUIDs;
            string assetPath = "Assets/3.Frame/KooFrame/FrameBase/0.Setting";
            foreach (string guid in selectedGUIDs)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guid);
            }

            string fileName = "FrameSetting.asset";

            if (!AssetDatabase.IsValidFolder(assetPath))
                AssetDatabase.CreateFolder("Assets", "CustomHierarchyConfigs");

            AssetDatabase.CreateAsset(setting, $"{assetPath}/{fileName}");
            AssetDatabase.SaveAssets();

            Debug.Log($"FrameSetting配置创建在: {assetPath}/{fileName}");
        }

        [Button("重置设定")]
        [Obsolete("Obsolete")]
        public void Reset()
        {
            SetResourcesSystemType();
            SetSaveSystemType();
            InitUIWindowDataDicOnEditor();
        }

        public void InitOnEditor()
        {
            SetResourcesSystemType();
            InitUIWindowDataDicOnEditor();
        }


        /// <summary>
        /// 增加预处理指令
        /// </summary>
        public static void AddScriptCompilationSymbol(string name)
        {
            BuildTargetGroup buildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string group = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!group.Contains(name))
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, group + ";" + name);
            }
        }

        /// <summary>
        /// 移除预处理指令
        /// </summary>
        public static void RemoveScriptCompilationSymbol(string name)
        {
            BuildTargetGroup buildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string group = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (group.Contains(name))
            {
                UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup,
                    group.Replace(";" + name, string.Empty));
            }
        }

        private void InitUIWindowDataDicOnEditor()
        {
            UIWindowDataDic.Clear();
            // 获取所有程序集
            System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            Type baseType = typeof(UI_WindowBase);
            // 遍历程序集
            foreach (System.Reflection.Assembly assembly in asms)
            {
                // 遍历程序集下的每一个类型
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (baseType.IsAssignableFrom(type)
                        && !type.IsAbstract)
                    {
                        var attributes = type.GetCustomAttributes<UIWindowDataAttribute>();
                        foreach (var attribute in attributes)
                        {
                            UIWindowDataDic.Add(attribute.windowKey,
                                new UIWindowData(attribute.isCache, attribute.assetPath, attribute.layerNum));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改资源管理系统的类型
        /// </summary>
        public void SetResourcesSystemType()
        {
            switch (resourcesSystemType)
            {
                case ResourcesSystemType.Resources:
                    RemoveScriptCompilationSymbol("ENABLE_ADDRESSABLES");
                    // 查找资源R.cs，如果有需要删除
                    string path = Application.dataPath + "/3.Frame/KooFrame/FrameSystems/2.ResSystem/R.cs";
                    if (System.IO.File.Exists(path))
                        AssetDatabase.DeleteAsset("Assets/3.Frame/KooFrame/FrameSystems/2.ResSystem/R.cs");
                    break;
                case ResourcesSystemType.Addressables:
                    AddScriptCompilationSymbol("ENABLE_ADDRESSABLES");
                    break;
            }
        }

        /// <summary>
        /// 修改存档系统的类型
        /// </summary>
        private void SetSaveSystemType()
        {
            // 清空存档
            SaveSystem.DeleteAll();
        }

#endif
    }
}