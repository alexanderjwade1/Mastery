using UnityEngine;
using System.Collections;

public class make_swim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
//		if (other.gameObject.CompareTag ("water"))
//			GM.instance.player.gameObject.GetComponent<Animator>().SetBool ("is_swimming", true );
		GM.instance.Swim ();
//		Debug.Log ("swim now");
	}

//	void OnTriggerExit2D(Collider2D other){
//		if (other.gameObject.CompareTag ("water"))
//			GM.instance.player.gameObject.GetComponent<Animator>().SetBool ("is_swimming", false );
//	}

}
