//****************** 代码文件申明 ***********************
//* 文件：Graph                                       
//* 作者：32867
//* 创建时间：2023/10/25 15:14:11 星期三
//* 描述：
//*****************************************************

using KooFrame.FrameTools;
using UnityEngine;
//隐式的使用类型
using static UnityEngine.Mathf;

namespace GameBuild
{
    public class CPUGraph : MonoBehaviour
    {
        #region 字段

        [SerializeField]
        Transform pointPrefab;

        [SerializeField, Range(10, 200)]
        int resolution = 10;

        [SerializeField, Min(0f)]
        float functionDuration = 1f;

        [SerializeField, Min(0f)]
        private float transitionDuration = 1f;

        private float duration;

        bool transitioning;

        public enum TransitionMode
        {
            Cycle,
            Random
        }

        [SerializeField]
        TransitionMode transitionMode;

        KooMathTool.MathFunctionNameVector3 transitionFunction;


        private Transform[] points;

        [SerializeField]
        KooMathTool.MathFunctionVector2 function;

        [SerializeField]
        KooMathTool.MathFunctionNameVector3 functionNameVector3;

        [SerializeField]
        private float cycle;

        #endregion

        #region Graph生命周期

        private void Awake()
        {
            points = new Transform[resolution * resolution];
            float step = 2f / resolution;
            var scale = Vector3.one * step;
            for (int i = 0; i < points.Length; i++)
            {
                Transform point = points[i] = Instantiate(pointPrefab);

                point.localScale = scale;
                point.SetParent(transform, false);
            }
        }


        private void Update()
        {
            duration += Time.deltaTime;
            if (transitioning)
            {
                if (duration >= transitionDuration)
                {
                    duration -= transitionDuration;
                    transitioning = false;
                }
            }
            else if (duration >= functionDuration)
            {
                duration -= functionDuration;
                transitioning = true;
                transitionFunction = functionNameVector3;
                PickNextFunction();
            }

            if (transitioning)
            {
                UpdateFunctionTransition();
            }
            else
            {
                UpdateMathFunction();
            }
        }

        void PickNextFunction()
        {
            functionNameVector3 = transitionMode == TransitionMode.Cycle
                ? KooMathTool.GetNextMathFunction(functionNameVector3)
                : KooMathTool.GetRandomFunctionNameOtherThan(functionNameVector3);
        }
        
        /// <summary>
        /// 函数变换的更新
        /// </summary>
        void UpdateFunctionTransition()
        {
            KooMathTool.MathFunctionVector3
                from = KooMathTool.GetFunctionVector3(transitionFunction),
                to = KooMathTool.GetFunctionVector3(functionNameVector3);
            //进度为当前时间/转换需要的时间
            float progress = duration / transitionDuration;
            float time = Time.time;
            float step = 2f / resolution;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                }

                float u = (x + 0.5f) * step - 1f;
                float v = (z + 0.5f) * step - 1f;
                points[i].localPosition = KooMathTool.Morph(
                    u, v, time, from, to, progress
                );
            }
        }

        private void UpdateMathFunction()
        {
            var f = KooMathTool.GetFunctionVector3(functionNameVector3);
            float time = Time.time;

            float step = 2f / resolution;
            for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
            {
                if (x == resolution)
                {
                    x = 0;
                    z += 1;
                }

                float u = (x + 0.5f) * step - 1f;
                float v = (z + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, time);
            }
        }

        #endregion

        #region Graph功能

        public float Wave(float x, float t)
        {
            return Sin(PI * (x + t));
        }

        public float MultiWave(float x, float t, float c)
        {
            float y = Sin(PI * (x + t));
            y += 0.5f * Sin(2f * PI * (x + c * t));
            //虽然除法比乘法需要更多的工作  但类似 1f / 2f 这类常量表达式 已经被编译器简化成了单个数字
            return y * (2f / 3f);
        }

        public static float Ripple(float x, float t)
        {
            float d = Abs(x);
            float y = Sin(PI * (4f * d - t));
            return y / (1f + 10f * d);
        }

        #endregion
    }
}