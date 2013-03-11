using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FiniteStateMachine))]
public class FiniteStateExamples : MonoBehaviour {
	public enum ExampleType {ClickToggle,TimedToggle,Toggle2,ToggleMultiColor,ClickTimedToggle,PressurePlate,PressurePlateLight};
	
	public ExampleType Example;
	static void DebugTest(FiniteStateMachine fsm) {
		FiniteState start_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		
		start_state.StateName = "Start";
		red_state.StateName = "Red";
		green_state.StateName = "Green";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			red_state.ExitAction = fsm.gameObject.AddComponent<StateActions.SA_SetVariable>() as IStateAction;
			StateActions.SA_SetVariable sa = red_state.ExitAction as StateActions.SA_SetVariable;
			sa.Value = new Vector3(1,2,3);
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
		
		{
			green_state.ExitAction = fsm.gameObject.AddComponent<StateActions.SA_SetScriptVariable>() as IStateAction;
			StateActions.SA_SetScriptVariable sa = green_state.ExitAction as StateActions.SA_SetScriptVariable;
			sa.ScriptName = "SimpleScript";
			sa.ValueName = "Position";
			sa.Value = new Vector3(42,43,44);
		}
			
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(start_state,typeof(Transitions.OnMouseClick),red_state) as Transitions.OnMouseClick;
			t.Name = "click_start";
			
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(red_state,typeof(Transitions.OnMouseClick),green_state) as Transitions.OnMouseClick;
			t.Name = "click_red";
			
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(green_state,typeof(Transitions.OnMouseClick),red_state) as Transitions.OnMouseClick;
			t.Name = "click_green";
			
		}
		fsm.ChangeState(start_state);
	}
	
	/// <summary>
	/// Create a simple red/green toggling FSM.  Reponds to left-click, alternates colors.
	/// </summary>
	static void DebugClickToggle(FiniteStateMachine fsm){
		FiniteState start_state = new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		
		start_state.StateName = "Start";
		red_state.StateName = "Red";
		green_state.StateName = "Green";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
		
		{
			ITransitionCommand component = fsm.gameObject.AddComponent<Transitions.OnMouseClick>() as ITransitionCommand;
			Transitions.OnMouseClick t = fsm.AddTransition(start_state,component,red_state) as Transitions.OnMouseClick;
			t.Name = "click_start";
			
		}
		
		{
			ITransitionCommand component = fsm.gameObject.AddComponent<Transitions.OnMouseClick>() as ITransitionCommand;
			Transitions.OnMouseClick t = fsm.AddTransition(red_state,component,green_state) as Transitions.OnMouseClick;
			t.Name = "click_red";
			
		}
		
		{
			ITransitionCommand component = fsm.gameObject.AddComponent<Transitions.OnMouseClick>() as ITransitionCommand;
			Transitions.OnMouseClick t = fsm.AddTransition(green_state,component,red_state) as Transitions.OnMouseClick;
			t.Name = "click_green";
			
		}
		fsm.ChangeState(start_state);
	}
	
	/// <summary>
	/// Create a simple timer toggle FSM.  Started with a left click, object alternates between
	/// red and green every 2 seconds.
	/// </summary>
	static void DebugTimedToggle(FiniteStateMachine fsm){
		FiniteState start_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		start_state.StateName = "Start";
		red_state.StateName = "Red";
		green_state.StateName = "Green";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(start_state,typeof(Transitions.OnMouseClick),red_state) as Transitions.OnMouseClick;
			t.Name = "click_start";
			
		}
		
		{
			Transitions.OnTimer2 t = fsm.AddTransition(red_state,typeof(Transitions.OnTimer2),green_state) as Transitions.OnTimer2;
			t.Name = "timer_red";
			
			t.Delay = 2;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(green_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "timer_green";
			
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
	}
	static void DebugToggle2(FiniteStateMachine fsm){
		/*FiniteState start_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		start_state.StateName = "Start";
		red_state.StateName = "Red";
		green_state.StateName = "Green";
		red_state.EnterAction = FiniteState.TestAction_ChangeColorRed;
		green_state.EnterAction = FiniteState.TestAction_ChangeColorGreen;
		
		{
			Transitions.OnTimer2 t = fsm.AddTransition(start_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "timer_start";
			
		}
		
		{
			Transitions.OnTimer2 t = fsm.AddTransition(red_state,typeof(Transitions.OnTimer2),green_state) as Transitions.OnTimer2;
			t.Name = "timer_red";
			
			t.Delay = 2;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(green_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "timer_green";
			
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
		 */
	}
	static void DebugToggleMultiColor(FiniteStateMachine fsm){
		FiniteState start_state 	=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state 		=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state 	=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState blue_state 		=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState yellow_state 	=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState magenta_state 	=  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		
		start_state.StateName 	= "Start";
		red_state.StateName 		= "Red";
		green_state.StateName 	= "Green";
		blue_state.StateName 	= "blue";
		yellow_state.StateName 	= "yellow";
		magenta_state.StateName 	= "magenta";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
		
		{
			blue_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = blue_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.blue;
		}
		
		{
			yellow_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = yellow_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.yellow;
		}
		{
			magenta_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = magenta_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.magenta;
		}
		
		{
			Transitions.OnTimer2 t = fsm.AddTransition(start_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "timer_start";
			
			t.Delay = 1.0f;
		}
		
		{
			Transitions.OnTimer2 t = fsm.AddTransition(red_state,typeof(Transitions.OnTimer2),green_state) as Transitions.OnTimer2;
			t.Name = "timer_red";
			
			t.Delay = Random.value * 10.0f;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(green_state,typeof(Transitions.OnTimer2),blue_state) as Transitions.OnTimer2;
			t.Name = "timer_green";
			
			t.Delay = Random.value * 10.0f;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(blue_state,typeof(Transitions.OnTimer2),yellow_state) as Transitions.OnTimer2;
			t.Name = "timer_blue";
			
			t.Delay = Random.value * 10.0f;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(yellow_state,typeof(Transitions.OnTimer2),magenta_state) as Transitions.OnTimer2;
			t.Name = "timer_yellow";
			
			t.Delay = Random.value * 10.0f;
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(magenta_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "timer_magenta";
			
			t.Delay = Random.value * 10.0f;
		}
		fsm.ChangeState(start_state);
	}
	static void DebugClickTimedToggle(FiniteStateMachine fsm){
		FiniteState start_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState red_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState green_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		start_state.StateName = "Start";
		red_state.StateName = "Red";
		green_state.StateName = "Green";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(start_state,typeof(Transitions.OnMouseClick),red_state) as Transitions.OnMouseClick;
			t.Name = "click_start";
			
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(red_state,typeof(Transitions.OnMouseClick),green_state) as Transitions.OnMouseClick;
			t.Name = "click_red";
			
		}
		
		{
			Transitions.OnMouseClick t = fsm.AddTransition(green_state,typeof(Transitions.OnMouseClick),red_state) as Transitions.OnMouseClick;
			t.Name = "click_green";
			
		}
		{
			Transitions.OnTimer2 t = fsm.AddTransition(green_state,typeof(Transitions.OnTimer2),red_state) as Transitions.OnTimer2;
			t.Name = "click_green";
			
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
	}
	
	static void DebugPressurePlate(FiniteStateMachine fsm){
		FiniteState unpressed_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState pressed_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		
		{
			unpressed_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_PressurePlateColor>() as IStateAction;
			StateActions.SA_PressurePlateColor sa = unpressed_state.EnterAction as StateActions.SA_PressurePlateColor;
			sa.NewColor = Color.red;
		}
		
		{
			pressed_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_PressurePlateColor>() as IStateAction;
			StateActions.SA_PressurePlateColor sa = pressed_state.EnterAction as StateActions.SA_PressurePlateColor;
			sa.NewColor = Color.green;
		}
		
		{
			Transitions.OnTriggerEnterTransition t = fsm.AddTransition(unpressed_state,typeof(Transitions.OnTriggerEnterTransition),pressed_state) as Transitions.OnTriggerEnterTransition;
			t.Name = "trigger_enter";
			
		}
		
		{
			Transitions.OnTriggerExitTransition t = fsm.AddTransition(pressed_state,typeof(Transitions.OnTriggerExitTransition),unpressed_state) as Transitions.OnTriggerExitTransition;
			t.Name = "trigger_exit";
			
		}
		fsm.ChangeState(unpressed_state);
		
	}
	static void DebugPressurePlateLight(FiniteStateMachine fsm){
		FiniteState unpressed_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		FiniteState pressed_state =  new FiniteState();//ScriptableObject.CreateInstance<FiniteState>();
		
		{
			//unpressed_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_PressurePlateLight>() as IStateAction;
			//StateActions.SA_PressurePlateLight sa = unpressed_state.EnterAction as StateActions.SA_PressurePlateLight;
			//sa.State = false;
		}
		
		{
			pressed_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_PressurePlateLight>() as IStateAction;
			StateActions.SA_PressurePlateLight sa = pressed_state.EnterAction as StateActions.SA_PressurePlateLight;
			sa.State = true;
		}
		
		{
			Transitions.OnTriggerEnterTransition t = fsm.AddTransition(unpressed_state,typeof(Transitions.OnTriggerEnterTransition),pressed_state) as Transitions.OnTriggerEnterTransition;
			t.Name = "trigger_enter";
			
		}
		
		{
			Transitions.OnTriggerExitTransition t = fsm.AddTransition(pressed_state,typeof(Transitions.OnTriggerExitTransition),unpressed_state) as Transitions.OnTriggerExitTransition;
			t.Name = "trigger_exit";
			
		}
		fsm.ChangeState(unpressed_state);
		
	}
	// Use this for initialization
	void Start () {
		FiniteStateMachine fsm = gameObject.GetComponent<FiniteStateMachine>() as FiniteStateMachine;
		switch(Example){
			case ExampleType.ClickToggle:
				DebugClickToggle(fsm);
				break;
			case ExampleType.TimedToggle:
				DebugTimedToggle(fsm);
				break;
			case ExampleType.Toggle2:
				DebugToggle2(fsm);
				break;
			case ExampleType.PressurePlate:
				DebugPressurePlate(fsm);
				break;
			case ExampleType.ClickTimedToggle:
				DebugClickTimedToggle(fsm);
				break;
			case ExampleType.ToggleMultiColor:
				DebugToggleMultiColor(fsm);
				break;
			case ExampleType.PressurePlateLight:
				DebugPressurePlateLight(fsm);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
