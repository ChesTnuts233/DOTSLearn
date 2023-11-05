Shader "Graph/Point Surface GPU"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {


        CGPROGRAM
        // 这条编译器指令 杂注 表示 指示着色器编译器生成具有标准照明和完全支持阴影的表面着色器
        //“编译指示”一词来自希腊语，指的是一个动作或需要做的事情。它在许多编程语言中用于发出特殊的编译器指令。
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        //表示表面着色器需要调用每个顶点的 ConfigureProcedural 函数
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural

        // 这条指令为着色器的目标级别和质量设置了最小值
        //#pragma target 3.0
        #pragma target 4.5

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
			StructuredBuffer<float3> _Positions;
        #endif

        float _Step;

        void ConfigureProcedural()
        {
            #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
                float3 position = _Positions[unity_InstanceID];

				unity_ObjectToWorld = 0.0;
				unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
				unity_ObjectToWorld._m00_m11_m22 = _Step;
            #endif
        }

        // SurfaceOutputStandard 为表面配置数据
        // inout 表示它既传递给函数又作用于函数的结果
        void ConfigureSurface(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
            surface.Smoothness = _Smoothness;
        }
        ENDCG
    }
    FallBack "Diffuse"

}