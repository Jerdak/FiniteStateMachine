using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StateActions.SA_SetVariable))] 
class SA_SetVariableEditor : Editor {
    public override void OnInspectorGUI () {
		StateActions.SA_SetVariable ctarget = (StateActions.SA_SetVariable) target;
		ctarget.Variable.LayoutField();
    }
}
public static class DynamicVariableMethods {
	private static readonly IDictionary<System.Type,System.Action<System.Object>> _dispatchMap 
            = new Dictionary<System.Type, System.Action<System.Object>>();
    static DynamicVariableMethods()
    {
		_dispatchMap[typeof(float)] = x => LayoutField( (float)x );
		_dispatchMap[typeof(System.Int32)] = x => LayoutField( (System.Int32)x );
		_dispatchMap[typeof(System.Double)] = x => LayoutField( (System.Double)x );
		_dispatchMap[typeof(UnityEngine.Vector2)] = x => LayoutField( (UnityEngine.Vector2)x );
        _dispatchMap[typeof(UnityEngine.Vector2)] = x => LayoutField( (UnityEngine.Vector2)x );
        _dispatchMap[typeof(UnityEngine.Vector3)] = x => LayoutField( (UnityEngine.Vector3)x );
        _dispatchMap[typeof(UnityEngine.Vector4)] = x => LayoutField( (UnityEngine.Vector4)x );
    }
	public static void LayoutField(this object instance){
		if(instance == null){
			//Debug.Log("null instance passed to DynamicvariableMethods");
			return;
		}
		if(!_dispatchMap.ContainsKey(instance.GetType())){
			Debug.Log("DynamicVariableMethods dispatch map unhandled type: " + instance.GetType());
			return;
		}
		_dispatchMap[instance.GetType()](instance);
	}
	public static void LayoutField(System.Int32 instance){
		System.Int32 v = (System.Int32)instance;
		instance = EditorGUILayout.IntField("Variable",v); 	
	}
	public static void LayoutField(float instance){
		float v = (float)instance;
		instance = EditorGUILayout.FloatField("Variable",v); 	
	}
	public static void LayoutField(System.Double instance){
		float v = (float)instance;
		instance = EditorGUILayout.FloatField("Variable",v); 	
	}
	public static void LayoutField(UnityEngine.Vector2 instance){
		Vector2 v = (Vector2)instance;
		instance = EditorGUILayout.Vector2Field("Variable",v); 	
	}
	public static void LayoutField(UnityEngine.Vector3 instance){
		Vector3 v = (Vector3)instance;
		instance = EditorGUILayout.Vector3Field("Variable",v); 	
	}
	public static void LayoutField(UnityEngine.Vector4 instance){
		Vector4 v = (Vector4)instance;
		instance = EditorGUILayout.Vector4Field("Variable",v); 	
	}
}
public static class VariableExtensionMethods{
    private static void LayoutField(this UnityEngine.Vector2 instance )
    {
		Vector2 v = (Vector2)instance;
		instance = EditorGUILayout.Vector2Field("Variable",v); 
    }

    private static void LayoutField(this UnityEngine.Vector3 instance)
    {
		Vector3 v = (Vector3)instance;
		instance = EditorGUILayout.Vector3Field("Variable",v); 
    }

    private static void LayoutField(this UnityEngine.Vector4 instance)
    {
		Vector4 v = (Vector4)instance;
		instance = EditorGUILayout.Vector4Field("Variable",v); 
    }
}