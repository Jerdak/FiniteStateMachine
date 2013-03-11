using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ScriptEditor_FSM: EditorWindow {
	FiniteStateMachine FSM = null;
	
	class StateWindow {	
		public FiniteState state = null;
	};
	class TransitionPair {
		public FiniteState fromState = null;
		public FiniteState toState = null;
	};
	
	Dictionary<FiniteState,StateWindow> StateWindowMap = new Dictionary<FiniteState,StateWindow>();
	List<StateWindow> StateWindows = new List<StateWindow>();
	List<TransitionPair> Transitions = new List<TransitionPair>();

    [MenuItem("Window/ScriptEditor_FSM")]
    static void init()
    {
        ScriptEditorDebughelpers_FSM.openScriptEditor();
    }

    void curveFromTo(Rect wr, Rect wr2, Color color, Color shadow)
    {
        Drawing.bezierLine(
            new Vector2(wr.x + wr.width, wr.y + 3 + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + 3 + wr.height / 2),
            new Vector2(wr2.x, wr2.y + 3 + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + 3 + wr2.height / 2), shadow, 5, true,20);
        Drawing.bezierLine(
            new Vector2(wr.x + wr.width, wr.y + wr.height / 2),
            new Vector2(wr.x + wr.width + Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr.y + wr.height / 2),
            new Vector2(wr2.x, wr2.y + wr2.height / 2),
            new Vector2(wr2.x - Mathf.Abs(wr2.x - (wr.x + wr.width)) / 2, wr2.y + wr2.height / 2), color, 2, true,20);
    }
	
	void OnSelectionChange(){
		Repaint();
	}
	
	void Update(){
		if(Selection.gameObjects.Length != 0){
			GameObject go = Selection.gameObjects[0];
			FiniteStateMachine tmp = go.GetComponent<FiniteStateMachine>() as FiniteStateMachine;
			UpdateFSM(tmp);
		}
	}
	void UpdateFSM(FiniteStateMachine fsm){
		if(fsm == FSM)return;
		Debug.Log("Updating FSM editor w/ new FSM");
		
		FSM = fsm;
		StateWindowMap.Clear();
		StateWindows.Clear();
		Transitions.Clear();
		if(FSM == null)return;
		
		List<FiniteState> states;
		List<StateTransitionState> transitions;
		FSM.GetGraph(out states, out transitions);
		
		foreach(FiniteState state in states){
			Debug.Log("Adding new state <" + state.StateName + ">");
		
			AddStateWindow(state);
		}
		foreach(StateTransitionState sts in transitions){
			Debug.Log("Adding new transition from <" + sts.StartState.StateName + "> to <" + sts.EndState.StateName + ">");
			Transitions.Add(new TransitionPair{fromState = sts.StartState,toState = sts.EndState});
		}
	}
	void AddStateWindow(FiniteState state){
		StateWindow wnd = new StateWindow();
		state.WindowRect.width = Mathf.Clamp(state.WindowRect.width,150,Mathf.Infinity);
		state.WindowRect.height = Mathf.Clamp(state.WindowRect.height,100,Mathf.Infinity);
		wnd.state = state;
		StateWindowMap.Add(state,wnd);
		StateWindows.Add(wnd);
	}
	void AddState(object obj){
		if(FSM == null) {
			Debug.Log("No FSM selected");
			return;
		}
		FiniteState state = FiniteStateMachine.CreateState();//ScriptableObject.CreateInstance<FiniteState>();
		Vector2 pos = (Vector2)obj;
		state.EnterAction = FSM.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
		state.StateName = FSM.UniqueName("change_color");
		state.WindowRect = new Rect(pos.x,pos.y,100,100);
		FSM.AddState(state);
		AddStateWindow(state);

		//EditorUtility.SetDirty(state);
		EditorUtility.SetDirty(FSM);
	}
	void AttachFSM(object obj){
		if(Selection.gameObjects.Length == 0){
			return;
		}
		GameObject go = Selection.gameObjects[0];
		go.AddComponent<FiniteStateMachine>();
	}
	void Purge(object obj){
		if(Selection.gameObjects.Length == 0){
			return;
		}
		GameObject go = Selection.gameObjects[0];
		foreach(FiniteStateMachine fsm in go.GetComponents<FiniteStateMachine>()){
			DestroyImmediate(fsm);
		}
		foreach( ITransitionCommand transition in go.GetComponents<ITransitionCommand>()){
			DestroyImmediate(transition);
		}
		foreach( IStateAction state in go.GetComponents<IStateAction>()){
			DestroyImmediate(state);
		}
		
		FSM = null;
		StateWindowMap.Clear();
		StateWindows.Clear();
		Transitions.Clear();
	}
	void AddTransition(object obj){
		TransitionPair tp = (TransitionPair)obj;
		{
			Transitions.OnTimer2 t = FSM.AddTransition(tp.fromState,typeof(Transitions.OnTimer2),tp.toState) as Transitions.OnTimer2;
			t.Name = "timer_transition";
			t.Delay = 2.0f;
			EditorUtility.SetDirty(t);
		}
		Transitions.Add(tp);
		EditorUtility.SetDirty(FSM);
	}
	void AddTransitionRandom(object obj){
		TransitionPair tp = (TransitionPair)obj;
		{
			Transitions.OnTimerRandom t = FSM.AddTransition(tp.fromState,typeof(Transitions.OnTimerRandom),tp.toState) as Transitions.OnTimerRandom;
			t.Name = "random_timer_transition";
			EditorUtility.SetDirty(t);
		}
		Transitions.Add(tp);
		EditorUtility.SetDirty(FSM);
	}
	bool HasFSM(){
		return (FSM == null)?false:true;
	}
	void OnStateContextMenu(){
		Event evt = Event.current;
		if(evt.type != EventType.ContextClick){
			return;
		}
		Debug.Log("StateContextMenu::ContextClick");
		
		GenericMenu menu = new GenericMenu();
		foreach(StateWindow wnd in StateWindowMap.Values){
			if(!wnd.state.WindowRect.Contains(evt.mousePosition)){
				menu.AddItem (new GUIContent ("Add/TransitionTo/"+ wnd.state.StateName), false, AttachFSM, evt.mousePosition);
			}
		}
		menu.ShowAsContext ();
		evt.Use();
	}
    void OnContextMenu(){
		Event evt = Event.current;
		if(evt.type == EventType.ContextClick){
			GenericMenu menu = new GenericMenu();
			if(!HasFSM()){
			  	menu.AddItem (new GUIContent ("AttachFSM"), false, AttachFSM, evt.mousePosition);
			} else {
				menu.AddItem (new GUIContent ("Add/NewState/ChangeColor"), false, AddState,evt.mousePosition);
	       	}
			menu.AddSeparator("");
			menu.AddItem (new GUIContent ("Purge"), false, Purge, evt.mousePosition);
            menu.ShowAsContext ();
			evt.Use();
			Debug.Log("OnContextMenu::ContextClick");
		}
	
		//menu.AddItem (new GUIContent ("Add/NewTransition/Timer"), false, AddTransition, evt.mousePosition);
		
	}
	void SetStartState(object obj){
		FSM.SetStart((FiniteState)obj);
		EditorUtility.SetDirty(FSM);
	}
	void OnTransitionButton(int id){
		GenericMenu menu = new GenericMenu();
		int index = 0;
		foreach(StateWindow wnd in StateWindows){	
			
			if(index != id){
				menu.AddItem (
					new GUIContent ("Add/TimerTransition/" + wnd.state.StateName), 
					false, 
					AddTransition, 
					new TransitionPair{fromState = StateWindows[id].state,
									   toState = StateWindows[index].state}
				);
				menu.AddItem (
					new GUIContent ("Add/RandomTimerTransition/" + wnd.state.StateName), 
					false, 
					AddTransitionRandom, 
					new TransitionPair{fromState = StateWindows[id].state,
									   toState = StateWindows[index].state}
				);
				
			}
			index += 1;
		}
		menu.AddItem (new GUIContent ("SetStart"),false,SetStartState,StateWindows[id].state);
		menu.ShowAsContext ();
		EditorUtility.SetDirty(FSM);
	}
	
	void doWindow(int id)
    {
		OnStateContextMenu();
        if(GUI.Button(new Rect(0, 30, 100, 50), "State"))OnTransitionButton(id);
        GUI.DragWindow();
    }
	void OnGUI()
    {
		if(Event.current.button == 1 && Event.current.isMouse ){
			//Debug.Log("Right click");
		}
		OnContextMenu();

		foreach(TransitionPair tp in Transitions){
			Color base_color = new Color(0.4f, 0.4f, 0.5f);
        	curveFromTo(
				tp.fromState.WindowRect, 
				tp.toState.WindowRect, 
				new Color(0.3f,0.7f,0.4f), 
				base_color
			);
		}
		/*
	    Color s = new Color(0.4f, 0.4f, 0.5f);
        curveFromTo(wr, wr2, new Color(0.3f,0.7f,0.4f), s);
        curveFromTo(wr2, wr3, new Color(0.7f,0.2f,0.3f), s);

        BeginWindows();
        wr = GUI.Window(0, wr, doWindow, "hello");
        wr2 = GUI.Window(1, wr2, doWindow, "world");
        wr3 = GUI.Window(2, wr3, doWindow, "!");
        EndWindows();*/
		BeginWindows();
		int index = 0;
		foreach(StateWindow wnd in StateWindows){
			wnd.state.WindowRect = GUI.Window(index++, wnd.state.WindowRect, doWindow, wnd.state.StateName);
			EditorUtility.SetDirty(FSM);
		}
		EndWindows();
    }
}
