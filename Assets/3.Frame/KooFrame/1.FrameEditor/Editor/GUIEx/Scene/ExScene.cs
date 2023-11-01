using UnityEditor;
using UnityEngine;

namespace KooFrame
{
	public class ExScene
	{
#if UNITY_EDITOR //此为宏定义标签 UNITY_EDITOR表示这段代码只在Editor模式下执行 发布后剔除
		public static bool CanChooseInScene = true;
		[InitializeOnLoadMethod]
		static void InitializeOnLoadMethod()
		{
			SceneView.duringSceneGui += delegate (SceneView sceneView)
			{
				Event e = Event.current;
				if (e != null && CanChooseInScene == false)
				{
					//FocusType.Passive表示禁止接受控制焦点 获取它的controllID后即可禁止点击事件穿透下去
					int controlID = GUIUtility.GetControlID(FocusType.Passive);
					if (e.type == EventType.Layout)
					{
						HandleUtility.AddDefaultControl(controlID);
					}
					Handles.BeginGUI();
					float sceneWindowHeight = SceneView.currentDrawingSceneView.position.height;
					GUI.color = Color.red;
					GUI.Label(new Rect(10f, sceneWindowHeight-75f, 200f, 15f), "现在不能在Scene中选择物品哦");
					GUI.color = Color.white;
					if (GUI.Button(new Rect(10f, sceneWindowHeight - 50f, 140f, 20f), "关闭禁止选中物品功能"))
					{
						CanChooseInScene = true;
						Debug.Log("KooHelp:" + "现在可以在场景中选择物品了");
					}
					Handles.EndGUI();
				}
			};
		}
#endif
	}
	//在Scene视图中容易选择到子节点 绑定一个[SelectionBase] 即可定位所有子节点到此对象上
	//[SelectionBase]
	//public class RootScripts : MonoBehaviour
	//{

	//}
}
