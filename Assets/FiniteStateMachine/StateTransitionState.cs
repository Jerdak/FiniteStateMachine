using System;
using System.Runtime.Serialization;

using UnityEngine;

[Serializable]
public class StateTransitionState  {
	public FiniteState StartState;
	public ITransitionCommand2 Transition;
	public FiniteState EndState;
}
