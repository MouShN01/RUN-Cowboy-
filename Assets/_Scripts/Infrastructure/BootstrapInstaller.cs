using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
   [SerializeField] private PickSoundManager soundManager;
   [SerializeField] private AdManager adManager;
   public override void InstallBindings()
   {
      PickSoundManager pickSoundManager = Container.InstantiatePrefabForComponent<PickSoundManager>(this.soundManager);
      GameObject.DontDestroyOnLoad(pickSoundManager.gameObject); // ← ВАЖНО!
      Container.Bind<PickSoundManager>().FromInstance(pickSoundManager).AsSingle().NonLazy();

      AdManager adManagerInstance = Container.InstantiatePrefabForComponent<AdManager>(this.adManager);
      GameObject.DontDestroyOnLoad(adManagerInstance.gameObject); // ← ТОЖЕ!
      Container.Bind<AdManager>().FromInstance(adManagerInstance).AsSingle().NonLazy();
   }
}
