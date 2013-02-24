using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ScriptEditor_FSM: EditorWindow {
	FiniteStateMachine2 FSM = null;
	
	class TransitionButton {
		public Rect rect = new Rect(0,0,100,50);
		public ITransitionCommand2 transition;
	};
	class StateWindow {	
		public FiniteState state = null;
		public Rect rect = new Rect(0,0,100,100);
		public List<TransitionButton> transitions = new List<TransitionButton>();
	};
	class TransitionPair {
		public FiniteState fromState = null;
		public FiniteState toState = null;
		public int fromStateIndex = -1;
		public int toStateIndex = -1;
	};
	List<StateWindow> States = new List<StateWindow>();
	List<FiniteState> FiniteStates = new List<FiniteState>();
	List<TransitionPair> Transitions = new List<TransitionPair>();
	
    [MenuItem("Window/ScriptEditor_FSM")]
    static void init()
    {
        ScriptEditorDebughelpers_FSM.openScriptEditor();
    }
	void AddState(){
	
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
	
	void Update(){
		if(Selection.gameObjects.Length != 0){
			GameObject go = Selection.gameObjects[0];
			FSM = go.GetComponent<FiniteStateMachine2>() as FiniteStateMachine2;
		}
	}
	
	void AddState(object obj){
		if(FSM == null) {
			Debug.Log("No FSM selected");
			return;
		}
		FiniteState state 	= new FiniteState();
		state.EnterAction = FSM.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
		state.Name = "change_color";
			
		Vector2 pos = (Vector2)obj;
		StateWindow wnd = new StateWindow();
		wnd.rect.x = pos.x;
		wnd.rect.y = pos.y;
		States.Add(wnd);
		FiniteStates.Add(state);
		EditorUtility.SetDirty(FSM);
		
	}
	void AttachFSM(object obj){
		if(Selection.gameObjects.Length == 0){
			return;
		}
		GameObject go = Selection.gameObjects[0];
		go.AddComponent<FiniteStateMachine2>();
	}
	void AddTransition(object obj){
		TransitionPair tp = (TransitionPair)obj;
		{
			OnTimer2 t = FSM.AddTransitionType(tp.fromState,typeof(OnTimer2),tp.toState) as OnTimer2;
			t.Name = "timer_transition";
			t.FSM = FSM;
			t.Delay = 2.0f;
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
		int index = 0;
		foreach(StateWindow wnd in States){
			if(!wnd.rect.Contains(evt.mousePosition)){
				menu.AddItem (new GUIContent ("Add/TransitionTo/"+ FiniteStates[index].Name), false, AttachFSM, evt.mousePosition);
			}
			index += 1;
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
            menu.ShowAsContext ();
			evt.Use();
			Debug.Log("OnContextMenu::ContextClick");
		}
	
		//menu.AddItem (new GUIContent ("Add/NewTransition/Timer"), false, AddTransition, evt.mousePosition);
		
	}
	void SetStartState(object obj){
		int id = (int)obj;
		FiniteState state = FiniteStates[id];
		FSM.ChangeState(state);
		EditorUtility.SetDirty(FSM);
	}
	void OnTransitionButton(int id){
		GenericMenu menu = new GenericMenu();
		int index = 0;
		foreach(StateWindow wnd in States){
			if(index != id){
				menu.AddItem (
					new GUIContent ("Add/TransitionTo/" + FiniteStates[index].Name), 
					false, 
					AddTransition, 
					new TransitionPair{fromState = FiniteStates[id],fromStateIndex = id,
									   toState = FiniteStates[index],toStateIndex = index}
				);
			}
			index += 1;
		}
		menu.AddItem (new GUIContent ("SetStart"),false,SetStartState,id);
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
		OnContextMenu();
		
		foreach(TransitionPair tp in Transitions){
			Color base_color = new Color(0.4f, 0.4f, 0.5f);
        	curveFromTo(
				States[tp.fromStateIndex].rect, 
				States[tp.toStateIndex].rect, 
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
		
		foreach(StateWindow wnd in States){
			wnd.rect = GUI.Window(index++, wnd.rect, doWindow, "hello");
		}
		EndWindows();
    }
}
