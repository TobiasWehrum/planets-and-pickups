using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A Zenject installer that binds all <see cref="MonoBehaviour"/>s found in its children.
    /// </summary>
    public class AutomaticChildMonoBehaviourInstaller : MonoInstaller
    {
        [SerializeField] bool printDebugInformation;
        
        public override void InstallBindings()
        {
            foreach (var monoBehaviour in GetComponentsInChildren<MonoBehaviour>())
            {
                // Only bind children
                if (monoBehaviour.gameObject == gameObject)
                    continue;

                if (printDebugInformation)
                {
                    Debug.Log("[AutomaticChildMonoBehaviourInstaller] Installing: " + monoBehaviour.GetType());
                }

                Container.Bind(monoBehaviour.GetType()).FromInstance(monoBehaviour).AsSingle();
            }
        }
    }
}