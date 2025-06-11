using Leopotam.EcsLite;

using UnityEngine;

namespace Client 
{
    sealed class RunResolveUpgradeSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<ResolveUpgradeEvent> upgradePool;
        EcsPool<IncomeComponent> incomePool;

        public void Init(IEcsSystems systems)
        {
            var state = State.Instance;
            world = state.EcsRunHandler.World;
            filter = world.Filter<ResolveUpgradeEvent>().Inc<IncomeComponent>().End();
            upgradePool = world.GetPool<ResolveUpgradeEvent>();
            incomePool = world.GetPool<IncomeComponent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                ref var incomeComp = ref incomePool.Get(entity);
                ref var upgradeComp = ref upgradePool.Get(entity);

                incomeComp.AddModifier(upgradeComp.Value);

                ObserverEntity.BusinessChange(entity);
            }
        }
    }
}