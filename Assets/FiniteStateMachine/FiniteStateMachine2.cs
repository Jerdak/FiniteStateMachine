using UnityEngine;
using System.Collections.Generic;

public class FiniteStateMachine2 : MonoBehaviour {
	FiniteState CurrentState = null;

	/// <summary>
	/// Complete state-transition map.  Key'd to (state,transition) pairs.
	/// </summary>
	Dictionary<FiniteState,List<ITransitionCommand2>> StateTransitionMap = new Dictionary<FiniteState, List<ITransitionCommand2>>();
	
	/// <summary>
	/// Per state transition list, used to limit FSM's Update() to only the current states possible transitions
	/// </summary>
	Dictionary<StateTransition2,FiniteState> StateTransitions = new Dictionary<StateTransition2, FiniteState>();
	
	/// <summary>
	/// List of transitions for the current state only
	/// </summary>
	List<ITransitionCommand2> ActiveTransitionList = null;
	
	/// <summary>
	/// Add a new transition from 1 state to another.
	/// </summary>
	public ITransitionCommand2 AddTransitionType(FiniteState fromState, System.Type transition_type, FiniteState toState){
		// Components can only be instantiated by a game object, which is why AddTransitionType only
		// takes a transition_type as an argument and not a fully instantiated component.
		ITransitionCommand2 component = gameObject.AddComponent(transition_type) as ITransitionCommand2;
		component.enabled = false;
		
		StateTransition2 st = new StateTransition2(fromState,component);
		
		// make sure to clear out the ununused component
		if(StateTransitions.ContainsKey(st)){
			Destroy(component);
			Debug.Log("fromState " + fromState.Name + " already contains a transition type " + transition_type.ToString());
			return null;
		}
		StateTransitions.Add(st,toState);
		if(!StateTransitionMap.ContainsKey(fromState)){
			StateTransitionMap[fromState] = new List<ITransitionCommand2>();
		}
		StateTransitionMap[fromState].Add(component);
		return component;
	}
	
	/// <summary>
	/// Quick test of component instantiation via System.Type.
	/// </summary>
	void DebugTypeTest(){
		System.Type type = typeof(OnTimer2);
		OnTimer2 t1 = gameObject.AddComponent(type) as OnTimer2;
		t1.Description = "t1";
		OnTimer2 t2 = gameObject.AddComponent(type) as OnTimer2;
		t2.Description = "t2";
		Debug.Log("Instantiated time components");
		ITransitionCommand2 m = gameObject.GetComponents(type)[0] as ITransitionCommand2;
		m.enabled = false;
	}

	
	/// <summary>
	/// Transition to the next state (assumes 'transition' is valid for the current state)
	/// </summary>
	public void Transition(ITransitionCommand2 transition){
		StateTransition2 st = new StateTransition2(CurrentState,transition);
		ChangeState(StateTransitions[st]);
	}
	
	/// <summary>
	/// Disables currently active transition components.
	/// </summary>
	public void DisableActiveComponents(){
		if(ActiveTransitionList==null)return;
		foreach(ITransitionCommand2 m in ActiveTransitionList){
			//Debug.Log("Disabling component " + m.Name);
			m.enabled = false;
			m.Enabled = "false";
		}
	}
	
	/// <summary>
	/// Enables currently active transition components.
	/// </summary>
	void EnableActiveComponents(){
		if(ActiveTransitionList==null)return;
		foreach(ITransitionCommand2 m in ActiveTransitionList){
			//Debug.Log("Enabling component " + m.Name);
			m.ResetTransition();
			m.enabled = true;
			m.Enabled = "true";
		}
	}
	
	/// <summary>
	/// Change to the next state and update the currently active transitions.
	/// </summary>
	public void ChangeState(FiniteState nextState){
		if(nextState == null) {
			Debug.Log("No next state, was passed null");
			return;
		}
		

		// disable any active transition components
		DisableActiveComponents();
		
		// apply transition logic from current state
		if(CurrentState != null){
			CurrentState.ExitState(gameObject);
			CurrentState.DisableStateChanges();
		}
		CurrentState = nextState;
		CurrentState.EnableStateChanges();
		
		// apply transition to new state
		CurrentState.EnterState(gameObject);

		// Change the transition component list to the active states transitions and enable them
		ActiveTransitionList = StateTransitionMap[CurrentState];
		EnableActiveComponents();
	}
		
	void Start(){
	
	}
	void Update(){

	}
}
