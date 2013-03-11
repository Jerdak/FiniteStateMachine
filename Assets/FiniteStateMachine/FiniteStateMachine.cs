using UnityEngine;
using System.Collections.Generic;

public class FiniteStateMachine : MonoBehaviour {
	public FiniteState CurrentState = null;
	public FiniteState StartState = null;
	
	[SerializeField]
	static int nameSalt = 0;
	
	/// <summary>
	/// StartState/Transition pair lookup
	/// </summary>
	Dictionary<StateTransition,FiniteState> StateTransitions = new Dictionary<StateTransition, FiniteState>();
	
	/// <summary>
	/// State transition map, all transitions out of a given FiniteState.
	/// </summary>
	Dictionary<FiniteState, List<ITransitionCommand>> TransitionMap = new Dictionary<FiniteState, List<ITransitionCommand>>();
	
	/// <summary>
	/// Complete state/transition/state list for rebuilding state/transition dictionaries on deserialization
	/// </summary>
	[SerializeField]
	public List<StateTransitionState> StateCache = new List<StateTransitionState>();
	
	/// <summary>
	/// List of transitions for the current state only
	/// </summary>
	List<ITransitionCommand> ActiveTransitionList = null;
	
	/// <summary>
	/// Return full finite state machine graph. 
	/// </summary>
	/// <param name='states'>
	/// Every state, including orphans
	/// </param>
	/// <param name='transitions'>
	/// All valid fromState/transition/toState combinations
	/// </param>
	public void GetGraph(out List<FiniteState> states, out List<StateTransitionState> transitions){
		states = new List<FiniteState>();
		transitions = new List<StateTransitionState>();
		
		Dictionary<FiniteState,FiniteState> stateMap = new Dictionary<FiniteState, FiniteState>();
		Dictionary<StateTransitionState,bool> transitionMap = new Dictionary<StateTransitionState, bool>();
		foreach(StateTransitionState sts in StateCache){
			
			if(sts.StartState && !stateMap.ContainsKey(sts.StartState)){
				stateMap.Add(sts.StartState,sts.StartState);
				states.Add(sts.StartState);
			} else if(sts.StartState) {
				sts.StartState = stateMap[sts.StartState];
			}
			
			if(sts.EndState && !stateMap.ContainsKey(sts.EndState)){
				stateMap.Add(sts.EndState,sts.EndState);
				states.Add(sts.EndState);
				Debug.Log("Added EndState: "+sts.EndState.UniqueID);
			}else if(sts.EndState) {
				Debug.Log("Updating EndState: "+sts.EndState.UniqueID);
				sts.EndState = stateMap[sts.EndState];
			}
		}
		foreach(StateTransitionState sts in StateCache){
			if(sts.Valid() && !transitionMap.ContainsKey(sts))
			{
				transitionMap.Add(sts,true);
				transitions.Add(sts);
			}
		}
	}
	//TODO:  This probably doesn't need to be static since each FSM will self contain its states
	public static FiniteState CreateState(){
		nameSalt += 1;
		return new FiniteState{UniqueID=nameSalt};
	}
	public void SetStart(FiniteState start){
		SetStart(start,true);
	}
	public void SetStart(FiniteState start,bool clear){
		if(clear){
			foreach(StateTransitionState sts in StateCache){
				if(sts.StartState != null)sts.StartState.Start = false;
			}
		}
		start.Start = true;
		StartState = start;
	}
	/// <summary>
	/// Called by 'awake()' to rebuild dictionaries, which Unity doesn't serialize
	/// </summary>
	void RebuildStateTransitionMap(){
		TransitionMap.Clear();
		StateTransitions.Clear();
		StartState = null;
		CurrentState = null;
		
		//Debug.Log("Rebuilding transition map");
		//Unity serialization doesn't work w/ Generic.Dictionary, rebuild from state list
		foreach(StateTransitionState sts in StateCache){
 			AddState(sts.StartState,false);
			AddState(sts.EndState,false);
			
			if(sts.StartState!=null && sts.Transition!=null) {
				StateTransition st = new StateTransition(sts.StartState,sts.Transition);
				StateTransitions.Add(st,sts.EndState);
				
				TransitionMap[sts.StartState].Add(sts.Transition);
				
				if(sts.StartState.Start = true){
					SetStart(sts.StartState,false);
				}
			}
			
			if(sts.Valid()){
				//Debug.Log("Rebuilding transition from " + sts.StartState.StateName + " to " + sts.EndState.StateName);
			} else if(sts.StartState!=null && sts.EndState==null){
				//Debug.Log("Rebuilding start state w/ no end state <" + sts.StartState.StateName + ">");
			} else if(sts.StartState==null && sts.EndState!=null){
				//Debug.Log("Rebuilding end state w/ no start state <" + sts.EndState.StateName + ">");
			} else if(sts.StartState==null && sts.EndState==null && sts.Transition==null){
				//Debug.Log("Warning, disconnected transition <" + sts.Transition.Name + ">");
			}
		}
		//StateTransition st1 = new StateTransition(StateCache[0].StartState,StateCache[0].Transition);
		//StateTransition st2 = new StateTransition(StartState,StateCache[0].Transition);
		//Debug.Log("Comparison Hash.  " + st1.GetHashCode() + "==" + st2.GetHashCode());
		//Debug.Log("Comparison Equl.  " + (st1 == st2).ToString());
	}
	
