using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CarPath : MonoBehaviour {

	[HideInInspector]
	public List<GameObject> Nodes = new List<GameObject>();
	//public List<float> Speeds = new List<float>();

	public List<NodeListClass> NodesProperties = new List<NodeListClass>();

	[HideInInspector]
	public float RotationSpeed = 50f;
	[HideInInspector]
	private int NumberOfTurnPoints = 100;
	[HideInInspector]
	public float NodeThreshold = 0.25f;

	public bool Repeat = true;
	public bool ReverseDirections = false;
	public bool SingleSpeedForAllNodes = false;
	[Range(0.01f,120f)]
	public float SingleSpeed;
	//public bool UpdateAfterEveryLoop = false;

	public GameObject NodeParent;

	List<Vector3> points = new List<Vector3>();

	public IEnumerator ienum;

	Quaternion SampledRotation;

	void Update(){



	}

	void OnDrawGizmos(){

		if(Repeat){
			if(Nodes.Count >= 2){
				Gizmos.DrawLine(Nodes.Last().transform.position,Nodes.First().transform.position);
			}
		}else{

			Nodes.First().GetComponent<NodeScript>().BefNode = null;
			Nodes.First().GetComponent<NodeScript>().CurvePoint = false;

			Nodes.Last().GetComponent<NodeScript>().AftNode = null;
			Nodes.Last().GetComponent<NodeScript>().CurvePoint = false;

		}

		//foreach(Vector3 point in points){

			//Gizmos.DrawSphere(point, 0.15f);

		//}

	}

	/*private float m_LastEditorUpdateTime;

	public virtual void Play(){
		
		//Debug.Log("Nothing here ATM");
		m_LastEditorUpdateTime = Time.realtimeSinceStartup;
		EditorApplication.update += OnEditorUpdate;

	}

	protected virtual void Stop(){

		//Debug.Log("Nothing here ATM");
		EditorApplication.update -= OnEditorUpdate;
		transform.position = points.First();
		//transform.LookAt()

	}

	protected virtual void OnEditorUpdate()
	{
		// In here you can check the current realtime, see if a certain
		// amount of time has elapsed, and perform some task.
	}*/

	public void AddNode(){

		if(NodeParent != null){

			GameObject new_node = new GameObject("Node");

			new_node.AddComponent<NodeScript>();

			if(Nodes.Count == 0){
				new_node.transform.position = transform.position;
			}else{
				new_node.GetComponent<NodeScript>().BefNode = Nodes.Last();
				new_node.transform.position = Nodes.Last().transform.position;
				Nodes.ToArray()[Nodes.Count-1].GetComponent<NodeScript>().AftNode = new_node;
				Nodes.First().GetComponent<NodeScript>().BefNode = new_node;
			}

			new_node.transform.parent = NodeParent.transform;
			new_node.GetComponent<NodeScript>().Car = gameObject;

			#if UNITY_EDITOR

			Selection.activeGameObject = new_node;

			#endif

			Nodes.Add(new_node);

		}else{

			Debug.Log("No Node Parent set, creating one automatically");
			NodeParent = new GameObject("NodeParent");
			NodeParent.transform.position = Vector3.zero;

			GameObject new_node = new GameObject("Node");

			new_node.AddComponent<NodeScript>();

			if(Nodes.Count == 0){
				new_node.transform.position = transform.position;
			}else{
				new_node.GetComponent<NodeScript>().BefNode = Nodes.Last();
				new_node.transform.position = Nodes.Last().transform.position;
				Nodes.ToArray()[Nodes.Count-1].GetComponent<NodeScript>().AftNode = new_node;
				Nodes.First().GetComponent<NodeScript>().BefNode = new_node;
			}

			new_node.transform.parent = NodeParent.transform;
			new_node.GetComponent<NodeScript>().Car = gameObject;

			#if UNITY_EDITOR

			Selection.activeGameObject = new_node;

			#endif

			Nodes.Add(new_node);

		}


	}

	public void RemoveNode(){

		if(Nodes.Count != 0){
			DestroyImmediate(Nodes.Last());
			Nodes.Remove(Nodes.Last());

			//Nodes.Last().GetComponent<NodeScript>().AftNode = null;
			//Nodes.First().GetComponent<NodeScript>().AftNode = null;

			if(Nodes.Last().GetComponent<NodeScript>().CurvePoint || Nodes.First().GetComponent<NodeScript>().CurvePoint){
				Nodes.Last().GetComponent<NodeScript>().CurvePoint = false;
				Nodes.First().GetComponent<NodeScript>().CurvePoint = false;
			}

		}else{

			Debug.Log("No nodes to remove");

		}

	}

	public void RecalculatePath(){

		//if(Application.isPlaying) {

		NodesProperties.Clear();

		for(int i = 0; i < Nodes.Count; i++) {
			//Debug.Log("yup");
			NodesProperties.Add(Nodes.ToArray()[i].GetComponent<NodeScript>().ThisNodeClass);
			//Debug.Log(Nodes.ToArray()[i].GetComponent<NodeScript>().ThisNodeClass.Speed);
			//Debug.Log(NodesTest.ToArray()[i].Speed);
		}

		float sum = 0f;
		foreach(GameObject node in Nodes) {
			sum += node.GetComponent<NodeScript>().Speed;
		}
		sum = sum / Nodes.Count;

		//sum_p = sum;

		if(sum < 12f) {

			NodeThreshold = 0.35f;
			RotationSpeed = 30f;

		} else if(sum >= 20f && sum < 50f) {

			NodeThreshold = 0.95f;
			RotationSpeed = 100f;

		} else if(sum >= 50f && sum < 100f) {

			NodeThreshold = 1.55f;
			RotationSpeed = 120f;

		} else if(sum >= 100f && sum < 200f) {

			NodeThreshold = 1.95f;
			RotationSpeed = 150f;

		} else if(sum >= 200 && sum < 300) {

			NodeThreshold = 2.75f;
			RotationSpeed = 250f;

		} else if(sum >= 300) {

			NodeThreshold = 2.5f;
			RotationSpeed = 350f;

		}

		StopAllCoroutines();

		points.Clear();

		int n = 0;

		while(n < Nodes.ToArray().Length) {

			if(Nodes.ToArray()[n].GetComponent<NodeScript>().CurvePoint == true) {

				float t = 0f;
				Vector3 position = Vector3.zero;
				for(int i = 0; i < NumberOfTurnPoints; i++) {
					t = i / (NumberOfTurnPoints - 1.0f);
					position = (1.0f - t) * (1.0f - t) * Nodes.ToArray()[n].GetComponent<NodeScript>().CurveStartPoint + 2.0f * (1.0f - t) * t * Nodes.ToArray()[n].transform.position + t * t * Nodes.ToArray()[n].GetComponent<NodeScript>().CurveEndPoint;

					points.Add(position);
				}

				n++;

			} else {

				points.Add(Nodes.ToArray()[n].transform.position);
				n++;

			}

		}

		if(ReverseDirections){
			points.Reverse ();
		}

		if(Nodes.Count >= 2) {

			StartCoroutine(Following());

		} else {

			Debug.Log("Too few nodes to follow");

		}



		//} else {
		
			//Debug.Log("Use 'Recalculate Path' in Play mode, the path gets updated after every loop in edit mode");

		//}

	}
		
	void OnValidate(){

		if(SingleSpeedForAllNodes) {
			foreach(NodeListClass node_lc in NodesProperties) {
				node_lc.Node.GetComponent<NodeScript>().Speed = SingleSpeed;
			}
		} else {
			foreach(NodeListClass node_lc in NodesProperties) {
				node_lc.Node.GetComponent<NodeScript>().Speed = node_lc.Speed;
			}
		}

		//if(Application.isPlaying){
			//RecalculatePath();
		//}

		//RecalculatePath();

	}
		
	protected virtual void EditorUpdate(){
		if(ienum == null) {
			ienum = Following();
		}
		if(!Application.isPlaying){
			if(ienum.MoveNext()) {
				for(int i = 0; i < 2500; i++) {
					ienum.MoveNext();
				}
			}
		}
			
	}

	public void SampleRotation(){
		SampledRotation = transform.rotation;
	}

	public void MTSP(){

		transform.position = Nodes.First ().transform.position;
		transform.rotation = SampledRotation;

	}

	#if UNITY_EDITOR

	public void Play(){
		//MonoBehaviour.res
		if(!Application.isPlaying) {
			//StopAllCoroutines();
			EditorApplication.update -= EditorUpdate;
			//RecalculatePath();
			EditorApplication.update += EditorUpdate;
			//goto restart;
		} else {
			Debug.Log("Play is only used in edit mode");
		}
	}

	public void Stop(){
		if(!Application.isPlaying) {
			EditorApplication.update -= EditorUpdate;
			//transform.position = Nodes.First().transform.position;
			//StopCoroutine(ienum);
			StopAllCoroutines();
		} else {
			Debug.Log("Stop is only used in edit mode");
		}
	}

	#endif

	void Start () {

		if(!Application.isPlaying) {
			ienum = Following();
		}

		//NodesTest = new List<NodeListClass>(new NodeListClass[Nodes.Count]);

		if(NodesProperties.Count == 0){
			for(int i = 0; i < Nodes.Count; i++){
				//Debug.Log("yup");
				NodesProperties.Add(Nodes.ToArray()[i].GetComponent<NodeScript>().ThisNodeClass);
				//Debug.Log(Nodes.ToArray()[i].GetComponent<NodeScript>().ThisNodeClass.Speed);
				//Debug.Log(NodesTest.ToArray()[i].Speed);
			}
		}

		//EditorApplication.update -= TestUpdate;

		//Play();
		float sum = 0f;
		foreach(GameObject node in Nodes){
			sum+=node.GetComponent<NodeScript>().Speed;
		}
		sum=sum/Nodes.Count;

		//sum_p = sum;

		if(sum < 12f){

			NodeThreshold = 0.35f;
			RotationSpeed = 30f;

		}else if(sum >= 20f && sum < 50f){

			NodeThreshold = 0.95f;
			RotationSpeed = 100f;

		}else if(sum >= 50f && sum < 100f){

			NodeThreshold = 1.35f;
			RotationSpeed = 120f;

		}else if(sum >= 100f && sum < 200f){

			NodeThreshold = 1.55f;
			RotationSpeed = 150f;

		}else if(sum >= 200 && sum < 300){

			NodeThreshold = 2.75f;
			RotationSpeed = 250f;

		}else if(sum >= 300){

			NodeThreshold = 2.5f;
			RotationSpeed = 350f;

		}


		//NodeThreshold = Mathf.Clamp(sum/20f,0.2f, 1.25f);
		//RotationSpeed = Mathf.Clamp(sum/1.5f, 20f, 200f);

		int n = 0;

		while(n < Nodes.ToArray().Length){

			if(Nodes.ToArray()[n].GetComponent<NodeScript>().CurvePoint == true){

				float t = 0f;
				Vector3 position = Vector3.zero;
				for(int i = 0; i < NumberOfTurnPoints; i++) 
				{
					t = i / (NumberOfTurnPoints - 1.0f);
					position = (1.0f - t) * (1.0f - t) * Nodes.ToArray()[n].GetComponent<NodeScript>().CurveStartPoint + 2.0f * (1.0f - t) * t * Nodes.ToArray()[n].transform.position + t * t * Nodes.ToArray()[n].GetComponent<NodeScript>().CurveEndPoint;

					points.Add(position);
				}

				n++;

			}else{

				points.Add(Nodes.ToArray()[n].transform.position);
				n++;

			}

		}

		if(ReverseDirections){
			points.Reverse ();
		}

		if(Nodes.Count >= 2){

			StartCoroutine(Following());

		}else{

			Debug.Log("Too few nodes to follow");

		}

		//IEnumerator e = Following();
		//while (e.MoveNext());

	}

	public void ReevaluatePath(){
		Nodes.Clear();// = Car.GetComponent<CarPath>().NodeParent.GetComponentsInChildren<GameObject>().ToList();

		foreach (Transform child in NodeParent.transform)
		{
			Nodes.Add(child.gameObject);
		}
			
		RecalculatePath();
	}

	float OriginalSpeed;

	public IEnumerator Following(){

		restart:

		int node_number = 0;

		int point_number = 0;

		foreach(Vector3 point in points){

			bool stopping = false;

			bool accelerating = false;

			//Debug.Log (Nodes.Count + " - Count");
			//Debug.Log (node_number);

	

			if (Nodes.ToArray () [node_number].GetComponent<NodeScript> ().StopPoint && Nodes.ToArray () [node_number].GetComponent<NodeScript> ().SmoothStopping) {

				stopping = true;

				OriginalSpeed = Nodes.ToArray () [node_number].GetComponent<NodeScript> ().Speed;

			} else if (node_number != 0) { 

				if (Nodes.ToArray () [node_number - 1].GetComponent<NodeScript> ().StopPoint && Nodes.ToArray () [node_number - 1].GetComponent<NodeScript> ().SmoothStopping) {

					accelerating = true;

					OriginalSpeed = Nodes.ToArray () [node_number].GetComponent<NodeScript> ().Speed;

					Nodes.ToArray () [node_number].GetComponent<NodeScript> ().Speed = 2f;

				}

			}

		

			while(Vector3.Distance(transform.position, point) > NodeThreshold){

				if(node_number < Nodes.Count) {
				transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed);
				} else {
					transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * SingleSpeed);
				}
				if(points[point_number] - transform.position != Vector3.zero) {
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(points[point_number] - transform.position, Vector3.up), Time.deltaTime * RotationSpeed);
				}else if(point_number+1 < points.Count){
					transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(points[point_number+1] - transform.position, Vector3.up), Time.deltaTime * RotationSpeed);
				}


				if(stopping) {


					if(Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed <= 0.05f) {
						if(SingleSpeedForAllNodes) {
							Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = SingleSpeed;
						} else {
							Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = OriginalSpeed;
						}
						break;
					} else {

						float param = Mathf.InverseLerp(0f, Vector3.Distance(Nodes.ToArray()[node_number].transform.position, Nodes.ToArray()[node_number - 1].transform.position), Vector3.Distance(transform.position, Nodes.ToArray()[node_number - 1].transform.position));

						Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = Mathf.Lerp(OriginalSpeed, 0f, param);

					}

				}else if(accelerating){

					if(Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed >= OriginalSpeed) {
						if(SingleSpeedForAllNodes) {
							Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = SingleSpeed;
						} else {
							Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = OriginalSpeed;
						}
						break;
					} else {

						float param = Mathf.InverseLerp(0f, Vector3.Distance(Nodes.ToArray()[node_number].transform.position, Nodes.ToArray()[node_number-1].transform.position), Vector3.Distance(transform.position, Nodes.ToArray()[node_number-1].transform.position));

						Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = Mathf.Lerp(2f,OriginalSpeed,param);

					}

				}

				yield return new WaitForFixedUpdate();

			}

			if(Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed != OriginalSpeed && stopping){
				Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed = OriginalSpeed;
			}


			point_number++;

			foreach(GameObject node in Nodes){

				if(point == node.transform.position || point == node.GetComponent<NodeScript>().CurveEndPoint){

					if (node_number + 1 < Nodes.Count) {
						node_number++;
					}

					if(node.GetComponent<NodeScript>().StopPoint){
						yield return new WaitForSeconds(node.GetComponent<NodeScript>().StopTime);
					}

					break;

				}

			}

			if(points.Last() == point && Repeat){

				StopCoroutine(Following());
				StartCoroutine(Following());

			}

			//Debug.Log(node_number);

		}

		if(!Application.isPlaying){

			RecalculatePath();
			goto restart;
			
		}

	}

	/*public IEnumerator GoingBack(){

		points.Reverse();

		Nodes.Reverse();

		int node_number = 0;

		int point_number = 0;

		foreach(Vector3 point in points){

			while(Vector3.Distance(transform.position, point) > NodeThreshold){

				transform.position = Vector3.MoveTowards(transform.position,point,Time.deltaTime * Nodes.ToArray()[node_number].GetComponent<NodeScript>().Speed);

				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(points[point_number] - transform.position,Vector3.up), Time.deltaTime * RotationSpeed);

				yield return null;

			}

			point_number++;

			foreach(GameObject node in Nodes){

				if(point == node.transform.position || point == node.GetComponent<NodeScript>().CurveEndPoint){

					node_number++;

					if(node.GetComponent<NodeScript>().StopBackwardPoint){
						yield return new WaitForSeconds(node.GetComponent<NodeScript>().StopTime);
					}

					break;

				}

			}

		}

		points.Reverse();

		Nodes.Reverse();

		StartCoroutine(Following());

	}*/
}
