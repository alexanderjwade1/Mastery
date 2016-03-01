using UnityEngine;
using System.Collections;

public class Trigger_Game_Over : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){

//		if (other.gameObject.CompareTag ("water"))
//			GM.instance.RegisterGameOver ();

		GM.instance.Swim ();
	}
}
