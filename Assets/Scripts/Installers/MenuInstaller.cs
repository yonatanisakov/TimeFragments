using UnityEngine;
using Zenject;

/// <summary>
/// Menu Installer - Sets up dependency injection for the main menu scene
/// This tells Zenject how to create and connect our menu components
/// </summary>
public class MenuInstaller : MonoInstaller
{
    [Header("Menu UI Reference")]
    [SerializeField] private MainMenuUI _mainMenuUI;

    public override void InstallBindings()
    {
        // Bind the MainMenuUI interface to the actual MainMenuUI component
        Container.Bind<IMainMenuUI>().FromInstance(_mainMenuUI).AsSingle();
        Container.Bind<LevelSelectionPanel>().FromComponentInHierarchy().AsSingle();

        Container.Bind<IProgressionService>().To<ProgressionService>().AsSingle();
        Container.Bind<ISceneService>().To<SceneService>().AsSingle();

    }
}