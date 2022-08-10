#if OPTIMIZERS_DOTS_IMPORTED

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersManager
    {

        private NativeList<int> dots_VisibleIds;
        private NativeList<int> dots_HittedId;
        private NativeList<float> dots_DelayElapsed;
        private NativeList<JobHandle> dots_JobHandles;
        private NativeList<JobHandle> dots_JobHandlesTr;
        private NativeArray<float3> dots_Rays;

        private NativeArray<RaycastHit> dots_HitsResults;
        private NativeArray<RaycastCommand> dots_RayCommands;

        private NativeList<float3> dots_TransparentHitPoints;

        private NativeArray<RaycastHit> dots_TrHitsResults;
        private NativeArray<RaycastCommand> dots_TransparentRayCommands;

        private int dots_Progressed;



        private void DOTS_ScheduleRaycasting()
        {
            dots_JobHandles.Clear();
            dots_HittedId.Clear();

            // Scheduling Raycasting Jobs ------------------------------
            CreateRayCommandsJob rayJobs = new CreateRayCommandsJob
            {
                position = MainCamera.transform.position,
                rotation = MainCamera.transform.rotation,

                rayCastCycleOffset = dots_Progressed,
                rayDirs = dots_Rays,
                maxDistance = MainCamera.farClipPlane,
                mask = ProgressiveCullingMask,
                rayCommands = dots_RayCommands
            };

            dots_JobHandles.Add(rayJobs.Schedule(ProgressiveResponseQuality, 64, default));

            // Triggering Jobs ------------------------------
            dots_Progressed += ProgressiveResponseQuality;
            if (dots_Progressed >= (dots_Rays.Length - ProgressiveResponseQuality)) dots_Progressed = 0;

            JobHandle.CompleteAll(dots_JobHandles);

            dots_JobHandles.Clear();

            // Scheduling results ----------------
            dots_JobHandles.Add(RaycastCommand.ScheduleBatch(dots_RayCommands, dots_HitsResults, 1, default));


            // Update Hold Visible ------------------------------
            CullingDelayJob updateJob = new CullingDelayJob()
            {
                elapsed = dots_DelayElapsed,
                delta = Time.deltaTime
            };

            dots_JobHandles.Add(updateJob.Schedule());
        }


        private void DOTS_ScheduleTrasnparentRaycasting()
        {
            dots_JobHandlesTr.Clear();

            if (Instance.dots_TrHitsResults.IsCreated) Instance.dots_TrHitsResults.Dispose();
            Instance.dots_TrHitsResults = new NativeArray<RaycastHit>(dots_TransparentHitPoints.Length, Allocator.Persistent);

            if (Instance.dots_TransparentRayCommands.IsCreated) Instance.dots_TransparentRayCommands.Dispose();
            Instance.dots_TransparentRayCommands = new NativeArray<RaycastCommand>(dots_TransparentHitPoints.Length, Allocator.Persistent);

            // Scheduling Transparent Raycasting Jobs ------------------------------
            CreateTransparentRayCommandsJob trasnparentRayJobs = new CreateTransparentRayCommandsJob
            {
                position = MainCamera.transform.position,
                rayPoints = dots_TransparentHitPoints,

                maxDistance = MainCamera.farClipPlane,
                mask = ProgressiveCullingMask,
                rayCommands = dots_TransparentRayCommands
            };

            dots_JobHandlesTr.Add(trasnparentRayJobs.Schedule(dots_TransparentHitPoints.Length, 32, default));

            JobHandle.CompleteAll(dots_JobHandlesTr);
            dots_JobHandlesTr.Clear();

            dots_JobHandlesTr.Add(RaycastCommand.ScheduleBatch(dots_TransparentRayCommands, dots_TrHitsResults, 1, default));

        }




        #region Handles


        [BurstCompile]
        public struct CullingDelayJob : IJob
        {
            public NativeList<float> elapsed;
            [ReadOnly] public float delta;

            public void Execute()
            {
                for (int i = 0; i < elapsed.Length; i++) elapsed[i] += delta;
            }
        }


        [BurstCompile]
        public struct CreateRayCommandsJob : IJobParallelFor
        {
            [ReadOnly] public float3 position;
            [ReadOnly] public quaternion rotation;

            [ReadOnly] public int rayCastCycleOffset;
            [ReadOnly] public NativeArray<float3> rayDirs;

            [ReadOnly] public int mask;
            [ReadOnly] public float maxDistance;
            [WriteOnly] public NativeArray<RaycastCommand> rayCommands;

            public void Execute(int index)
            {
                float3 direction = math.mul(rotation, rayDirs[rayCastCycleOffset + index]);
                RaycastCommand command = new RaycastCommand(position, direction, maxDistance, mask);
                rayCommands[index] = command;
            }
        }

        [BurstCompile]
        public struct CreateTransparentRayCommandsJob : IJobParallelFor
        {
            [ReadOnly] public float3 position;
            [ReadOnly] public NativeList<float3> rayPoints;

            [ReadOnly] public int mask;
            [ReadOnly] public float maxDistance;
            [WriteOnly] public NativeArray<RaycastCommand> rayCommands;

            public void Execute(int index)
            {
                float3 direction = rayPoints[index] - position;
                RaycastCommand command = new RaycastCommand(rayPoints[index], math.normalize(direction), maxDistance - math.length(direction), mask);
                rayCommands[index] = command;
            }
        }


        [BurstCompile]
        public struct GetResultsJob : IJob
        {
            public NativeList<int> visible;
            [ReadOnly] public NativeList<int> hitted;
            [WriteOnly] public NativeList<float> elapsed;

            public void Execute()
            {
                for (int i = 0; i < hitted.Length; i++)
                {
                    int id = hitted[i];
                    int index = visible.IndexOf(id);
                    if (index < 0) { visible.Add(id); elapsed.Add(0); } else elapsed[index] = 0;
                }
            }
        }


        #endregion

    }
}


#endif