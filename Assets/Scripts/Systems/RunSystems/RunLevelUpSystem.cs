using Leopotam.EcsLite;

namespace Client 
{
    sealed class RunLevelUpSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<IncomeComponent> incomePool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<ResolveLevelUpEvent>().Inc<IncomeComponent>().End();
            incomePool = world.GetPool<IncomeComponent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                ref var incomeComp = ref incomePool.Get(entity);
                incomeComp.LevelUp();

                ObserverEntity.BusinessChange(entity);
            }
        }
    }
}