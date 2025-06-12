using Leopotam.EcsLite;

using UnityEngine;

namespace Client 
{
    sealed class RunIncomeDelaySystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<IncomeDelayComponent> incomeDelayPool;
        EcsPool<IncomeComponent> incomePool;
        EcsPool<ResolveIncomeEvent> resolveDelayPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<IncomeDelayComponent>().Inc<IncomeComponent>().End();
            incomeDelayPool = world.GetPool<IncomeDelayComponent>();
            incomePool = world.GetPool<IncomeComponent>();
            resolveDelayPool = world.GetPool<ResolveIncomeEvent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                ref var incomeComp = ref incomePool.Get(entity);

                if (incomeComp.Level == 0) continue;

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