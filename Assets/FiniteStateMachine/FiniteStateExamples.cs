using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FiniteStateMachine2))]
public class FiniteStateExamples : MonoBehaviour {
	public enum ExampleType {RedGreen,DelayToggle,Toggle2,ToggleRandom,RedGreenToggleTimer,PressurePlate};
	
	public ExampleType Example;
	/// <summary>
	/// Create a simple red/green toggling FSM.  Reponds to left-click, alternates colors.
	/// </summary>
	static void DebugRedGreen(FiniteStateMachine2 fsm){
		FiniteState start_state = new FiniteState();
		FiniteState red_state = new FiniteState();
		FiniteState green_state = new FiniteState();
		
		start_state.Name = "Start";
		red_state.Name = "Red";
		green_state.Name = "Green";
		
		{
			red_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = red_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.red;
		}
		
		{
			red_state.ExitAction = fsm.gameObject.AddComponent<StateActions.SA_SetVariable>() as IStateAction;
			StateActions.SA_SetVariable sa = red_state.ExitAction as StateActions.SA_SetVariable;
			sa.Variable = new Vector3(1,2,3);
		}
		
		{
			green_state.EnterAction = fsm.gameObject.AddComponent<StateActions.SA_ChangeColor>() as IStateAction;
			StateActions.SA_ChangeColor sa = green_state.EnterAction as StateActions.SA_ChangeColor;
			sa.NewColor = Color.green;
		}
			
		
		{
			OnMouseClick t = fsm.AddTransitionType(start_state,typeof(OnMouseClick),red_state) as OnMouseClick;
			t.Name = "click_start";
			t.FSM = fsm;
		}
		
		{
			OnMouseClick t = fsm.AddTransitionType(red_state,typeof(OnMouseClick),green_state) as OnMouseClick;
			t.Name = "click_red";
			t.FSM = fsm;
		}
		
		{
			OnMouseClick t = fsm.AddTransitionType(green_state,typeof(OnMouseClick),red_state) as OnMouseClick;
			t.Name = "click_green";
			t.FSM = fsm;
		}
		fsm.ChangeState(start_state);
	}
	
