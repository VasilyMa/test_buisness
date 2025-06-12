using Leopotam.EcsLite;

namespace Client 
{
    sealed class RunLevelUpSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;
        EcsPool<BusinessComponent> busineesPool;
        EcsPool<IncomeComponent> incomePool;
        EcsPool<CostComponent> costPool;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<ResolveLevelUpEvent>().Inc<IncomeComponent>().Inc<CostComponent>().End();
            incomePool = world.GetPool<IncomeComponent>();
            costPool = world.GetPool<CostComponent>();
            busineesPool = world.GetPool<BusinessComponent>();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                ref var incomeComp = ref incomePool.Get(entity);
                incomeComp.LevelUp();

                ref var costComp = ref costPool.Get(entity);
                costComp.Cost = costComp.CostBase * (1 + incomeComp.Level);

                ref var businessComp = ref busineesPool.Get(entity);

                string key = businessComp.KEY_ID;

                var businessData = SaveModule.CurrentData.BusinessList.Find(x => x.KEY_ID == key);

                if (businessData != null)
                {
                    businessData.Level = incomeComp.Level;
                }
                else
                {
                    SaveModule.CurrentData.BusinessList.Add(new BusinessData(key, incomeComp.Level));
                }

                ObserverEntity.BusinessChange(entity);
            }
        }
    }
}