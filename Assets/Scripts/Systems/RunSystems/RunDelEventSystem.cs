using Leopotam.EcsLite;

namespace Client 
{
    sealed class RunDelEventSystem<T> : IEcsInitSystem, IEcsRunSystem where T : struct
    {
        EcsWorld world;
        EcsFilter filter;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld(); 
            filter = world.Filter<T>().End();
        }

        public void Run (IEcsSystems systems) 
        {
            foreach (var entity in filter)
            {
                world.GetPool<T>().Del(entity);
            }
        }
    }
}