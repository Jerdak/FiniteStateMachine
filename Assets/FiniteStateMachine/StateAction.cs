using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
[ExecuteInEditMode]
public class IStateAction : MonoBehaviour {
	public string Name = "";
	public string Description = "";
	
	public virtual void Act(){}
}

namespace StateActions {
	public class SA_ChangeColor : IStateAction {
		public Color NewColor = Color.white;
		public override void Act(){
			gameObject.renderer.material.color = NewColor;
		}
	}
	public class SA_SetVariable : IStateAction {
		public GameObject Target = null;
		public object Variable = null;
		public override void Act(){
			//if(Target)Target.SendMessage("
			//Debug.Log("Variable type: " + Variable.GetType());
			
		}
	}
	
	
}