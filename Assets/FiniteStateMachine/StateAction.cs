using UnityEngine;
using System.Reflection;
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
			//Debug.Log("SA_ChangeColor");
			gameObject.renderer.material.color = NewColor;
		}
	}
	public class SA_PressurePlateColor : IStateAction {
		public Color NewColor = Color.white;
		public override void Act(){
			PressurePlate pp = gameObject.GetComponent<PressurePlate>();
			pp.Target.renderer.material.color = NewColor;
		}
	}
	public class SA_PressurePlateLight : IStateAction {
		public bool State = false;
		public override void Act(){
			PressurePlate pp = gameObject.GetComponent<PressurePlate>();
			foreach(GameObject target in pp.Targets){
				Light light = target.GetComponent<Light>();
				light.enabled = !light.enabled;
			}
		}
	}
	public class SA_SetVariable : IStateAction {
		public object Variable = null;
		public object Value = null;
		// Could we have a Dictionary<System.Type,List<object>> type_values???
		public override void Act(){
			//Debug.Log("SA_SetVariable");
			Variable = Value;
		}
	}
	public class SA_SetScriptVariable : IStateAction {
		public string ScriptName = null;
		public string ValueName = null;
		public object Value = null;
		public GameObject Go =null;
		// Could we have a Dictionary<System.Type,List<object>> type_values???
		public override void Act(){
			GameObject go = (Go==null)?gameObject:Go;
			Component comp = go.GetComponent(ScriptName);
			if(comp==null){
				Debug.Log("gameObject does not contain component <" + ScriptName + ">.");
				return;
			}
			System.Type type = comp.GetType();
			FieldInfo fieldInfo = type.GetField(ValueName);
			if(fieldInfo == null){
				Debug.Log("No field of name <"+ValueName+"> was found");
				return;
			}
			fieldInfo.SetValue(comp,Value);
		}
	}
}

/*
public static class Vector3Extension{
	public static void Hooah(this Vector3 v,int i){
		Debug.Log("Hooah worked: " + i);
	}
}

public static class IntExtension{
	public static void Hooah(this int i){
		Debug.Log("Int hooah");
	}
	public static void Hooah(this object o){
		Debug.Log("object hooah");
	}
	public static void Hooah(this Vector3 o){
		Debug.Log("vector3 hooah");
	}
}*/