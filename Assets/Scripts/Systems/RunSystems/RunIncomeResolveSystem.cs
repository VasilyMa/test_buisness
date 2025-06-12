using Leopotam.EcsLite;

namespace Client 
{
    sealed class RunIncomeResolveSystem : IEcsInitSystem, IEcsRunSystem
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<IncomeComponent> incomePool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<IncomeComponent>().Inc<ResolveIncomeEvent>().End();
            incomePool = world.GetPool<IncomeComponent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                var state = State.Instance;

                ref var incomeComp = ref incomePool.Get(entity);    

                int value = incomeComp.Value;

                if (state.TryGetEntity("player", out int playerEntity))
                {
                    ref var playerComp = ref world.GetPool<PlayerComponent>().Get(playerEntity);
                    playerComp.AddValue(value);

                    SaveModule.CurrentData.Value = playerComp.Value;

                    world.GetPool<ResolveSaveEvent>().Add(world.NewEntity());
                }
            }
        }
    }
}