	/// <summary>
	/// Create a simple timer toggle FSM.  Started with a left click, object alternates between
	/// red and green every 2 seconds.
	/// </summary>
	static void DebugToggle(FiniteStateMachine2 fsm){
		/*FiniteState start_state = new FiniteState();
		FiniteState red_state = new FiniteState();
		FiniteState green_state = new FiniteState();
		start_state.Name = "Start";
		red_state.Name = "Red";
		green_state.Name = "Green";
		red_state.EnterAction = FiniteState.TestAction_ChangeColorRed;
		green_state.EnterAction = FiniteState.TestAction_ChangeColorGreen;
		
		{
			OnMouseClick t = fsm.AddTransitionType(start_state,typeof(OnMouseClick),red_state) as OnMouseClick;
			t.Name = "click_start";
			t.FSM = fsm;
		}
		
		{
			OnTimer2 t = fsm.AddTransitionType(red_state,typeof(OnTimer2),green_state) as OnTimer2;
			t.Name = "timer_red";
			t.FSM = fsm;
			t.Delay = 2;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(green_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "timer_green";
			t.FSM = fsm;
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
		 */
	}
	static void DebugToggle2(FiniteStateMachine2 fsm){
		/*FiniteState start_state = new FiniteState();
		FiniteState red_state = new FiniteState();
		FiniteState green_state = new FiniteState();
		start_state.Name = "Start";
		red_state.Name = "Red";
		green_state.Name = "Green";
		red_state.EnterAction = FiniteState.TestAction_ChangeColorRed;
		green_state.EnterAction = FiniteState.TestAction_ChangeColorGreen;
		
		{
			OnTimer2 t = fsm.AddTransitionType(start_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "timer_start";
			t.FSM = fsm;
		}
		
		{
			OnTimer2 t = fsm.AddTransitionType(red_state,typeof(OnTimer2),green_state) as OnTimer2;
			t.Name = "timer_red";
			t.FSM = fsm;
			t.Delay = 2;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(green_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "timer_green";
			t.FSM = fsm;
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
		 */
	}
	static void DebugToggleRandom(FiniteStateMachine2 fsm){
		/*FiniteState start_state 	= new FiniteState();
		FiniteState red_state 		= new FiniteState();
		FiniteState green_state 	= new FiniteState();
		FiniteState blue_state 		= new FiniteState();
		FiniteState yellow_state 	= new FiniteState();
		FiniteState magenta_state 	= new FiniteState();
		
		start_state.Name 	= "Start";
		red_state.Name 		= "Red";
		green_state.Name 	= "Green";
		blue_state.Name 	= "blue";
		yellow_state.Name 	= "yellow";
		magenta_state.Name 	= "magenta";
		
		
		red_state.EnterAction 		= FiniteState.TestAction_ChangeColorRed;
		green_state.EnterAction 	= FiniteState.TestAction_ChangeColorGreen;
		blue_state.EnterAction 		= FiniteState.TestAction_ChangeColorBlue;
		yellow_state.EnterAction 	= FiniteState.TestAction_ChangeColorYellow;
		magenta_state.EnterAction 	= FiniteState.TestAction_ChangeColorMagenta;
		
		{
			OnTimer2 t = fsm.AddTransitionType(start_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "timer_start";
			t.FSM = fsm;
			t.Delay = 1.0f;
		}
		
		{
			OnTimer2 t = fsm.AddTransitionType(red_state,typeof(OnTimer2),green_state) as OnTimer2;
			t.Name = "timer_red";
			t.FSM = fsm;
			t.Delay = Random.value * 10.0f;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(green_state,typeof(OnTimer2),blue_state) as OnTimer2;
			t.Name = "timer_green";
			t.FSM = fsm;
			t.Delay = Random.value * 10.0f;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(blue_state,typeof(OnTimer2),yellow_state) as OnTimer2;
			t.Name = "timer_blue";
			t.FSM = fsm;
			t.Delay = Random.value * 10.0f;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(yellow_state,typeof(OnTimer2),magenta_state) as OnTimer2;
			t.Name = "timer_yellow";
			t.FSM = fsm;
			t.Delay = Random.value * 10.0f;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(magenta_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "timer_magenta";
			t.FSM = fsm;
			t.Delay = Random.value * 10.0f;
		}
		fsm.ChangeState(start_state);
		*/
	}
	static void DebugRedGreenToggleTimer(FiniteStateMachine2 fsm){
		/*FiniteState start_state = new FiniteState();
		FiniteState red_state = new FiniteState();
		FiniteState green_state = new FiniteState();
		start_state.Name = "Start";
		red_state.Name = "Red";
		green_state.Name = "Green";
		red_state.EnterAction = FiniteState.TestAction_ChangeColorRed;
		green_state.EnterAction = FiniteState.TestAction_ChangeColorGreen;
		
		{
			OnMouseClick t = fsm.AddTransitionType(start_state,typeof(OnMouseClick),red_state) as OnMouseClick;
			t.Name = "click_start";
			t.FSM = fsm;
		}
		
		{
			OnMouseClick t = fsm.AddTransitionType(red_state,typeof(OnMouseClick),green_state) as OnMouseClick;
			t.Name = "click_red";
			t.FSM = fsm;
		}
		
		{
			OnMouseClick t = fsm.AddTransitionType(green_state,typeof(OnMouseClick),red_state) as OnMouseClick;
			t.Name = "click_green";
			t.FSM = fsm;
		}
		{
			OnTimer2 t = fsm.AddTransitionType(green_state,typeof(OnTimer2),red_state) as OnTimer2;
			t.Name = "click_green";
			t.FSM = fsm;
			t.Delay = 2;
		}
		fsm.ChangeState(start_state);
		*/
	}
	
	static void DebugPressurePlate(FiniteStateMachine2 fsm){
		/*FiniteState unpressed_state = new FiniteState();
		FiniteState pressed_state = new FiniteState();
		unpressed_state.EnterAction = FiniteState.TestAction_ChangeColorTargetRed;
		pressed_state.EnterAction = FiniteState.TestAction_ChangeColorTargetGreen;
		
		{
			OnTriggerEnterTransition t = fsm.AddTransitionType(unpressed_state,typeof(OnTriggerEnterTransition),pressed_state) as OnTriggerEnterTransition;
			t.Name = "trigger_enter";
			t.FSM = fsm;
		}
		{
			OnTriggerExitTransition t = fsm.AddTransitionType(pressed_state,typeof(OnTriggerExitTransition),unpressed_state) as OnTriggerExitTransition;
			t.Name = "trigger_exit";
			t.FSM = fsm;
		}
		fsm.ChangeState(unpressed_state);
		*/
	}
	
	// Use this for initialization
	void Start () {
		FiniteStateMachine2 fsm = gameObject.GetComponent<FiniteStateMachine2>() as FiniteStateMachine2;
		switch(Example){
			case ExampleType.DelayToggle:
				DebugToggle(fsm);
				break;
			case ExampleType.Toggle2:
				DebugToggle2(fsm);
				break;
			case ExampleType.PressurePlate:
				DebugPressurePlate(fsm);
				break;
			case ExampleType.RedGreen:
				DebugRedGreen(fsm);
				break;
			case ExampleType.RedGreenToggleTimer:
				DebugRedGreenToggleTimer(fsm);
				break;
			case ExampleType.ToggleRandom:
				DebugToggleRandom(fsm);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
