using System.Collections.Generic;

using Client;

using UnityEngine;

public class BusinessLayout : MonoBehaviour
{
    [SerializeField] Transform parent;
    private List<BusinessSlotView> slots = new List<BusinessSlotView>();

    public BusinessLayout Init()
    {
        var interfaceConfig = InterfaceConfig.Instance; 

        var world = State.Instance.EcsRunHandler.World;

        var filter = world.Filter<BusinessComponent>().End();

        foreach (var entity in filter)
        {
            var slot = GameObject.Instantiate(interfaceConfig.BusinessSlotPref, parent);

            slot.Init(entity);
            
            slots.Add(slot);
        }

        return this;
    }
}
