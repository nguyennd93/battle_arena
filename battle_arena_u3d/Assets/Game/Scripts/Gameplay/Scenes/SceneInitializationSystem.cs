using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[BurstCompile]
public partial struct SceneInitializationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Game init
        if (SystemAPI.HasSingleton<SceneInitialization>())
        {
            ref SceneInitialization sceneInitializer = ref SystemAPI.GetSingletonRW<SceneInitialization>().ValueRW;

            // // Spawn player
            // Entity playerEntity = state.EntityManager.Instantiate(sceneInitializer.PlayerPrefabEntity);

            // // Spawn character at spawn point
            // Entity characterEntity = state.EntityManager.Instantiate(sceneInitializer.CharacterPrefabEntity);
            // LocalTransform spawnTransform = SystemAPI.GetComponent<LocalTransform>(sceneInitializer.CharacterSpawnPointEntity);
            // SystemAPI.SetComponent(characterEntity, LocalTransform.FromPositionRotation(spawnTransform.Position, spawnTransform.Rotation));

            // // Spawn camera
            // Entity cameraEntity = state.EntityManager.Instantiate(sceneInitializer.CameraPrefabEntity);
            // state.EntityManager.AddComponentData(cameraEntity, new GameCamera());

            state.EntityManager.DestroyEntity(SystemAPI.GetSingletonEntity<SceneInitialization>());
        }
    }
}