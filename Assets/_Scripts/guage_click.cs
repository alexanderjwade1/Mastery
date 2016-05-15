using UnityEngine;
using System.Collections;

public class guage_click : MonoBehaviour {

	public AudioClip tink;
	public AudioClip select;
	public GameObject gaugeSprite;
	public GameObject pointerSprite;
	
	private AudioSource source;
	
	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		//get mouse position
		Vector3 mousePos = Input.mousePosition;
		float gaugeX = mousePos.x - (Screen.width/2);
		float gaugeY = mousePos.y - (Screen.height/2);

		//calculate pointer angle (-3pi/2 <= theta <= pi/2)
		float theta = Mathf.Atan2 (gaugeY, gaugeX) - (Mathf.PI/2);

		//don't want it to go below x-axis
		if (theta < (Mathf.PI) && theta < -(Mathf.PI))
			theta = (Mathf.PI / 2);
		else if (theta < -(Mathf.PI / 2))
			theta = 0 - (Mathf.PI / 2);
		
		//actually do the rotation
		pointerSprite.transform.eulerAngles = new Vector3 (0, 0, theta * Mathf.Rad2Deg);
	}
	
	void OnMouseEnter()
	{
		// not past cue select
		if (GM.instance.curr_game_phase != GM.game_phase.FEELINGS) return;

		print ("Entered");

		// hover sound
		source.PlayOneShot (tink);
		
	}
	
	void OnMouseExit()
	{
		// reset color
		//gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
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
