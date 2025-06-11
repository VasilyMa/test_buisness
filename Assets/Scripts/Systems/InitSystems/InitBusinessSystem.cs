using Leopotam.EcsLite;

namespace Client 
{
    sealed class InitBusinessSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems)
        {
            var state = State.Instance;
            var world = state.EcsRunHandler.World;

            var businessList = BusinessConfig.Instance.BusinessListData;

            foreach (var business in businessList)
            {
                var entity = world.NewEntity();

                business.InitEntity(world, entity);
            }
        }
    }
}