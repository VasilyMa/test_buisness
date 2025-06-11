using Leopotam.EcsLite;

namespace Client 
{
    sealed class InitPlayerSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems) 
        {
            var state = State.Instance;
            var world = state.EcsRunHandler.World;

            var playerEntity = world.NewEntity();

            if (state.TryAddEntity("player", playerEntity))
            {
                ref var playerComp = ref world.GetPool<PlayerComponent>().Add(playerEntity);
                playerComp.Value = 0;
            }
        }
    }
}