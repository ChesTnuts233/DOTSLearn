// //****************** 代码文件申明 ************************
// //* 文件：AudioLibrary                                       
// //* 作者：wheat
// //* 创建时间：2023/10/01 10:04:00 星期日
// //* 描述：负责存储和管理AudioStack
// //*****************************************************
// using UnityEngine;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using OfficeOpenXml;
// using System.IO;
// using OfficeOpenXml.Table;
// using UnityEngine.Audio;
//
// #if UNITY_EDITOR
//
// using UnityEditor;
// using UnityEditor.AddressableAssets.Settings;
//
// namespace KooFrame
// {
//     [CreateAssetMenu(fileName = "AudioLibrary", menuName = "ScriptableObject/Audio/new AudioLibrary")]
//     public class AudioLibrary : ScriptableObject
//     {
//         [SerializeField, LabelText("SO文件保存路径"), FoldoutGroup("信息预览")] private string savePath;
//         [SerializeField, LabelText("AB包设置"), FoldoutGroup("信息预览")] private AddressableAssetSettings addressableAssetSettings;
//         [SerializeField, LabelText("AB包分组"), FoldoutGroup("信息预览")] private AddressableAssetGroup addressableGroup;
//         [field: SerializeField, LabelText("音效库"), FoldoutGroup("信息预览")] public List<AudioStack> Audioes { get; private set; }
//         [field: SerializeField, LabelText("音源库"), FoldoutGroup("参数设置")] public List<AudioClip> AudioClips { get; private set; }
//         [field: SerializeField, LabelText("音轨库"), FoldoutGroup("参数设置")] public List<AudioMixerGroup> AudioGroups { get; private set; }
//
//         private enum ExcelLabel
//         {
//             AudioIndex = 0, AudioName = 1, AudioGroup = 2, Volume = 3, Pitch = 4, MinPitch = 5, MaxPitch = 6, Is3D = 7,
//         }
//
//         #region 基础方法
//
//
//         /// <summary>
//         /// 读取信息生成音效文件数据
//         /// </summary>
//         /// <param name="datas"></param>
//         public void LoadAudioes(string[,] datas)
//         {
//             //添加已有的
//             Dictionary<int, AudioStack> oldAudioDic = new Dictionary<int, AudioStack>();
//             foreach (var audio in Audioes)
//             {
//                 oldAudioDic.Add(audio.AudioIndex, audio);
//             }
//
//             int row = datas.GetLength(0);
//             //依次读取数据
//             //跳过首行，首行为标题
//             for (int r = 1; r < row; r++)
//             {
//                 //如果为空就跳过
//                 if (datas[r, 0] == "" || datas[r, 0] == null)
//                 {
//                     continue;
//                 }
//
//                 #region 初始化变量
//                 int audioIndex;
//                 string audioName;
//                 AudioClip audioClip;
//                 AudioMixerGroup audioGroup;
//                 float volume;
//                 float pitch;
//                 float minPitch;
//                 float maxPitch;
//                 bool is3D;
//
//                 #endregion
//
//                 //音效id
//                 if (!int.TryParse(datas[r, (int)ExcelLabel.AudioIndex], out audioIndex))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabel.AudioIndex]);
//                     Debug.Log("在读取第" + r + "个音效id时发生错误");
//                     continue;
//                 }
//
//                 //音效名称
//                 audioName = datas[r, (int)ExcelLabel.AudioName];
//
//                 //音效Clip
//                 if (audioIndex < 0 || audioIndex >= AudioClips.Count)
//                 {
//                     Debug.Log("在读取第" + r + "个音效Clip时发生错误");
//                     continue;
//                 }
//
//                 audioClip = AudioClips[audioIndex];
//
//                 //音轨
//                 switch (datas[r, (int)ExcelLabel.AudioGroup])
//                 {
//                     case "SFX":
//                         audioGroup = AudioGroups[0];
//                         break;
//                     case "BGM":
//                         audioGroup = AudioGroups[1];
//                         break;
//                     default:
//                         audioGroup = null;
//                         break;
//                 }
//
//                 //音量
//                 if (!float.TryParse(datas[r, (int)ExcelLabel.Volume], out volume))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabel.Volume]);
//                     Debug.Log("在读取第" + r + "个音量时发生错误");
//                     continue;
//                 }
//
//                 //音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabel.Pitch], out pitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabel.Pitch]);
//                     Debug.Log("在读取第" + r + "个音高时发生错误");
//                     continue;
//                 }
//
//                 //最低音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabel.MinPitch], out minPitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabel.MinPitch]);
//                     Debug.Log("在读取第" + r + "个最低音高时发生错误");
//                     continue;
//                 }
//
//                 //最高音高
//                 if (!float.TryParse(datas[r, (int)ExcelLabel.MaxPitch], out maxPitch))
//                 {
//                     Debug.Log(datas[r, (int)ExcelLabel.MaxPitch]);
//                     Debug.Log("在读取第" + r + "个最高音高时发生错误");
//                     continue;
//                 }
//
//                 //是否为3D音效
//                 is3D = datas[r, (int)ExcelLabel.Is3D] == "是";
//
//                 //如果已经有audioStack了
//                 if (oldAudioDic.TryGetValue(audioIndex, out AudioStack audioStack))
//                 {
//                     //那就找到然后更新数据
//                     audioStack.UpdateDatas(audioIndex, audioName, audioClip, audioGroup, volume, pitch, minPitch, maxPitch, is3D);
//                 }
//                 else
//                 {
//                     //没有就新创一个
//                     audioStack = CreateAudioStack(audioIndex, audioName, audioClip, audioGroup, volume, pitch, minPitch, maxPitch, is3D);
//
//                     //添加入列表
//                     Audioes.Add(audioStack);
//                 }
//             }
//
//
//             //保存SO文件
//             EditorUtility.SetDirty(this);
//         }
//
//         /// <summary>
//         /// 创建AudioStack的SO文件
//         /// </summary>
//         private AudioStack CreateAudioStack(int audioIndex, string audioName, AudioClip clip, AudioMixerGroup audioGroup, float volume, float pitch, float minPitch, float maxPitch, bool is3D)
//         {
//             //初始化
//             AudioStack audioStack = ScriptableObject.CreateInstance<AudioStack>();
//             audioStack.UpdateDatas(audioIndex, audioName, clip, audioGroup, volume, pitch, minPitch, maxPitch, is3D);
//
//             //获取保存路径
//             string targetPath = savePath;
//
//             if (audioGroup != null)
//             {
//                 targetPath += "/" + audioGroup.name;
//             }
//
//             //如果路径不存在就创建文件夹
//             if (!File.Exists(targetPath))
//             {
//                 Directory.CreateDirectory(targetPath);
//             }
//             targetPath += "/AS" + audioIndex + audioName;
//
//             //防止路径冲突
//             targetPath = AvoidPathCollision(targetPath, ".asset", 0);
//
//             //保存到目标路径
//             EditorUtility.SetDirty(audioStack);
//
//             AssetDatabase.CreateAsset(audioStack, targetPath);
//             AssetDatabase.SaveAssets();
//
//             AddToAddressable(targetPath, "AS" + audioIndex + audioName);
//
//             return audioStack;
//         }
//         /// <summary>
//         /// 将文件添加到Addressable
//         /// </summary>
//         /// <param name="path">文件路径</param>
//         /// <param name="soName">文件名称</param>
//         private void AddToAddressable(string path, string soName)
//         {
//             AddressableAssetEntry entry = addressableAssetSettings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), addressableGroup);
//             entry.address = soName;
//
//             addressableAssetSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true, true);
//         }
//         #endregion
//
//
//         #region 读表
//
//
//         /// <summary>
//         /// 打开Excel加载物品信息
//         /// </summary>
//         [Button("打开Excel加载"), FoldoutGroup("信息预览"), PropertySpace(5f, 5f)]
//         public void OpenExcelLoad()
//         {
//             bool success = ReadExcelPath(out string newPath);
//
//             if (success)
//             {
//                 AnalyzeData(newPath);
//             }
//         }
//
//         /// <summary>
//         /// 获取路径
//         /// </summary>
//         /// <param name="newPath"></param>
//         /// <returns></returns>
//         protected bool ReadExcelPath(out string newPath)
//         {
//             newPath = EditorUtility.OpenFilePanel("选择表格", "", "");
//
//             if (newPath == "")
//             {
//                 return false;
//             }
//
//             if (!newPath.Contains("xlsx"))
//             {
//                 EditorUtility.DisplayDialog("表格读取失败", "不是正确的文件格式，请读取xlsx的格式的文件", "确认");
//                 return false;
//             }
//
//             return true;
//         }
//
//         protected virtual bool AnalyzeData(string path)
//         {
//             string[,] datas;
//             int Row = 0;
//             int Column = 0;
//             //读取路径
//             FileInfo fileInfo = new FileInfo(path);
//
//             using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
//             {
//                 //先获取工作区
//                 ExcelWorksheet excelWorksheet = null;
//
//                 //找到首个不为空的表
//                 foreach (var sheet in excelPackage.Workbook.Worksheets)
//                 {
//                     if (sheet.Dimension != null)
//                     {
//                         excelWorksheet = sheet;
//                     }
//                 }
//
//                 //防止报空
//                 if (excelWorksheet.Dimension == null)
//                 {
//                     Debug.Log("表格为空");
//                     return false;
//                 }
//
//                 //获取行
//                 Row = excelWorksheet.Dimension.End.Row;
//                 //获取列(这边跳过一列)
//                 Column = excelWorksheet.Dimension.End.Column;
//
//                 //生成数据库
//                 datas = new string[Row, Column];
//
//                 for (int i = excelWorksheet.Dimension.Start.Row; i <= Row; i++)
//                 {
//                     //如果首个元素为空就跳过该行
//                     var _tableValue = excelWorksheet.GetValue(i, 1);
//                     if (_tableValue == null)
//                     {
//                         continue;
//                     }
//
//                     //在第一行，第i列拿到需要的字段
//                     //遍历每一行的每一列
//                     for (int j = excelWorksheet.Dimension.Start.Column;
//                          j <= Column;
//                          j++)
//                     {
//                         //每一列的第一行 读取当前位置的文本
//                         var tableValue = excelWorksheet.GetValue(i, j);
//                         //赋值
//                         datas[i - 1, j - 1] = tableValue.ToString();
//
//                     }
//
//                 }
//             }
//
//             //生成音效信息表
//             LoadAudioes(datas);
//
//             return true;
//         }
//
//         /// <summary>
//         /// 防止路径冲突
//         /// </summary>
//         /// <param name="path">路径</param>
//         /// <param name="format">格式</param>
//         /// <param name="id">id</param>
//         private string AvoidPathCollision(string path, string format, int id = 0)
//         {
//             //先获取路径
//             var res = path;
//             //如果id不为0那就加入(id)
//             if (id != 0)
//             {
//                 res += "(" + id + ")";
//             }
//
//             //最后添加后缀
//             res += format;
//
//             //如果文件已存在那就继续更改
//             if (File.Exists(res))
//             {
//                 return AvoidPathCollision(path, format, id + 1);
//             }
//             else
//             {
//                 //不存在就输出
//                 return res;
//             }
//         }
//
//
//         #endregion
//
//
//     }
//
// }
//
//
// #endif
//
