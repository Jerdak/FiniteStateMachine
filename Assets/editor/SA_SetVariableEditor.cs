using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StateActions.SA_SetVariable))] 
class SA_SetVariableEditor : Editor {
    public override void OnInspectorGUI () {
		StateActions.SA_SetVariable ctarget = (StateActions.SA_SetVariable) target;
		if(ctarget.Variable == null) return;
		System.Type type = ctarget.Variable.GetType();
		
		Dictionary<System.Type,int> d = new Dictionary<System.Type, int>();
		d[typeof(UnityEngine.Vector3)] = 0;
	
		
		switch(d[type]){
			case 0: //UnityEngine.Vector3
				Vector3 v = (Vector3)ctarget.Variable;
				ctarget.Variable = EditorGUILayout.Vector3Field("Variable",v);
				
			break;
		}
		//SA_SetVariable ctarget = (SA_SetVariable) target;
        //ctarget.Variable = EditorGUILayout.ObjectField("Variable",ctarget.Variable,ctarget.Variable.GetType(),ctarget.Variable);
        //if (GUI.changed)
        //    EditorUtility.SetDirty (target);
    }
}