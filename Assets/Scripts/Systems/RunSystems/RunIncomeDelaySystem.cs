using Leopotam.EcsLite;

using UnityEngine;

namespace Client 
{
    sealed class RunIncomeDelaySystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<IncomeDelayComponent> incomeDelayPool;
        EcsPool<ResolveIncomeEvent> resolveDelayPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<IncomeDelayComponent>().End();
            incomeDelayPool = world.GetPool<IncomeDelayComponent>();
            resolveDelayPool = world.GetPool<ResolveIncomeEvent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                ref var delayComp = ref incomeDelayPool.Get(entity);

                delayComp.RemainningTime -= Time.deltaTime;

                if (delayComp.RemainningTime <= 0)
                {
                    delayComp.RemainningTime = delayComp.DelayTime;
                    resolveDelayPool.Add(entity);
                }

                float value = 1 - (delayComp.RemainningTime / delayComp.DelayTime);

                ObserverEntity.ProgressChange(entity, value);
            }
        }
    }
}