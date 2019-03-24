using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    public class SceneObjectsMonoBehaviourInstaller : MonoInstaller
    {
        [SerializeField] MonoBehaviour[] monoBehaviours;
        
        public override void InstallBindings()
        {
            foreach (var monoBehaviour in monoBehaviours)
            {
                Container.Bind(monoBehaviour.GetType()).FromInstance(monoBehaviour).AsSingle();
            }
        }
    }
}