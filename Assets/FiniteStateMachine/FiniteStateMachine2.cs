using UnityEngine;
using System.Collections.Generic;

public class FiniteStateMachine2 : MonoBehaviour {
	public FiniteState CurrentState = null;
	public FiniteState StartState = null;

	/// <summary>
	/// Per state transition list, used to limit FSM's Update() to only the current states possible transitions
	/// </summary>
	Dictionary<StateTransition2,FiniteState> StateTransitions = new Dictionary<StateTransition2, FiniteState>();
	
	[SerializeField]
	public List<StateTransitionState> StateCache = new List<StateTransitionState>();
	
	/// <summary>
	/// List of transitions for the current state only
	/// </summary>
	List<ITransitionCommand2> ActiveTransitionList = null;

	
	void Awake() {
		//Unity serialization doesn't work w/ Generic.Dictionary, rebuild from state list
		foreach(StateTransitionState sts in StateCache){
			StateTransition2 st = new StateTransition2(sts.StartState,sts.Transition);
			StateTransitions.Add(st,sts.EndState);
			Debug.Log("Rebuilding transition from " + sts.StartState.Name + " to " + sts.EndState.Name);
			Debug.Log("  - Rebuild Hash: " + st.GetHashCode());
			Debug.Log("  - Hash start state: " + sts.StartState.GetHashCode());
			Debug.Log("  - Hash transition: " + sts.Transition.GetHashCode());
			Debug.Log("  - Hash End State : " + sts.StartState.GetHashCode());
		}	
		StateTransition2 st1 = new StateTransition2(StateCache[0].StartState,StateCache[0].Transition);
		StateTransition2 st2 = new StateTransition2(StartState,StateCache[0].Transition);
		Debug.Log("Comparison Hash.  " + st1.GetHashCode() + "==" + st2.GetHashCode());
		Debug.Log("Comparison Equl.  " + (st1 == st2).ToString());
	}
	
	/// <summary>
	/// Add a new transition from 1 state to another.
	/// </summary>
	public ITransitionCommand2 AddTransitionType(FiniteState fromState, System.Type transition_type, FiniteState toState){
		// Components can only be instantiated by a game object, which is why AddTransitionType only
		// takes a transition_type as an argument and not a fully instantiated component.
		ITransitionCommand2 component = gameObject.AddComponent(transition_type) as ITransitionCommand2;
		component.enabled = false;
		
		StateTransition2 st = new StateTransition2(fromState,component);
		Debug.Log("Adding new transition type " + component.GetType() + " from state " + fromState.Name + " to state " + toState.Name);
		Debug.Log("  - Hash: " + st.GetHashCode());	
		// make sure to clear out the ununused component
		if(StateTransitions.ContainsKey(st)){
			Destroy(component);
			Debug.Log("fromState " + fromState.Name + " already contains a transition type " + transition_type.ToString());
			return null;
		}
		StateTransitions.Add(st,toState);
		
		//add transition to the from state (states store outgoing states only)
		fromState.Transitions.Add(component);
		StateCache.Add(new StateTransitionState{StartState = fromState, Transition = component, EndState = toState});
		Debug.Log("Added transition type");
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
		Debug.Log("Trying to transition from state: " + CurrentState.Name);
		Debug.Log("  - Hash Full: " + st.GetHashCode());
		Debug.Log("  - Hash CurrentState: " + CurrentState.GetHashCode());
		Debug.Log("  - Hash transition: " + transition.GetHashCode());
		
		StateTransition2 st1 = new StateTransition2(StateCache[0].StartState,StateCache[0].Transition);
		StateTransition2 st2 = new StateTransition2(CurrentState,StateCache[0].Transition);
		Debug.Log("Comparison2 Hash.  " + st1.GetHashCode() + "==" + st2.GetHashCode());
		Debug.Log("Comparison2 Equl.  " + (st1 == st2).ToString());
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
		ActiveTransitionList = CurrentState.Transitions;//StateTransitionMap[CurrentState];
		EnableActiveComponents();
	}

	
	void Start(){
		ChangeState(StartState);
	}
	void Update(){

	}
}
