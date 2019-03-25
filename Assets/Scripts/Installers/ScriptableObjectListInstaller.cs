using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
    /// <summary>
    /// A Zenject installer that binds one or more <see cref="ScriptableObject"/>s.
    /// </summary>
    public class ScriptableObjectListInstaller : MonoInstaller
    {
        [SerializeField] ScriptableObject[] scriptableObjects;
        
        public override void InstallBindings()
        {
            foreach (var scriptableObject in scriptableObjects)
            {
                Container.Bind(scriptableObject.GetType()).FromScriptableObject(scriptableObject).AsSingle();
            }
        }
    }
}