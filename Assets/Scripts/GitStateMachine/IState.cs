using UnityEngine;

namespace GitAmendStateMachine
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }

    public abstract class BaseState : IState
    {
        protected AIAgent agent; 

        protected BaseState(AIAgent agent) { this.agent = agent; }

        public virtual void FixedUpdate() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update() { }
    }
}
