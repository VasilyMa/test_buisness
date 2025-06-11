using Leopotam.EcsLite;

using UnityEngine;

namespace Client {
    sealed class InitInterfaceSystem : IEcsInitSystem 
    {
        public void Init (IEcsSystems systems) 
        {
            var canvas = GameObject.FindObjectOfType<MainCanvas>(true);

            if (canvas != null)
            {
                canvas.Init();
            }
        }
    }
}