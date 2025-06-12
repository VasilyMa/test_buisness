
using Client;

using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [SerializeField] Text titleValue;
    private BusinessLayout layout;


    public MainPanel Init()
    {
        layout = GetComponentInChildren<BusinessLayout>().Init();

        if (State.Instance.TryGetEntity("player", out int playerEntity))
        {
            var world = State.Instance.EcsRunHandler.World;

            ref var playerComp = ref world.GetPool<PlayerComponent>().Get(playerEntity);

            titleValue.text = $"Баланс: ${playerComp.Value}";
        }
        ObserverEntity.OnValueChange += onValueChange;

        return this;
    }

    void onValueChange(int value)
    {
        titleValue.text = $"Баланс: ${value}";
    }
}
