//****************** 代码文件申明 ***********************
//* 文件：Graph                                       
//* 作者：32867
//* 创建时间：2023/10/25 15:14:11 星期三
//* 描述：使用GPU来绘制图形
//*****************************************************

using KooFrame.FrameTools;
using UnityEngine;
using static UnityEngine.Mathf;

namespace GameBuild
{
    public class GPUGraph : MonoBehaviour
    {
        #region 字段

        [SerializeField]
        private ComputeShader computeShader;

        static readonly int PositionsId = Shader.PropertyToID("_Positions"),
                            ResolutionId = Shader.PropertyToID("_Resolution"),
                            StepId = Shader.PropertyToID("_Step"),
                            TimeId = Shader.PropertyToID("_Time");


        [SerializeField]
        Material material;

        [SerializeField]
        Mesh mesh;

        [SerializeField, Range(10, 200)]
        int resolution = 10;

        // [SerializeField, Min(0f)]
        // float functionDuration = 1f;
        //
        // [SerializeField, Min(0f)]
        // private float transitionDuration = 1f;

        private float _duration;

        private bool _transitioning;

        [SerializeField]
        private float cycle;

        private enum TransitionMode
        {
            Cycle,
            Random
        }

        [SerializeField]
        TransitionMode transitionMode;


        [SerializeField]
        KooMathTool.MathFunctionVector2 function;


        [SerializeField]
        KooMathTool.MathFunctionNameVector3 functionNameVector3;

        KooMathTool.MathFunctionNameVector3 transitionFunction;


        ComputeBuffer positionsBuffer;

        #endregion


        #region Graph生命周期

        private void Awake()
        {
            positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
        }

        private void OnEnable()
        {
            positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
        }

        void OnDisable()
        {
            positionsBuffer.Release();
            positionsBuffer = null;
        }

        private void Update()
        {
            UpdateFunctionOnGPU();
        }

        private void PickNextFunction()
        {
            functionNameVector3 = transitionMode == TransitionMode.Cycle
                ? KooMathTool.GetNextMathFunction(functionNameVector3)
                : KooMathTool.GetRandomFunctionNameOtherThan(functionNameVector3);
        }

        #endregion

        #region Graph功能

        void UpdateFunctionOnGPU()
        {
            float step = 2f / resolution;
            //把值传递到着色器中去计算
            computeShader.SetInt(ResolutionId, resolution);
            computeShader.SetFloat(StepId, step);
            computeShader.SetFloat(TimeId, Time.time);

            //设置缓冲区
            computeShader.SetBuffer(0, PositionsId, positionsBuffer);
            //调用具有四个整数参数的计算着色器来运行内核 第一个是内核索引 另外3个是要运行的组数量，同样按维度拆分 将1用于所有的维度意味着仅计算第一组8x8位置

            int groups = Mathf.CeilToInt(resolution / 8f);

            computeShader.Dispatch(0, groups, groups, 1);

            material.SetBuffer(PositionsId, positionsBuffer);
            material.SetFloat(StepId, step);

            var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));

            Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
        }

        #region 临时的内部函数

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

        #endregion
    }
}