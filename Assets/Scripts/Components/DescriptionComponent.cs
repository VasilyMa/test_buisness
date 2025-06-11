using Leopotam.EcsLite;

namespace Client 
{
    [System.Serializable]
    public struct DescriptionComponent : IComponent
    {
        public string Name;
        public string Description;

        public void Init(EcsWorld world, int entity)
        {
            ref var descriptionComp = ref world.GetPool<DescriptionComponent>().Add(entity);
            descriptionComp.Description = Description;
            descriptionComp.Name    = Name;
        }
    }
}