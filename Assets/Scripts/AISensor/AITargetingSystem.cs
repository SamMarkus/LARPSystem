using UnityEngine;

[ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    public float memorySpan = 3.0f;
    public int bufferSize = 10;
    public float distanceWeight = 1.0f;
    public float angleWeight = 1.0f;
    public float ageWeight = 1.0f;

    public bool HasTarget
    {
        get
        {
            return bestMemory != null;
        }
    }
    public GameObject Target
    {
        get
        {
            return bestMemory.gameObject;
        }
    }
    public Vector3 TargetPosition
    {
        get
        {
            return bestMemory.gameObject.transform.position;
        }
    }
    public bool TargetInSight
    {
        get
        {
            return bestMemory.Age < 0.5f; // seconds
        }
    }
    public float TargetDistance
    {
        get
        {
            return bestMemory.distance;
        }
    }

    AISensoryMemory memory = new AISensoryMemory(0, "");
    AISensor sensor;
    AIMemory bestMemory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensor = GetComponent<AISensor>();
        memory = new AISensoryMemory(bufferSize, sensor.layers.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);

        EvaluateScores();
    }

    void EvaluateScores()
    {
        bestMemory = null;

        foreach (var memory in memory.memories)
        {
            memory.score = CalculateScore(memory); 
            if (bestMemory == null ||
                memory.score > bestMemory.score)
            {
                bestMemory = memory; 
            }
        }
    }

    // Helper function that places a value in a 0 to 1 range.
    float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }

    float CalculateScore(AIMemory memory)
    {
        float distanceScore = Normalize(memory.distance, sensor.distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, sensor.angle) * angleWeight;
        float ageScore = Normalize(memory.Age, memorySpan) * ageWeight; 
        return distanceScore + angleScore + ageWeight;
    }

    // OnDrawGizmos is called when gizmos are displayed through edit view
    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (var memory in memory.memories)
        {
            maxScore = Mathf.Max(maxScore, memory.score);
        }

        foreach(var memory in memory.memories)
        {
            Color color = Color.red;
            if (memory == bestMemory)
            {
                color = Color.yellow;
            }
            color.a = memory.score / maxScore; 
            Gizmos.color = color;
            Gizmos.DrawSphere(memory.position, 3f);
        }
    }
}
