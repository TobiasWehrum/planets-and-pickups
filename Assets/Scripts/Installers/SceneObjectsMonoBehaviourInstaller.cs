using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A Zenject installer that binds one or more <see cref="MonoBehaviour"/>s already existing in the scene.
    /// </summary>
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