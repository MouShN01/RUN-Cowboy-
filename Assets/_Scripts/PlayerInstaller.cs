using _Scripts;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerController player;
    public override void InstallBindings()
    {
        PlayerController player = Container.InstantiatePrefabForComponent<PlayerController>(this.player);
        Container
            .Bind<PlayerController>()
            .FromInstance(player)
            .AsSingle().NonLazy();
        Container
            .Bind<PlayerStats>()
            .FromComponentInHierarchy()
            .AsSingle().NonLazy();
    }
}
