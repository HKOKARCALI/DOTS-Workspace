using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class NewDestinationSystem : SystemBase
{
    private RandomSystem randomSystem;

    protected override void OnCreate()
    {
        
        randomSystem = World.GetExistingSystem<RandomSystem>();         //Think of this as "GetComponent" but for Systems

    }
    protected override void OnUpdate()
    {
        var randomArray = randomSystem.RandomArray;

        Entities.WithNativeDisableParallelForRestriction(randomArray).ForEach((int nativeThreadIndex, ref Destination destination,in Translation translation) => {     //WithNativeDisableParallelForRestriction: Removing restriction systems(It is dangerous), nativeThreadIndex: Which thread we are working on?
            
            float distance = math.abs(math.length(destination.Value - translation.Value));

            if(distance < 0.1f){

                var random = randomArray[nativeThreadIndex];

                destination.Value.x = random.NextFloat(0,500);
                destination.Value.z = random.NextFloat(0,500);

                randomArray[nativeThreadIndex] = random;        //Modified values will be saved back into the same array

            }

        }).ScheduleParallel();
    }
}
