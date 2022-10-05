using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]      //It has to be in "Initialization" group so first the number gets generated then used.
public class RandomSystem : SystemBase
{
    public NativeArray<Unity.Mathematics.Random> RandomArray{get; private set;}

    protected override void OnCreate()          //Called once like "Start"
    {
        var randomArray = new Unity.Mathematics.Random[JobsUtility.MaxJobThreadCount];  //Inside Array: How much thread does the computer have
        var seed = new System.Random();

        for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++)
        {
            
            randomArray[i] = new Unity.Mathematics.Random((uint)seed.Next());

        }

        RandomArray = new NativeArray<Unity.Mathematics.Random>(randomArray,Allocator.Persistent);  //Allocator.Persistent: Keep the memory until we dispose it.
    }

    protected override void OnDestroy()
    {
        
        RandomArray.Dispose();  //Dispose the memory

    }
    protected override void OnUpdate()
    {

    }
}
