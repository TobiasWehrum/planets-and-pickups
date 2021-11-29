using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using MiniPlanetDefense;
using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{

    
    public GameObject physicsHelperPrefab;
    public GameObject soundManagerPrefab;
    public GameObject playerPrefab;
    public GameObject uiPrefab;
    public GameObject poolPrefab;
    public GameObject gameAreaPrefab;



    // Start is called before the first frame update
    public override void InstallBindings()
    {
        Debug.Log("Starting Installer");
        
        // Create one definitive instance of "Constants" and re-use that for every class that asks for it

        // Instantiate GO from prefab and grab definitive instance of "<ScriptName>"
        // and re-use for every class that asks for it 
        Container.Bind<PhysicsHelper>().FromComponentInNewPrefab(physicsHelperPrefab).AsSingle().NonLazy();
        Container.Bind<SoundManager>().FromComponentInNewPrefab(soundManagerPrefab).AsSingle().NonLazy();
        Container.Bind<IngameUI>().FromComponentInNewPrefab(uiPrefab).AsSingle().NonLazy();
        Container.Bind<Player>().FromComponentInNewPrefab(playerPrefab).AsSingle().NonLazy();
        Container.Bind<Pool>().FromComponentInNewPrefab(poolPrefab).AsSingle().NonLazy();
        Container.Bind<GameArea>().FromComponentInNewPrefab(gameAreaPrefab).AsSingle().NonLazy();
    }

}
