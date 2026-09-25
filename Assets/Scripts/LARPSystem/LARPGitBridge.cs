using LARPSystem;
using GitAmendStateMachine;
using UnityEngine;
using System; 

public class LARPTransition : ITransition
{
    public IState Option1 { get; }
    public IState Option2 { get; }
    public IState To { get; }

    public IPredicate Condition { get; }

    public LARPTransition (IState option1, IState option2, IPredicate condition)
    {
        Option1 = option1;
        Option2 = option2;
        Condition = condition;
    }
}
