using Client;

using Leopotam.EcsLite;

public class EcsRunHandler
{
    public EcsWorld World;
    IEcsSystems _systems;

    public void Init()
    {
        World = new EcsWorld();
        _systems = new EcsSystems(World);
        _systems
            .Add(new InitPlayerSystem())
            .Add(new InitBusinessSystem())
            .Add(new InitInterfaceSystem())

            .Add(new RunIncomeDelaySystem())

            .Add(new RunResolveUpgradeSystem())
            .Add(new RunLevelUpSystem())

            .Add(new RunIncomeResolveSystem())

            .Add(new RunSaveSystem())

            .Add(new RunDelEventSystem<ResolveIncomeEvent>())
            .Add(new RunDelEventSystem<ResolveLevelUpEvent>())
            .Add(new RunDelEventSystem<ResolveUpgradeEvent>())

#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
            .Init();
    }

    public void Run()
    {
        // process systems here.
        _systems?.Run();
    }

    public void Dispose()
    {
        if (_systems != null)
        {
            SaveModule.SaveData();
            _systems.Destroy();
            _systems = null;
        }

        // cleanup custom worlds here.

        // cleanup default world.
        if (World != null)
        {
            World.Destroy();
            World = null;
        }
    }
}