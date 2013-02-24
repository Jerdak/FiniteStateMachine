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
    public readonly FiniteState CurrentState;
    public readonly ITransitionCommand Command;

    public StateTransition(FiniteState currentState, ITransitionCommand command)
    {
        CurrentState = currentState;
        Command = command;
    }

    public override int GetHashCode()
    {
        return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        StateTransition other = obj as StateTransition;
        return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
    }
}

//[System.Serializable]
class StateTransition2
{
    public FiniteState CurrentState;
    public ITransitionCommand2 TransitionComponent;

    public StateTransition2(FiniteState currentState, ITransitionCommand2 component)
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
        StateTransition2 other = obj as StateTransition2;
        return 	other != null && 
				this.CurrentState == other.CurrentState && 
				this.TransitionComponent == other.TransitionComponent;
    }
}