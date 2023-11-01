using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace KooFrame.FrameTools
{
    /// <summary>
    /// <para>文件工具类</para>
    /// <para>1.包括了对编辑器中的文件进行操作</para>
    /// <para>2.包括了一些常用的单位 如字节</para>
    /// <para>3.包括了对UnityPackage的简化操作</para>
    /// <para>4. IO 操作</para>
    /// </summary>
    public static class KooFileTool
    {
        #region 计算机单位

        /// <summary>
        /// 字节单位（1字节 = 8位）
        /// </summary>
        public const int Byte = 8;

        /// <summary>
        /// 千字节单位（1 KB = 1024字节）
        /// </summary>
        public const int Kilobyte = 1024;

        /// <summary>
        /// 兆字节单位（1 MB = 1024 KB）
        /// </summary>
        public const int Megabyte = 1024 * Kilobyte;

        #endregion

        #region IO操作

        private static BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// 保存Json
        /// </summary>
        /// <param name="jsonString">Json的字符串</param>
        /// <param name="path">路径</param>
        public static void SaveJson(string jsonString, string path)
        {
            File.WriteAllText(path, jsonString);
        }

        /// <summary>
        /// 读取Json为指定的类型对象
        /// </summary>
        public static T LoadJson<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="saveObject">保存的对象</param>
        /// <param name="path">保存的路径</param>
        public static void SaveFile(object saveObject, string path)
        {
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            // 二进制的方式把对象写进文件
            binaryFormatter.Serialize(f, saveObject);
            f.Dispose();
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <typeparam name="T">加载后要转为的类型</typeparam>
        /// <param name="path">加载路径</param>
        public static T LoadFile<T>(string path) where T : class
        {
            if (!File.Exists(path))
            {
                return null;
            }

            FileStream file = new FileStream(path, FileMode.Open);
            // 将内容解码成对象
            T obj = (T)binaryFormatter.Deserialize(file);
            file.Dispose();
            return obj;
        }

        #endregion

        /// <summary>
        /// 在资源管理器中打开本地数据路径。
        /// </summary>
        public static void OpenLocalPath()
        {
            Application.OpenURL("file:////" + GetLocalPathTo());
        }

        public static void OpenPathFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                //EditorUtility.RevealInFinder(folderPath); // 打开文件夹
                Application.OpenURL("file:////" + GetLocalPathTo() + folderPath);
            }
        }


        /// <summary>
        /// (用处不大的方法)获取本地数据路径的父目录，并在路径末尾添加斜杠。
        /// </summary>
        /// <returns>应用程序数据路径的父目录。</returns>
        public static string GetLocalPathTo()
        {
            // 获取应用程序数据路径
            string dataPath = UnityEngine.Application.dataPath;

            // 获取应用程序数据路径的父目录，并在末尾添加斜杠
            string parentPath = Path.GetDirectoryName(dataPath);
            // 使用 Path.DirectorySeparatorChar 来获取适用于当前操作系统的目录分隔符以替代固定的斜杠。
            string localPathToParent = parentPath + Path.DirectorySeparatorChar;

            return localPathToParent;
        }

        /// <summary>
        /// (用处不大的方法)获取应用程序数据路径，并在路径末尾添加斜杠。
        /// </summary>
        /// <returns>应用程序数据路径。</returns>
        public static string GetAssetsPathTo()
        {
            return UnityEngine.Application.dataPath + "/";
        }


        /// <summary>
        /// 将输入的文本保存到指定的文件路径，并输出日志信息。
        /// </summary>
        /// <param name="path">要保存到的文件路径。</param>
        /// <param name="text">要保存的文本内容。</param>
        public static void SaveTextToFile(string path, string text)
        {
            try
            {
                // 将文本保存到指定的文件路径
                File.WriteAllText(path, text);
                UnityEngine.Debug.Log("文本保存至: " + path);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("文本保存到文件时出错: " + e.Message);
            }
        }


        /// <summary>
        /// 在制定路径创建文件夹
        /// </summary>
        /// <param name="path">指定的路径</param>
        public static void CreateDirectory(string path)
        {
            // 检查目录是否存在，如果不存在，则创建它
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log("KooHelp:文件夹已创建: " + path);
            }
            else
            {
                Debug.Log("KooHelp:文件夹已存在: " + path);
            }
        }


#if UNITY_EDITOR
        /// <summary>
        /// 将指定的资源路径导出为资源包文件，并进行递归导出。
        /// </summary>
        /// <param name="assetPathName">要导出的资源路径。</param>
        /// <param name="fileName">导出的文件名。</param>
        public static void ExportPackage(string assetPathName, string fileName)
        {
            try
            {
                // 使用 AssetDatabase.ExportPackage 方法进行资源包导出
                UnityEditor.AssetDatabase.ExportPackage(assetPathName, fileName,
                    UnityEditor.ExportPackageOptions.Recurse);
                UnityEngine.Debug.Log("UnityPackage包导出至: " + fileName);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("包导出失败: " + e.Message);
            }
        }
#endif
    }
}