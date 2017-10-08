using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour 
{
	//the viewer's game object
	public GameObject view_object = null;

    [Tooltip ("How tall is the player, in meters?")]
	public float height = 1.75f;
	[Tooltip ("How fast to move to new location?")]
	public float speed = 10f;

	//a list of all waypoints
	private Waypoint[] _waypoint;

	//the current waypoint
	private Waypoint _current;

	void Start () {
		//first, if the view object is null, use the camera object
		if(view_object == null) {
			//view_object = Camera.main.gameObject;
            view_object = Camera.main.transform.parent.gameObject;
		}

		//now, find all the waypoints that have been placed in the scene
		_waypoint = FindAll();

		//and search them for the one nearest to the view object
		_current = _waypoint[0]; //Nearest();
        MoveTo(_current);
	}

	void Update () {
		if(_waypoint.Length > 0) {
			//if so, check all the waypoints to see if one of them has been hit
 			for(int i = 0; i < _waypoint.Length; i++) {
				//if a waypoint has been hit, it's an active waypoint, and the person is pressing the trigger, activate it
				if(_waypoint[i].triggered) {
					//exit the current waypoint
					_current.Depart();
			
					//set the current waypoint to be the new waypoint
					_current = _waypoint[i];
				}
			}	
			
			//if the current waypoint isn't occupied (ie, it has been changed) and we aren't already on it, move towards it
			if(_current.occupied == false && view_object.transform.position != _current.position) {
               MoveTo(_current);
			}
		}
	}

	//finds all the waypoint prefabs in the scene (tagged as "Waypoint") and puts them in an array
 	public Waypoint[] FindAll() {	
		GameObject[] waypoint_object = GameObject.FindGameObjectsWithTag("Waypoint");
		
		Waypoint[] waypoint = new Waypoint[waypoint_object.Length];

		for(int i = 0; i < waypoint_object.Length; i++) {
			waypoint[i] = waypoint_object[i].GetComponent<Waypoint>();
		}

		return waypoint;
	}

	//moves the player to the current waypoint - if the player is within .05 it snaps them directly on it
	public void MoveTo(Waypoint waypoint) {
        Vector3 destPos = waypoint.position + Vector3.up * height;
		//float distance = Vector3.Distance(view_object.transform.position, destPos);
        //Debug.Log("Moving to " + destPos + ", distance=" + distance);

		//if(distance > 0.05f) {
		//	view_object.transform.position = Vector3.Lerp(view_object.transform.position, destPos, Time.deltaTime * speed);
		//} else {
			view_object.transform.position = destPos;
        //}	
		
        _current.Occupy();
	}

	//this searches all the waypoints to find the one closest to the view
	public Waypoint Nearest() {
		int nearest_waypoint_index	= 0;
		float distance_to_nearest	= float.PositiveInfinity;

		for(int i = 0; i < _waypoint.Length; i++) {
			float distance_to_waypoint = Vector3.Distance(view_object.transform.position, _waypoint[i].position);
			
			if(distance_to_waypoint < distance_to_nearest) {
				nearest_waypoint_index	= i;
				distance_to_nearest 	= distance_to_waypoint;
			}
		}
		
		return _waypoint[nearest_waypoint_index];
	}
}
