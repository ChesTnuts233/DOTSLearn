using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

namespace KooFrame
{
    public class ExCreateInHierarchy
    {
        [MenuItem("GameObject/UI/Image_NoRaycast ([无射线检测]优先使用)")]
        static void CreateImage()
        {
            if (Selection.activeTransform)
            {
                if (Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    Image image = new GameObject("Image").AddComponent<Image>();
                    image.raycastTarget = false;
                    image.transform.SetParent(Selection.activeTransform, false);
                    //设置选中状态
                    Selection.activeTransform = image.transform;
                }
            }
        }

        [MenuItem("GameObject/UI/RawImage_NoRaycast")]
        static void CreateRawImage()
        {
            if (Selection.activeTransform)
            {
                if (Selection.activeTransform.GetComponentInParent<Canvas>())
                {
                    RawImage rawImage = new GameObject("RawImage").AddComponent<RawImage>();
                    rawImage.raycastTarget = false;
                    rawImage.transform.SetParent(Selection.activeTransform, false);
                    //设置选中状态
                    Selection.activeTransform = rawImage.transform;
                }
            }
        }

		//Hierarchy面板下
		[MenuItem("GameObject/KooFrame/Primitive/Cube(0,1,0)", false, 0)]
		static void HierarchyCreateCube()
		{
			GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			myCube.transform.SetPositionAndRotation(Vector3.up, Quaternion.identity);
		}

		[MenuItem("GameObject/KooFrame/Primitive/Plane50x1x50", false, 1)]
		static void HierarchyCreatePlane()
		{
			GameObject myPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			myPlane.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			myPlane.transform.localScale = new Vector3(50, 1, 50);
		}

		[MenuItem("GameObject/KooFrame/UI/UICanvas", false, 1)]
		static void ProjectCreateKooCanvas()
		{
			GameObject myCanvas = Object.Instantiate(Resources.Load<GameObject>("UICanvas"));
			myCanvas.transform.name = "KooTestCanvas";
		}

		[MenuItem("GameObject/KooFrame/Tools/Scene跟踪MainCamera", false, 0)]
		public static void FollowGameCamera()
		{
			var camera = Camera.allCameras.FirstOrDefault();
			SceneView.lastActiveSceneView.LookAt(camera.transform.position, camera.transform.rotation, 0.01f);
		}

		[MenuItem("GameObject/KooFrame/Tools/是否禁止在Scene中选择对象", false, 1)]
		static void CanChooseInScene()
		{
			if (ExScene.CanChooseInScene == false)
			{
				ExScene.CanChooseInScene = true;
				Debug.Log("KooHelp:" + "已经可以在场景中选择对象了哦~");
			}
			else
			{
				ExScene.CanChooseInScene = false;
				Debug.Log("KooHelp:" + "已经禁止在场景中选择对象了哦~");
			}
		}

		[MenuItem("GameObject/KooFrame/Lock ObjectFlag/Lock", false, 0)]
		static void Lock()
		{
			if (Selection.gameObjects != null)
			{
				foreach (var gameObject in Selection.gameObjects)
				{
					gameObject.hideFlags = HideFlags.NotEditable;
				}
			}
		}
		[MenuItem("GameObject/KooFrame/Lock ObjectFlag/UnLock", false, 1)]
		static void UnLock()
		{
			if (Selection.gameObjects != null)
			{
				foreach (var gameObject in Selection.gameObjects)
				{
					gameObject.hideFlags = HideFlags.None;
				}
			}
		}

		// [MenuItem("GameObject/UI/Frame_Panel")]
		// static void CreatePanel()
		// {
		//     if (Selection.activeTransform)
		//     {
		//         if (Selection.activeTransform.GetComponentInParent<Canvas>())
		//         {
		//             GameObject rawImage = new GameObject("Panel").AddComponent<Image>();
		//             rawImage.raycastTarget = false;
		//             rawImage.transform.SetParent(Selection.activeTransform, false);
		//             //设置选中状态
		//             Selection.activeTransform = rawImage.transform;
		//         }
		//     }
		// }
	}
}