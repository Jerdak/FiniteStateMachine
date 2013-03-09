using UnityEngine;
using System.Collections;

/// <summary>
/// State transition class - Encapsulates the state/command
/// pairing and creates a unique hash for that combination
/// </summary>
/// 
/// <notes>
/// This was adapted from a lovely FSM example StackOverflow:
/// http://stackoverflow.com/questions/5923767/simple-state-machine-example-in-c (Juliet's answer)
/// </notes>
class StateTransition
{
    public FiniteState CurrentState;
    public ITransitionCommand TransitionComponent;

    public StateTransition(FiniteState currentState, ITransitionCommand component)
    {
        CurrentState = currentState;
        TransitionComponent = component;
    }

    public override int GetHashCode()
    {
        return 17 + 31 * CurrentState.GetHashCode() + 31 * TransitionComponent.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        StateTransition other = obj as StateTransition;
        return 	other != null && 
				this.CurrentState == other.CurrentState && 
				this.TransitionComponent == other.TransitionComponent;
    }
}