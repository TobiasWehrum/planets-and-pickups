using UnityEngine;
using Zenject;

namespace MiniPlanetDefense
{
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