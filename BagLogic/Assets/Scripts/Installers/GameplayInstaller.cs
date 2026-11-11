using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private HudMessageHud _hudMessageHud;
    [SerializeField] private PlayerInteractInput _interactInput;

    public override void InstallBindings()
    {
        Container.Bind<Inventory>()
            .FromInstance(_playerInventory.Inventory)
            .AsSingle();

        Container.Bind<HudMessage>()
            .FromMethod(_ => _hudMessageHud.Message)
            .AsSingle();

        Container.Bind<PlayerInteractInput>()
            .FromInstance(_interactInput)
            .AsSingle();

        Container.Bind<PlayerLootPicker>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InventoryScreen>().FromComponentInHierarchy().AsSingle();
    }
}