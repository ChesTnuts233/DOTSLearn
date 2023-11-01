using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameBuild.DOTS
{
    public struct SpawnJob : IJobParallelFor
    {
        /// <summary>
        /// 用于克隆的原型Entity
        /// </summary>
        public Entity prototype;

        public int halfCountX; // 沿X轴方向的立方体数量的一半
        public int halfCountZ; // 沿Z轴方向的立方体数量的一半

        public EntityCommandBuffer.ParallelWriter Ecb; //并行写入实体命令缓冲

        public void Execute(int index)
        {
            var e = Ecb.Instantiate(index, prototype); //克隆原型
            Ecb.SetComponent(index, e, new LocalToWorld { Value = ComputeTransform(index) });
            Ecb.AddComponent(index, e, new RotateSpeed
            {
                rotateSpeed = 100f
            });
        }

        /// <summary>
        /// 计算立方体的变换矩阵
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float4x4 ComputeTransform(int index)
        {
            // 因为立方体之间的位置是周期性的 将index的值限制在(halfCountX * 2)内 来循环放置
            int x = index % (halfCountX * 2) - halfCountX;
            // z轴从零开始递增 每次增加halfCountX * 2 就会有新的一行 z轴的增加是线性的 只需要完成一次周期 就能放完z轴
            int z = index / (halfCountX * 2) - halfCountZ;
            // 创建4X4矩阵  x 和 z分别间隔 0.1
            float4x4 M = float4x4.TRS(
                new float3(x * 1.1f, 0, z * 1.1f),
                quaternion.identity,
                new float3(1));
            return M;
        }
    }

    [BurstCompile]
    partial struct WaveCubeEntityJob : IJobEntity
    {
        /// <summary>
        /// 计算波浪的时间
        /// </summary>
        [ReadOnly] public float elapsedTime;

        void Execute(ref LocalToWorld transform, ref RotateSpeed rotateSpeed)
        {
            //更新立方体位置
            var distance = math.distance(transform.Position, float3.zero);
            //根据距离和时间计算新位置
            float3 newPos = transform.Position + new float3(0, 1, 0) * math.sin(elapsedTime * 3f + distance * 0.2f);
            //设置新的位置
            transform.Value = float4x4.Translate(newPos);
            // 设置自旋转
            var rotationSpeed = rotateSpeed.rotateSpeed; // 获取自旋转速度
            
            //生成旋转矩阵
            var rotationMatrix = float4x4.RotateY(rotationSpeed * elapsedTime);
            //矩阵相乘
            transform.Value = math.mul(rotationMatrix, float4x4.Translate(newPos));
        }
    }

    // 指定需要匹配的查询以更新，属于GraphicsLesson1SystemGroup组
    [UpdateInGroup(typeof(CubeRotateSysytemGroup))]
    public partial struct WaveCubesMoveSystem : ISystem
    {
        // 用于性能分析的标记
        static readonly ProfilerMarker profilerMarker = new ProfilerMarker("WaveCubeEntityJobs");

        // 执行每帧的逻辑
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            using (profilerMarker.Auto()) // 在性能分析中创建标记
            {
                var job = new WaveCubeEntityJob() { elapsedTime = (float)SystemAPI.Time.ElapsedTime }; // 创建波浪效果的作业
                job.ScheduleParallel();                                                                // 并行执行作业
            }
        }
    }

    public class CreateCubesWithMono : MonoBehaviour
    {
        [Range(10, 100)]
        public int xHalfCount = 40;

        [Range(10, 100)]
        public int zHalfCount = 40;

        public Mesh mesh;
        public Material material;

        void Start()
        {
            var world = World.DefaultGameObjectInjectionWorld; //获取默认的ECS世界
            var entityManager = world.EntityManager;           //获取实体管理器

            //创建用于命令缓冲的实例
            EntityCommandBuffer ecbJob = new EntityCommandBuffer(Allocator.TempJob);

            //配置渲染过滤器
            var filterSettings = RenderFilterSettings.Default;
            filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
            filterSettings.ReceiveShadows = false;

            var renderMeshArray = new RenderMeshArray(new[] { material }, new[] { mesh });
            var renderMeshDescription = new RenderMeshDescription
            {
                FilterSettings = filterSettings,
                LightProbeUsage = LightProbeUsage.Off,
            };

            //创建实体
            var prototype = entityManager.CreateEntity();
            RenderMeshUtility.AddComponents(
                prototype,
                entityManager,
                renderMeshDescription,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            //生成的线程调度
            var spawnJob = new SpawnJob
            {
                prototype = prototype,
                Ecb = ecbJob.AsParallelWriter(),
                halfCountX = xHalfCount,
                halfCountZ = zHalfCount
            };
            // 执行生成立方体的作业
            var spawnHandle = spawnJob.Schedule(4 * xHalfCount * zHalfCount, 128);

            //等待作业完成
            spawnHandle.Complete();
            //播放命令缓冲以创建立方体实例
            ecbJob.Playback(entityManager);
            //释放命令缓冲
            ecbJob.Dispose();
            //销毁原型Entity
            entityManager.DestroyEntity(prototype);
        }
    }
}