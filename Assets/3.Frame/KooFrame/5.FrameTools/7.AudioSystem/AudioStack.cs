//****************** 代码文件申明 ************************
//* 文件：AudioStack                                       
//* 作者：wheat
//* 创建时间：2023/09/29 18:54:33 星期五
//* 描述：用于存储音效信息
//*****************************************************
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KooFrame
{
    [CreateAssetMenu(fileName ="AudioStack",menuName ="ScriptableObject/Audio/new AudioStack")]
    public class AudioStack:ScriptableObject
    {
        #region 字段(均为Private Set)
        [field:SerializeField,LabelText("id")] public int AudioIndex {  get; private set; }
        [field:SerializeField,LabelText("名称")] public string AudioName {  get; private set; }
        [field:SerializeField,LabelText("Clip")] public AudioClip Clip {  get; private set; }
        [field:SerializeField,LabelText("分组")] public AudioMixerGroup AudioGroup {  get; private set; }
        [field:SerializeField,LabelText("音量")] public float Volume {  get; private set; }
        [field:SerializeField,LabelText("音高"),Range(-3,3f)] public float Pitch {  get; private set; }
        [field:SerializeField,LabelText("最低音高"),Range(-3,3f)] public float MinPitch {  get; private set; }
        [field: SerializeField, LabelText("最高音高"), Range(-3, 3f)] public float MaxPitch { get; private set; }
        [field:SerializeField,LabelText("3D音效")] public bool Is3D {  get; private set; }

        #endregion

        #region 构造函数
        public AudioStack()
        {
            Clip = null;
            AudioGroup = null;
            Volume = 1f;
            Pitch = 1f;
            MaxPitch = 1f;
            MinPitch = 1f;
            Is3D = false;
        }
        public AudioStack(int audioIndex,string audioName,AudioClip clip, AudioMixerGroup audioGroup, float volume, float pitch, float minPitch, float maxPitch, bool is3D)
        {
            AudioIndex = audioIndex;
            AudioName = audioName;
            Clip = clip;
            AudioGroup = audioGroup;
            Volume = volume;
            Pitch = pitch;
            MinPitch = minPitch;
            MaxPitch = maxPitch;
            Is3D = is3D;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 在AudioLibrary使用，其他地方尽量不要使用
        /// </summary>
        public void UpdateDatas(int audioIndex, string audioName, AudioClip clip, AudioMixerGroup audioGroup, float volume, float pitch, float minPitch, float maxPitch, bool is3D)
        {
            AudioIndex = audioIndex;
            AudioName = audioName;
            Clip = clip;
            AudioGroup = audioGroup;
            Volume = volume;
            Pitch = pitch;
            MinPitch = minPitch;
            MaxPitch = maxPitch;
            Is3D = is3D;
        }

        #endregion
    }
}

