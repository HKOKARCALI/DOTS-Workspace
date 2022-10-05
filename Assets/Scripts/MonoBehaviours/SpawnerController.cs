using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] GameObject personPrefab;
    [SerializeField] int gridSize;
    [SerializeField] int spread;
    [SerializeField] Vector2 speedRange = new Vector2(2,10);
    [SerializeField] Vector2 lifetimeRange = new Vector2(10,60);
    private BlobAssetStore blobAsset;

    private void Start(){
        
        blobAsset = new BlobAssetStore();       //Allocate the memory
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAsset);        //Which memory this will use
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);     //Converting a "GameObject" to "Entity"
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for(int x = 0; x < gridSize; x++){

            for(int z = 0; z < gridSize; z++){

                var instance = entityManager.Instantiate(entity);      //Instantiating not GameObjects but Entities

                float3 position = new float3(x * spread, 0f, z * spread);
                entityManager.SetComponentData(instance, new Translation{Value = position});
                entityManager.SetComponentData(instance, new Destination{Value = position});

                float lifetime = UnityEngine.Random.Range(lifetimeRange.x,lifetimeRange.y);
                entityManager.SetComponentData(instance, new Lifetime{Value = lifetime});

                float speed = UnityEngine.Random.Range(speedRange.x,speedRange.y);
                entityManager.SetComponentData(instance, new MovementSpeed{Value = speed});

            }

            

        }

        

    }

    private void OnDestroy(){
        
        blobAsset.Dispose();                    //Dispose the memory

    }
}
