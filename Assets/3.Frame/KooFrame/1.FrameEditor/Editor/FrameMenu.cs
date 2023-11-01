#if UNITY_EDITOR

using KooFrame.BaseSystem;
using UnityEditor;
using UnityEngine;


namespace KooFrame
{
	public static class FrameMenu
	{
		
		
		[MenuItem("KooFrame/生成框架推荐文件分类模式", false, 99)]
		private static void CreateFoldByFrameModel()
		{
			KooFrameInstance.CreateFoldsByUseModel();
		}
		
		
		// 这里下面是写创建脚本模板的位置  后面我再研究一下怎么动态根据文件内容生成这些 MenuItem 代码

		#region 代码模板

		[MenuItem("Assets/KooFrame/C# DefaultScripts", false, 1)]
		public static void CreatDefaultScripts()
		{
			KooScriptsTemplates.CreateMyScript("DefaultScripts.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/01-KooFrame__MonoBehaviour-DefaultScripts.cs.txt");
		}
		
		[MenuItem("Assets/KooFrame/C# DefaultMonoScripts 完整架构模板", false, 2)]
		public static void CreateDefaultMonoScripts()
		{
			KooScriptsTemplates.CreateMyScript("DefaultMonoScripts.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/04-KooFrame__MonoBehaviour 完整架构模板.cs.txt");
		}


		[MenuItem("Assets/KooFrame/C# KooMonoBehaviourScript ", false, 3)]
		public static void CreatKooMonoBehaviourScript()
		{
			KooScriptsTemplates.CreateMyScript("KooBehaviourScript.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/00-KooFrame__KooBehaviour-KooBehaviourScript.cs.txt");
		}

		[MenuItem("Assets/KooFrame/C# TestMonoScript", false, 4)]
		public static void CreatTestMonoScript()
		{
			KooScriptsTemplates.CreateMyScript("TestMonoScript.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/03-KooFrame__TestMonoBehaviour.cs.txt");
		}

		[MenuItem("Assets/KooFrame/C# KooUIBasePanel", false, 5)]
		public static void CreatUIBaseScript()
		{
			KooScriptsTemplates.CreateMyScript("KooUIBasePanel.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/02-KooFrame__UI-KooUIBasePanel.cs.txt");
		}



		[MenuItem("Assets/KooFrame/C# TestScriptsTemplate", false, 6)]
		public static void CreatTestScriptsTemplate()
		{
			KooScriptsTemplates.CreateMyScript("TestScriptsTemplate.cs",
				"Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/05-KooFrame__TestScriptsTemplate.cs.txt");
		}

		[MenuItem("Assets/KooFrame/C# Tasks_AI", false, 7)]
		public static void CreatTasks_AI()
		{
			KooScriptsTemplates.CreateMyScript(" Tasks_AI.cs",
				 "Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/06-KooFrame__Tasks_AI.cs.txt");
		}
        [MenuItem("Assets/KooFrame/C# ItemStateScripts 架构模板", false, 2)]
        public static void CreateItemStateScript()
        {
            KooScriptsTemplates.CreateMyScript("ItemStateScript.cs",
                "Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/01-ItemStateScript.cs.txt");
        }
        // 写上面就行 下面是模板 //

        // [MenuItem("Assets/KooFrame/C# #你脚本的名字!#", false, #在菜单排布的Index 跟着往下写就行#)]
        // public static void Creat#你脚本的名字!#()
        // {
        //     KooScriptsTemplates.CreateMyScript("#你脚本的名字!#.cs",
        //         "Assets/3.Frame/KooFrame/1.FrameEditor/ScriptTemplates/#你设置的模板Index,#-KooFrame__#你脚本的名字!#.cs.txt");
        // }

        #endregion

        [MenuItem("KooFrame/生成框架UnityPackage", false, 100)]
		private static void GeneratorFramePackage()
		{
			KooFrameInstance.GeneratorUnityPackage();
		}

		[MenuItem("KooFrame/存档工具/打开存档路径", false, 1)]
		public static void OpenArchivedDirPath()
		{
			string path = Application.persistentDataPath.Replace("/", "\\");
			System.Diagnostics.Process.Start("explorer.exe", path);
		}

		[MenuItem("KooFrame/存档工具/清空存档", false, 2)]
		public static void CleanSave()
		{
			SaveSystem.DeleteAll();
		}

		[MenuItem("KooFrame/查询工具/查询脚本Static引用", false, 3)]
		public static void StaticReport()
		{
			FindStaticRef.StaticRef();
		}

		[MenuItem("KooFrame/查询工具/对象池状态查看器", false, 4)]
		public static void ShowPoolSystemViewer()
		{
			PoolSystemViewer.ShowExample();
		}
#if ENABLE_ADDRESSABLES
		[MenuItem("KooFrame/资源工具/生成资源引用代码", false, 5)]
		public static void GenerateResReferenceCode()
		{
			GenerateResReferenceCodeTool.GenerateResReferenceCode();
		}
		[MenuItem("KooFrame/资源工具/清理资源引用代码", false, 6)]
		public static void CleanResReferenceCode()
		{
			GenerateResReferenceCodeTool.CleanResReferenceCode();
		}
#endif

		[MenuItem("KooFrame/脚本工具/自定义脚本模板", false, 7)]
		public static void CreateScriptsTemplates()
		{
			EditorWindow.CreateWindow<ScriptsTemplateEditorWindow>();
		}







        [MenuItem("Assets/KooFrame/FrameSetting/CreateFrameSetting", false, 20)]
		private static void CreateFrameSetting()
		{
			FrameSetting.CreateFrameSetting();
		}

		[MenuItem("Assets/KooFrame/LocalizationConfig/CreateLocalizationConfig", false, 20)]
		private static void CreateLocalizationConfig()
		{
		}
	}
}
#endif