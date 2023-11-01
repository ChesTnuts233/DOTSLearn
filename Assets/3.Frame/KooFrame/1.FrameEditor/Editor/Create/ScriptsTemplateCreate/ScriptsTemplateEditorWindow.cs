//****************** 代码文件申明 ************************
//* 文件：ScriptsTemplateEditorWindow                      
//* 作者：32867
//* 创建时间：2023年09月06日 星期三 17:37
//* 描述：用于自己创建自定义的脚本模板
//*****************************************************

using System;
using System.IO;
using KooFrame.BaseSystem;
using KooFrame.FrameTools;
using UnityEditor;
using UnityEngine;

namespace KooFrame
{

	public class ScriptsTemplateEditorWindow : EditorWindow
	{
		private string templateIndex;
		private string templateName = "";
		private string template = "";

		private string scriptsTop = "//****************** 代码文件申明 ***********************\n" +
									"//* 文件：#SCRIPTNAME#\n" +
									"//* 作者：#AUTHORNAME#\n" +
									"//* 创建时间：#CREATETIME#\n" +
									"//* 描述：\n" +
									"//*****************************************************\n";
		private void OnEnable()
		{
			template = scriptsTop;
			templateIndex = "eg 08 仔细看一下目录里的模板序号 跟在后面写";
			templateName = "可以简要概述模板内容的名字";
		}

		private void OnGUI()
		{
			GUILayout.Label("脚本模板序号:");

			templateIndex = EditorGUILayout.TextArea(templateIndex);

			GUILayout.Label("脚本模板名称:");

			// 脚本模板名称
			templateName = EditorGUILayout.TextArea(templateName, GUILayout.Height(30));

			GUILayout.Label("脚本模板内容\n(建议在Text内写好直接复制进来,下方有模板头 复制上来):");

			// 创建模板内容
			template = EditorGUILayout.TextArea(template, GUILayout.Height(200));
			GUILayout.Space(20);
			if (GUILayout.Button("01 保存代码模板"))
			{
				SaveTextToFile(templateName, template);
				//KooScriptsTemplates.CreateMyScript("CreateTemplate"+templateIndex,);
			}
			GUILayout.Space(20);
			// 创建文本框
			GUILayout.Label("代码头模板：");
			GUILayout.TextArea(scriptsTop, GUILayout.Height(100));

			GUILayout.Space(5);

			if (GUILayout.Button("02 去编写MenuItem(右键菜单)"))
			{
				UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(KooFrameInstance.FrameInfo.AssetPath +
					"/1.FrameEditor/Editor/FrameMenu.cs", typeof(UnityEngine.Object));
				AssetDatabase.OpenAsset(asset, 15);
			}
			GUILayout.Space(5);
			if (GUILayout.Button("打开脚本模板所在文件夹"))
			{
				KooFileTool.OpenPathFolder(KooFrameInstance.FrameInfo.AssetPath +
					"/1.FrameEditor/ScriptTemplates/");
			}
			GUILayout.Space(5);
			GUILayout.Label("脚本创建在"+KooFrameInstance.FrameInfo.AssetPath +
					"/1.FrameEditor/ScriptTemplates/ 里\n 自己看一下序号 后面我再研究一下怎么动态根据文件内容生成");
			GUILayout.Label("写MenuItem的地方集中在"+ KooFrameInstance.FrameInfo.AssetPath +
					"/1.FrameEditor/Editor/FrameMenu.cs ");



		}

		private void SaveTextToFile(string name, string text)
		{
			string filePath = EditorUtility.SaveFilePanel("保存模板至", KooFrameInstance.FrameInfo.AssetPath +
					"/1.FrameEditor/ScriptTemplates", templateIndex + "-KooFrame__" + name, "cs.txt");

			if (!string.IsNullOrEmpty(filePath))
			{
				try
				{
					// 将文本内容写入txt文件
					File.WriteAllText(filePath, text);
					Debug.Log("模板保存至: " + filePath);
				}
				catch (Exception e)
				{
					Debug.LogError("Error saving text: " + e.Message);
				}
			}
		}
	}
}