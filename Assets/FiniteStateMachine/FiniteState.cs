using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Finite state class handles entrance and exit logic for states.
/// This particular state is limited to 1 action per entrance/exit.
/// </summary>
[Serializable]
public class FiniteState {
	public bool Start = false;
	public string StateName = "";
	public Rect WindowRect = new Rect(0,0,0,0);
	public int UniqueID = -1;
	
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
	
	public override int GetHashCode(){
		int hash = 17;
	    hash = hash * 31 + UniqueID;
		return hash;
	}
	
 	public override bool Equals(object obj)
    {
		if (object.ReferenceEquals(obj, null))return false;
		
        FiniteState item = obj as FiniteState;
        return item.UniqueID == this.UniqueID;
    }
	
	public static implicit operator bool(FiniteState a) 
	{
		if (object.ReferenceEquals(a, null)) return false;
		return (a.UniqueID==-1)?false:true;
	}
	
	public static bool operator ==(FiniteState a, FiniteState b)
	{
	    if (object.ReferenceEquals(a, null))
	    {
	         return object.ReferenceEquals(b, null);
	    }
	
	    return a.Equals(b);
	}
	public static bool operator !=(FiniteState a, FiniteState b)
	{
		if (object.ReferenceEquals(a, null))
	    {
	         return object.ReferenceEquals(b, null);
	    }
		return !a.Equals(b);
	}
}
