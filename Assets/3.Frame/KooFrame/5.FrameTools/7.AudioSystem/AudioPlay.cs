//****************** 代码文件申明 ************************
//* 文件：AudioPlay                                       
//* 作者：wheat
//* 创建时间：2023/09/29 18:39:23 星期五
//* 描述：音效播放
//*****************************************************
using UnityEngine;
using System;
using UnityEngine.Audio;

namespace KooFrame
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlay : MonoBehaviour
    {
        private AudioSource m_AudioSource;
        private event Action onVanish;

        private void Awake()
        {
            InitAudio();
        }
        // private void OnEnable()
        // {
        //     EventSystem.AddEventListener(EventTags.UnloadSceneEvent, OnUnloadSceneEvent);
        // }
        // private void OnDisable()
        // {
        //     EventSystem.RemoveEventListener(EventTags.UnloadSceneEvent, OnUnloadSceneEvent);
        // }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitAudio()
        {
            //获取AudioSource
            if(m_AudioSource == null)
            {
                m_AudioSource = GetComponent<AudioSource>();
            }

            //初始化属性
            m_AudioSource.playOnAwake = false;
            m_AudioSource.loop = false;
            m_AudioSource.clip = null;
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioStack">音效的Stack</param>
        /// <param name="callBack">回调函数</param>
        public void PlaySound(AudioStack audioStack,Action callBack =null)
        {
            //如果什么都没有就return
            if (audioStack == null) return;

            float pitch = 1f;

            if(audioStack.MaxPitch != 1f||audioStack.MinPitch == 1f)
            {
                pitch = UnityEngine.Random.Range(audioStack.MinPitch, audioStack.MaxPitch);
            }

            PlaySound(audioStack.Clip, audioStack.AudioGroup, pitch,audioStack.Is3D,callBack);
        }
        /// <summary>
        /// 播放声音
        /// (不推荐直接使用这个)
        /// </summary>
        /// <param name="audioClip">声音的Clip</param>
        /// <param name="group">输出的Group</param>
        /// <param name="pitch">音高</param>
        /// <param name="callBack">回调函数</param>
        public void PlaySound(AudioClip audioClip,AudioMixerGroup group,float pitch =1f,bool is3D = false,Action callBack = null)
        {
            if(m_AudioSource==null) InitAudio();

            onVanish += callBack;

            m_AudioSource.outputAudioMixerGroup = group;

            m_AudioSource.clip = audioClip;

            m_AudioSource.pitch = pitch;

            m_AudioSource.spatialBlend = is3D ? 1f : 0f;

            m_AudioSource.Play();
        }

        private void Update()
        {
            if(!m_AudioSource.isPlaying&&m_AudioSource.clip!=null)
            {
                Vanish();
            }
        }
        /// <summary>
        /// 卸载场景事件 
        /// </summary>
        private void OnUnloadSceneEvent()
        {
            Vanish();
        }
        /// <summary>
        /// 消失
        /// </summary>
        private void Vanish()
        {
            //如果有事件的话调用
            onVanish?.Invoke();
            //清空事件
            onVanish = null;
            //清空clip
            m_AudioSource.clip = null;
            //推入对象池
            PoolSystem.PushGameObject(gameObject);
        }

    }
}

