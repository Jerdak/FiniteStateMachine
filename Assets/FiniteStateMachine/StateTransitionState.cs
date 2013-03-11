using System;
using System.Runtime.Serialization;

using UnityEngine;

[Serializable]
public class StateTransitionState  {
	public FiniteState StartState = null;
	public ITransitionCommand Transition = null;
	public FiniteState EndState = null;
	
	/// <summary>
	/// Return true iff StartState, EndState, and Transition are all non-null
	/// </summary>
	public bool Valid(){
		return (StartState && EndState && Transition != null);
	}
}
