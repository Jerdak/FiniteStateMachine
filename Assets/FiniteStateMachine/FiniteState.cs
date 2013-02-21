using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Finite state class handles entrance and exit logic for states.
/// This particular state is limited to 1 action per entrance/exit.
/// </summary>
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
	/// <summary>
	/// Example state action, change renderer material to red.
	/// </summary>
	public static void TestAction_ChangeColorRed(GameObject go){
		go.renderer.material.color = Color.red;
	}

	/// <summary>
	/// Example state action, change renderer material to green.
	/// </summary>
	public static void TestAction_ChangeColorGreen(GameObject go){
		go.renderer.material.color = Color.green;
	}
		/// <summary>
	/// Example state action, change target renderer material to blue.
	/// </summary>
	public static void TestAction_ChangeColorBlue(GameObject go){
		go.renderer.material.color = Color.blue;
	}
	/// <summary>
	/// Example state action, change target renderer material to yellow.
	/// </summary>
	public static void TestAction_ChangeColorYellow(GameObject go){
		go.renderer.material.color = Color.yellow;
	}
	/// <summary>
	/// Example state action, change target renderer material to green.
	/// </summary>
	public static void TestAction_ChangeColorMagenta(GameObject go){
		go.renderer.material.color =  Color.magenta;
	}
	
	/// <summary>
	/// Example state action, change target renderer material to green.
	/// </summary>
	public static void TestAction_ChangeColorTargetGreen(GameObject go){
		PressurePlate pp = go.GetComponent<PressurePlate>();
		pp.Target.renderer.material.color = Color.green;
	}
	/// <summary>
	/// Example state action, change target renderer material to green.
	/// </summary>
	public static void TestAction_ChangeColorTargetRed(GameObject go){
		PressurePlate pp = go.GetComponent<PressurePlate>();
		pp.Target.renderer.material.color = Color.red;
	}

}
