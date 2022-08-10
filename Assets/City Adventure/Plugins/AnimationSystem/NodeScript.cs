using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
public class NodeScript : MonoBehaviour {

	public bool CurvePoint = false;

	[Tooltip("Stop Points don't work in Edit mode")]
	public bool StopPoint = false;

	public bool SmoothStopping = false;

	public float StopTime = 1f;

	[Range(0.0f, 100.0f)]
	public float EntryDistance = 2f;

	[Range(0.0f, 100.0f)]
	public float ExitDistance = 2f;

	[HideInInspector]
	public float Speed = 15f;

	[HideInInspector]
	public GameObject BefNode;
	[HideInInspector]
	public GameObject AftNode;

	[HideInInspector]
	public Vector3 CurveStartPoint;
	[HideInInspector]
	public Vector3 CurveEndPoint;

	[HideInInspector]
	public GameObject Car;
	[HideInInspector]
	public NodeListClass ThisNodeClass;

	void OnDestroy(){

		if(Car != null) {
			Car.GetComponent<CarPath>().Nodes.Remove(gameObject);
			Car.GetComponent<CarPath>().NodesProperties.Remove(ThisNodeClass);
		}
		if (BefNode != null) {
			BefNode.GetComponent<NodeScript> ().AftNode = AftNode;
		}
		if (AftNode != null) {
			AftNode.GetComponent<NodeScript> ().BefNode = BefNode;
		}

	}

	void OnValidate(){

		ThisNodeClass.Node = gameObject;
		ThisNodeClass.Speed = Speed;

		//List<NodeListClass> to_replace = Car.GetComponent<CarPath>().NodesTest;

		/*foreach(NodeListClass node_lc in Car.GetComponent<CarPath>().NodesTest) {

			if(node_lc.Node ==  gameObject){

				node_lc = ThisNodeClass;
				break;

			}

		}*/

		//Car.GetComponent<CarPath>().NodesTest.Find

		if(CurvePoint){

			StopPoint = false;

		}else if(StopPoint){

			CurvePoint = false;

		}

		//Car.GetComponent<CarPath>().NodeThreshold = Speed/

	}

	public void AddNode(){

		Car.GetComponent<CarPath>().AddNode();

	}

	public void RemoveNode(){

		//Car.GetComponent<CarPath>().RemoveNode();
		Car.GetComponent<CarPath>().Nodes.Remove(gameObject);
		Car.GetComponent<CarPath>().NodesProperties.Remove(ThisNodeClass);
		BefNode.GetComponent<NodeScript>().AftNode = AftNode;
		AftNode.GetComponent<NodeScript>().BefNode = BefNode;
		DestroyImmediate(gameObject);
		

	}

	void OnDrawGizmos(){

		if(BefNode != null){
			Gizmos.DrawLine(transform.position, BefNode.transform.position);
		}

		if(!CurvePoint && !StopPoint){
			Gizmos.color = Color.red;
			Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

			if(BefNode == null && Car.GetComponent<CarPath>().Repeat){

				BefNode = Car.GetComponent<CarPath>().Nodes.Last();

			}

			if(AftNode == null && Car.GetComponent<CarPath>().Repeat){

				AftNode = Car.GetComponent<CarPath>().Nodes.First();

			}

		}else if(CurvePoint && !StopPoint){

			List<Vector3> CurvePoints = new List<Vector3>();

			float t = 0f;
			Vector3 position = Vector3.zero;
			for(int i = 0; i < 50; i++) 
			{
				t = i / (50 - 1.0f);
				position = (1.0f - t) * (1.0f - t) * CurveStartPoint + 2.0f * (1.0f - t) * t * transform.position + t * t * CurveEndPoint;

				CurvePoints.Add(position);
			}

			foreach(Vector3 pos in CurvePoints){

				Gizmos.DrawCube(pos,new Vector3(0.1f, 0.1f, 0.1f));

			}


			Gizmos.color = Color.blue;
			Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

			CurveStartPoint = Vector3.MoveTowards(transform.position, BefNode.transform.position, EntryDistance);
			CurveEndPoint = Vector3.MoveTowards(transform.position, AftNode.transform.position, ExitDistance);

			Gizmos.DrawCube(CurveStartPoint, new Vector3(0.15f, 0.15f, 0.15f));
			Gizmos.DrawLine(transform.position, CurveStartPoint);

			Gizmos.DrawCube(CurveEndPoint,new Vector3(0.15f, 0.15f, 0.15f));
			Gizmos.DrawLine(transform.position, CurveEndPoint);



			if(AftNode == null){

				Debug.LogError("There must be another node after the curve node");

			}

		}else if(!CurvePoint && StopPoint){

			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

		}

	}

	void Start () {
		ThisNodeClass.Node = gameObject;
		ThisNodeClass.Speed = Speed;

		if(Car.GetComponent<CarPath>().Nodes.Contains(gameObject) == false){

			if(AftNode != null) {

				if(AftNode.GetComponent<NodeScript>().BefNode != gameObject){

					AftNode.GetComponent<NodeScript>().BefNode = gameObject;

				}

			}

			if(BefNode != null) {

				if(BefNode.GetComponent<NodeScript>().AftNode != gameObject){

					BefNode.GetComponent<NodeScript>().AftNode = gameObject;

				}

			}

			Car.GetComponent<CarPath>().Nodes.Clear();// = Car.GetComponent<CarPath>().NodeParent.GetComponentsInChildren<GameObject>().ToList();

			foreach (Transform child in Car.GetComponent<CarPath>().NodeParent.transform)
			{
				Car.GetComponent<CarPath>().Nodes.Add(child.gameObject);
			}

		}

	}
	

	void Update () {
	
	}
}
