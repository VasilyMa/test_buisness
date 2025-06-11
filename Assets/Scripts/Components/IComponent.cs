using Leopotam.EcsLite;

public interface IComponent
{
    void Init(EcsWorld world, int entity);
}