	void Awake() {
		RebuildStateTransitionMap();
	}
	
	
	/// <summary>
	/// Generate a unique FiniteState name.
	/// </summary>
	public string UniqueName(string baseName){
		nameSalt += 1;	//for testing we'll just append a numerical index that always increments.
		return baseName + nameSalt.ToString();

		/*int index = 0;
		int hardLimit = 50;
		string ret = baseName;
		while(HasState(ret) || index >= hardLimit){
			ret = baseName + index.ToString();
		}
		return (index==hardLimit)?null:ret;*/
	}
	
	/// <summary>
	/// Add a new state
	/// </summary>
	/// <param name='cache'>
	/// If true, state is stored in the StateCache for serialization
	/// </param>
	public void AddState(FiniteState state, bool cache){
		if(!state){
			//Debug.Log("Couldn't add state, null");
			return;
		}
		if(!TransitionMap.ContainsKey(state)){
			TransitionMap.Add(state,new List<ITransitionCommand>());
			//Debug.Log("Adding state: " + state.UniqueID);
			if(cache) {
				StateTransitionState sts = new StateTransitionState();//ScriptableObject.CreateInstance<StateTransitionState>() as StateTransitionState;
				sts.StartState = state;	
				StateCache.Add(sts);
			}
		}
	}	
	
	/// <summary>
	/// Add a new state, (cache defaults true)
	/// </summary>
	public void AddState(FiniteState state){
		AddState(state,true);
	}
	
	/// <summary>
	/// Map the transitiom from 'fromState' to 'toState'.
	/// </summary>
	/// <returns>
	/// false if there is a transition from 'fromState' matching 'transition', else true;
	/// </returns>
	bool MapTransition(FiniteState fromState, ITransitionCommand transition,FiniteState toState,bool cache){
		StateTransition st = new StateTransition(fromState,transition);
		if(StateTransitions.ContainsKey(st)){
			return false;
		}
		
		AddState(fromState,cache);
		AddState(toState,cache);
		
		StateTransitions.Add(st,toState);
		TransitionMap[fromState].Add(transition);
		
		if(cache){
			StateTransitionState sts = new StateTransitionState();//ScriptableObject.CreateInstance<StateTransitionState>() as StateTransitionState;
			sts.StartState = fromState;	
			sts.EndState = toState;
			sts.Transition = transition;
			StateCache.Add(sts);	
		}
		return true;
	}
	
	/// <summary>
	/// Map the transitiom from 'fromState' to 'toState'.
	/// </summary>
	/// <returns>
	/// false if there is a transition from 'fromState' matching 'transition', else true;
	/// </returns>
	bool MapTransition(FiniteState fromState, ITransitionCommand transition,FiniteState toState){
		return MapTransition(fromState,transition,toState,true);
	}
	
	/// <summary>
	/// Add a new transition from 1 state to another.
	/// </summary>
	public ITransitionCommand AddTransition(FiniteState fromState, System.Type transition_type, FiniteState toState){
		// Components should only be instantiated by a game object, which is why AddTransition 
		// takes a transition_type as an argument and not a fully instantiated component.
		
		ITransitionCommand component = gameObject.AddComponent(transition_type) as ITransitionCommand;
		return AddTransition(fromState,component,toState);
	}
	
	/// <summary>
	/// Add a new transition from 1 state to another
	/// </summary>
	/// <returns>
	public ITransitionCommand AddTransition(FiniteState fromState, ITransitionCommand component, FiniteState toState){
		component.enabled = false;
		component.FSM = this;
		
		StateTransition st = new StateTransition(fromState,component);
		//Debug.Log("Adding new transition type " + component.GetType() + " from state " + fromState.StateName + " to state " + toState.StateName);
		
		if(!MapTransition(fromState,component,toState)){
			Destroy(component);	//always delete duplicate transition components
			//Debug.Log("fromState " + fromState.StateName + " already contains a transition type " + component.GetType().ToString());
			return null;
		}
		return component;
	}

	/// <summary>
	/// Called by ITransitionCommand's when the transitional condition is met, triggers state change
	/// </summary>
	public void OnTransition(ITransitionCommand transition){
		StateTransition st = new StateTransition(CurrentState,transition);
		ChangeState(StateTransitions[st]);
	}
	
	/// <summary>
	/// Set state of active component list
	/// </summary>
	public void SetActiveComponentsEnabled(bool enabled){
		if(ActiveTransitionList==null)return;
		foreach(ITransitionCommand m in ActiveTransitionList){
			m.ResetTransition();
			m.enabled = enabled;
			m.Enabled = (enabled)?"true":"false";	// 'enabled' isn't exposed to the inspector so create a string version
		}
	}

	/// <summary>
	/// Change to the next state and update the currently active transitions.
	/// </summary>
	public void ChangeState(FiniteState nextState){
		if(!nextState) {
			Debug.Log("Change state ignored, nextState == null");
			return;
		}

		// disable any active transition components
		SetActiveComponentsEnabled(false);
		
		// apply transition logic from current state
		if(CurrentState){
			CurrentState.ExitState(gameObject);
			CurrentState.DisableStateChanges();
		}
		CurrentState = nextState;
		CurrentState.EnableStateChanges();
		
		// apply transition to new state
		CurrentState.EnterState(gameObject);

		// Change the transition component list to the active states transitions and enable them
		ActiveTransitionList = TransitionMap[CurrentState];
		SetActiveComponentsEnabled(true);
	}
	
	void Start(){
		ChangeState(StartState);
	}
}
