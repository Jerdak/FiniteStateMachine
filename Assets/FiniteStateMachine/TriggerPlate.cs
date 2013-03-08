using UnityEngine;
using System.Collections;

public class TriggerPlate : MonoBehaviour {
	public bool IsTriggered = false;
	public GameObject Target = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Target==null)return;
		if(IsTriggered)Target.renderer.material.color = Color.green;
		else Target.renderer.material.color = Color.red;
	}
}
