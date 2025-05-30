using Zenject;

public class TileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TileSpawner>().FromComponentInHierarchy().AsSingle();
    }
}
