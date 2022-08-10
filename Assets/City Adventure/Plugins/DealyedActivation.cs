using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DealyedActivation : MonoBehaviour {

	public float delay;
	List<Transform> childs;

	// Use this for initialization
	void Start () {

		childs = new List<Transform> ();
		for (int i = 0; i < transform.childCount; i++) {
			childs.Add(transform.GetChild(i));
			transform.GetChild (i).gameObject.SetActive (false);
		}

		StartCoroutine (Enable ());
	}

	IEnumerator Enable(){
		int i = 0,iterations=Mathf.RoundToInt(delay/Time.deltaTime)*200;
		if (iterations == 0)
			iterations = 1;
		while (childs.Count > 0) {
			yield return new WaitForSeconds (delay);
			for (int a = 0; a < iterations; a++) {
				if (childs.Count == 0)
					yield break;
				i = Random.Range (0, childs.Count);
				childs [i].gameObject.SetActive (true);
				childs.RemoveAt (i);
				Random.seed++;
			}
		}
	}
}
