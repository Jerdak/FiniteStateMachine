using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Finite state class handles entrance and exit logic for states.
/// This particular state is limited to 1 action per entrance/exit.
/// </summary>
// [System.Serializable]
public class FiniteState {
	public string Name;
	
	public delegate void StateEnterActionDelegate(GameObject go);
	public delegate void StateExitActionDelegate(GameObject go);
	
	public IStateAction EnterAction = null;
	public IStateAction ExitAction = null;
	
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
}
