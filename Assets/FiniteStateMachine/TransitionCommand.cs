using UnityEngine;
using System.Collections;

/// <summary>
/// Transition interface
/// </summary>
public interface ITransitionCommand {
	string Name { get; set; }
	string Description { get; set; }
	bool Update(GameObject go);
	void Start(GameObject go);
}

/// <summary>
/// Transition interface
/// </summary>
public class ITransitionCommand2 : MonoBehaviour {
	public string Enabled = "false";
	public string Name = "";
	public string Description = "";
	public FiniteStateMachine2 FSM { get; set; }
	
	
	public void NotifyFsm(){
		FSM.Transition(this);
	}
	public virtual void ResetTransition(){}
}
public class OnMouseClick : ITransitionCommand2 {
	public override void ResetTransition(){}
	void Intersection(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, Mathf.Infinity)){
			if(hit.collider.gameObject == gameObject){
				//Debug.Log("Hit object in component " + Name);
				NotifyFsm();
			}
		}
	}
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			Intersection();
		}
	}
}
public class OnTriggerEnterTransition : ITransitionCommand2 {
	public override void ResetTransition(){}
	void OnTriggerEnter(Collider other) {
		FSM.Transition(this);
	}
}
public class OnTriggerExitTransition : ITransitionCommand2 {
	public override void ResetTransition(){}
	void OnTriggerExit(Collider other) {
		FSM.Transition(this);	
	}
}
public class OnTimer2 : ITransitionCommand2 {
	public float Delay { get; set; }
	public float ElapsedTime { get; set; }
	
	public override void ResetTransition(){
		ElapsedTime = 0.0f;
	}
	public void Start(){
		ElapsedTime = 0.0f;
	}
	
	public void Update(){
		ElapsedTime += Time.deltaTime;
		if (ElapsedTime >= Delay)NotifyFsm();
	}	
}
/// <summary>
/// Example transition:  Left click object
/// </summary>
public class OnLeftClick : ITransitionCommand {
	public string Name { get; set; }
	public string Description { get; set; }
	
	public OnLeftClick(string name, string descrip){
		Name = name;
		Description = descrip;
	}
	public void Start(GameObject go){}
	bool Intersection(GameObject go){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray,out hit, Mathf.Infinity)){
			return (hit.collider.gameObject == go);
		}
		return false;
	}
	public bool Update(GameObject go){
		if(Input.GetMouseButtonDown(0)){
			return Intersection(go);
		}
		return false;
	}	
}

/// <summary>
/// Example transition: Delay timer
/// </summary>
public class OnTimer : ITransitionCommand {
	public string Name { get; set; }
	public string Description { get; set; }
	public float Delay { get; set; }
	public float ElapsedTime { get; set; }
	
	public void Start(GameObject go){
		ElapsedTime = 0.0f;
	}
	
	public bool Update(GameObject go){
		ElapsedTime += Time.deltaTime;
		Debug.Log("Elapsed Time: " + ElapsedTime);
		if (ElapsedTime >= Delay)return true;
		return false;
	}	
}
