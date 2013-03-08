using UnityEngine;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour {
	public GameObject Target = null;
	public List<GameObject> Targets = new List<GameObject>();
	
	void Start(){
		// Originally the pressure plate only handled 1 target.
		// Rather than rewriting all instances I simply move
		// the singular target in to the target array.
		Targets.Add(Target);
	}
}
