using UnityEngine;
using UnityEngine.AI;
using GitAmendStateMachine; 

public class AIAgent : MonoBehaviour
{
    // private AIStateMachine stateMachine;
    private StateMachine stateMachine; 
    public AIStateID initialState;
    public NavMeshAgent navMeshAgent;
    public AIAgentConfig config;
    public AISensor sensor; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<AISensor>();
        // Kiwi system
        /*
        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIRoamState());
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIFindPickupState());
        stateMachine.ChangeState(initialState);
        */

        // State Machine
        stateMachine = new StateMachine();

        // Declare states
        var roamState = new GitRoamState(this);
        var collectPickupState = new GitCollectPickupState(this);

        // Define transitions

        At(roamState, collectPickupState, new FuncPredicate(() => sensor.Objects.Count > 0));
        At(collectPickupState, roamState, new FuncPredicate(() => sensor.Objects.Count == 0));

        // Set initial state
        stateMachine.SetState(roamState); 
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Collectible"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
        }
    }
}
