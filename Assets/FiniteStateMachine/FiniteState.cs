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
public class FiniteState : ScriptableObject {
	public string StateName = "";
	public Rect WindowRect = new Rect(0,0,0,0);
	
	public delegate void StateEnterActionDelegate(GameObject go);
	public delegate void StateExitActionDelegate(GameObject go);
	
	public IStateAction EnterAction = null;
	public IStateAction ExitAction = null;

	public FiniteState(){}
	
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
