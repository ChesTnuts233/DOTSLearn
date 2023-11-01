using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 性能数据的显示模式
    /// </summary>
    public enum DisplayMode
    {
        FPS,
        MS,
    }

    /// <summary>
    /// 性能数据显示工具 支持显示最好值与最差值 并可以显示FPS和MS
    /// 左Ctrl+F 显示FPS 左Ctrl+M 显示MS
    /// </summary>
    public class FrameRateHelp : MonoBehaviour
    {
        /// <summary>
        /// 更新显示帧率的时间间隔
        /// </summary>
        [SerializeField, Range(0.1f, 2f)]
        float sampleDuration = 1f;

        /// <summary>
        /// 帧间间隔
        /// </summary>
        private float _duration;

        /// <summary>
        /// 最好帧间隔
        /// </summary>
        private float _bestDuration = float.MaxValue;

        /// <summary>
        /// 最差帧间隔
        /// </summary>
        private float _worstDuration = 0;

        /// <summary>
        /// 帧数
        /// </summary>
        private int _frames = 0;

        /// <summary>
        /// 平均帧数
        /// </summary>
        private float _curValue = 0;

        /// <summary>
        /// 最好帧数
        /// </summary>
        private float _bestValue;

        /// <summary>
        /// 最差帧数
        /// </summary>
        private float _worstValue;

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [SerializeField]
        private bool isHide;

        [SerializeField]
        DisplayMode displayMode = DisplayMode.FPS;

        private GUIStyle style = new GUIStyle();

        /// <summary>
        /// 是否控制目标帧率
        /// </summary>
        [SerializeField]
        private bool isControlTargetFrameRate = false;

        /// <summary>
        /// 限制最大帧率
        /// </summary>
        [SerializeField]
        private int maxFrameRate;


        private void Awake()
        {
            Application.targetFrameRate = isControlTargetFrameRate ? maxFrameRate : -1;
        }

        private void Start()
        {
            style.fontSize = 13;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.red;
        }

        private void Update()
        {
            //打包后使用键盘控制模式切换
            if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftControl))
            {
                displayMode = DisplayMode.FPS;
            }

            if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftControl))
            {
                displayMode = DisplayMode.MS;
            }

            if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftControl))
            {
                isControlTargetFrameRate = !isControlTargetFrameRate;
            }

            if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftControl))
            {
                isHide = !isHide;
            }

            Application.targetFrameRate = isControlTargetFrameRate ? 100 : -1;

            //帧时间
            float frameDuration = Time.unscaledDeltaTime;
            //更新帧
            _frames++;
            _duration += frameDuration;

            if (frameDuration < _bestDuration)
            {
                _bestDuration = frameDuration;
            }

            if (frameDuration > _worstDuration)
            {
                _worstDuration = frameDuration;
            }

            //每隔sampleDuration刷新帧数数值
            if (_duration >= sampleDuration)
            {
                if (displayMode == DisplayMode.FPS)
                {
                    _bestValue = 1f / _bestDuration;
                    _curValue = _frames / _duration;
                    _worstValue = 1f / _worstDuration;
                }
                else
                {
                    _bestValue = 1000f * _bestDuration;
                    _curValue = 1000f * _duration / _frames;
                    _worstValue = 1000f * _worstDuration;
                }

                _frames = 0;
                _duration = 0f;
                _bestDuration = float.MaxValue;
                _worstDuration = 0f;
            }
        }

        private void OnGUI()
        {
            if (!isHide)
            {
                GUILayout.BeginVertical("box");
                if (displayMode == DisplayMode.FPS)
                {
                    GUILayout.Label("Best  : " + _bestValue, style);
                    GUILayout.Label("FPS   : " + _curValue, style);
                    GUILayout.Label("Worst: " + _worstValue, style);
                }
                else
                {
                    GUILayout.Label("Best  : " + _bestValue, style);
                    GUILayout.Label("MS    : " + _curValue, style);
                    GUILayout.Label("Worst: " + _worstValue, style);
                }

                GUILayout.EndVertical();
            }
        }
    }
}