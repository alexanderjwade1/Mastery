using UnityEngine;
using System.Collections;

public class guage_click : MonoBehaviour {

	public AudioClip tink;
	public AudioClip select;
	
	private AudioSource source;
	
	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseEnter()
	{
		// not past cue select
		if (GM.instance.curr_game_phase != GM.game_phase.FEELINGS) return;
		
		// hover color
//		gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
		
		// hover sound
		source.PlayOneShot (tink);
		
	}
	
	void OnMouseExit()
	{
		// reset color
		gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
	}
	
	void OnMouseDown()
	{
		// not past cue select
		if (GM.instance.curr_game_phase != GM.game_phase.FEELINGS) return;
		
		// select debug
		//		Debug.Log ("touched Cue!");
		
		// select sound
		source.PlayOneShot (select);
		
		// register cue 
		GM.instance.RegisterGuageClick ();
		
	}
}
