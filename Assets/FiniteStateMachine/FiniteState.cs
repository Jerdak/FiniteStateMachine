using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Finite state class handles entrance and exit logic for states.
/// This particular state is limited to 1 action per entrance/exit.
/// </summary>
// [System.Serializable]
[Serializable]
public class FiniteState {
	public string Name;
	public int Hash;
	public bool HashGenerated = false;
	
	public delegate void StateEnterActionDelegate(GameObject go);
	public delegate void StateExitActionDelegate(GameObject go);
	
	public IStateAction EnterAction = null;
	public IStateAction ExitAction = null;
	
	public List<ITransitionCommand2> Transitions = new List<ITransitionCommand2>();

	public FiniteState(){
		GetHashCode();	//call once to trigger hash caching
	}
	public void ExitState(GameObject go){
		if(ExitAction != null)ExitAction.Act();
	}
	public void EnterState(GameObject go){
		if(EnterAction != null)EnterAction.Act();
	}
	public void EnableStateChanges(){
		if(EnterAction!=null)EnterAction.enabled = true;
		if(ExitAction!=null)ExitAction.enabled = true;
	}
	
	public void DisableStateChanges(){
		if(EnterAction!=null)EnterAction.enabled = false;
		if(ExitAction!=null)ExitAction.enabled = false;
	}
	public override int GetHashCode()
    {
		// Generate hash code on first call to GetHashCode()
		if(!HashGenerated){
			Hash = base.GetHashCode();
			HashGenerated = true;
		}
		return Hash;
    }
	public override bool Equals(object obj){
		FiniteState other = obj as FiniteState;
		return 	other != null && 
				this.GetHashCode() == other.GetHashCode();
	}
	/*public static bool operator==(FiniteState a, FiniteState b){
		return a.Equals(b);
	}
	public static bool operator!=(FiniteState a, FiniteState b){
		if (object.ReferenceEquals(null, a))
        	return object.ReferenceEquals(null, b);
		return !a.Equals(b);
	}*/
}
