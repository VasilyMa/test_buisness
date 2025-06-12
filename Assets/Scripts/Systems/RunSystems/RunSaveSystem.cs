using Leopotam.EcsLite;


namespace Client 
{
    sealed class RunSaveSystem : IEcsInitSystem, IEcsRunSystem 
    {
        EcsWorld world;
        EcsFilter filter;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            filter = world.Filter<ResolveSaveEvent>().End();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                SaveModule.SaveData();

                world.DelEntity(entity);
            }
        }
    }
}