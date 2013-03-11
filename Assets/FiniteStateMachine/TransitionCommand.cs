using UnityEngine;
using System.Collections;

/// <summary>
/// Transition interface
/// </summary>
[ExecuteInEditMode]

/// <summary>
/// Transition interface
/// </summary>
public class ITransitionCommand : MonoBehaviour {
	public string Enabled = "false";
	public string Name = "";
	public string Description = "";
	public FiniteStateMachine FSM = null;
	
	public void NotifyFsm(){
		FSM.OnTransition(this);
	}
	public virtual void ResetTransition(){}
}

namespace Transitions {
	public class OnMouseClick : ITransitionCommand {
		public int ClickCount = 0;
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
	public class OnTriggerEnterTransition : ITransitionCommand {
		public override void ResetTransition(){}
		void OnTriggerEnter(Collider other) {
			NotifyFsm();
		}
	}
	public class OnTriggerExitTransition : ITransitionCommand {
		public override void ResetTransition(){}
		void OnTriggerExit(Collider other) {
			NotifyFsm();
		}
	}
	public class OnTimer2 : ITransitionCommand {
		public float Delay = 0;
		public float ElapsedTime = 0;
		
		public override void ResetTransition(){
			ElapsedTime = 0.0f;
		}
		public void Start(){
			ElapsedTime = 0.0f;
		}
	
		public void Update(){
			ElapsedTime += Time.deltaTime;
			if (ElapsedTime >= Delay){
				NotifyFsm();
			}
		}	
	}
	public class OnTimerRandom : ITransitionCommand {
		public float Delay = 0;
		public float ElapsedTime = 0;
		
		public override void ResetTransition(){
			ElapsedTime = 0.0f;
		}
		public void Start(){
			Delay = Random.Range(0.0f,4.0f);
			ElapsedTime = 0.0f;
		}
	
		public void Update(){
			ElapsedTime += Time.deltaTime;
			if (ElapsedTime >= Delay){
				NotifyFsm();
			}
		}	
	}
}	//end namespace Transitions