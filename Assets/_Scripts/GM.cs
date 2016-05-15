using UnityEngine;
using System.Collections;
using UnityEngine.UI;     // UI text
using UnityEngine.Audio;

public class GM : MonoBehaviour {

	// GLOBALS 
	public static bool first_time = true;

	// self
	[HideInInspector]
	public static GM instance = null;

	public Text Breath_In_Text;
	public Text Breath_Out_Text;

	public Text Player_Breathing_Text;
	public Text Player_Exhaling_Text;

	public Text Select_Cue_Text;
	public GameObject Select_Cue_Grouper;
	public Text Game_Over_Text;
	public Text Game_Won_Text;

	public GameObject breath_light_game_object;
	public GameObject player_light_game_object;

	public Material breath_cube_material;
	public Material player_cube_material;

	public GameObject water;

	public float waterRiseSpeed;
	public float waterFallSpeed;
	
	private float timer = 100f;

	public GameObject cues_select;
	public GameObject frame_1;
	public GameObject frame_2;
	public GameObject frame_3;
	public GameObject frame_4;

	private GameObject selectedFrame;

	public GameObject portal;
	public GameObject cubes;
	//public GameObject guage;
	public GameObject gauge2;
	public GameObject wind;

	// game phase
	[HideInInspector]
	public enum game_phase
	{
		INTRO = 0,
		CUE_SELECT,
		FEELINGS,
		BREATH_IN,
		BREATH_OUT,
		GAME_WON,
		GAME_OVER
	}

	[HideInInspector]
	public game_phase curr_game_phase = game_phase.INTRO;

	private int feelings_counter = 0;

	public AudioMixerSnapshot no_water;
	public AudioMixerSnapshot water_snap;
	public AudioMixerSnapshot game_over;

	
	public GameObject player;

	private int breath_counter = 11;
	public Text Breath_Counter_Text;

	//public ModalPanel modalPanel;


	// Use this for initialization
	void Awake () 
	{
		if (!instance)
			instance = this;
		else
			Destroy (gameObject);
		
		Setup ();
	}
	// our personal setup

	/*void YesFunction () {
		first_time = true;
	}

	void NoFunction () {
		first_time = false;
	}*/

	public void Setup()
	{
		//GameStartModalWindow gameStartModalWindow = new GameStartModalWindow();
		//gameStartModalWindow.Awake ();
		//modalPanel = ModalPanel.Instance();
		//modalPanel.Choice ("Is this your first time playing?", YesFunction, NoFunction);
	}
	
	// Update is called once per frame
	void Update () {

		if (curr_game_phase == game_phase.CUE_SELECT ||
			curr_game_phase == game_phase.FEELINGS ||
			curr_game_phase == game_phase.INTRO) {
			// INTRO LOGIC
			// do nothing, wait until RegisterCueSelect() to be called by a clicked cue frame	

		} else if (curr_game_phase == game_phase.BREATH_IN ||
		           curr_game_phase == game_phase.BREATH_OUT  ) {
			// GAME LOGIC
			Timing_And_Breath_Update ();
			Player_Input_Update ();
		}

	}

	public void Select_Screen_Update(){

	}


	
	public void Player_Input_Update (){

		//bool space = Input.GetKey (KeyCode.Space);
		bool space = Input.GetMouseButton (0);

		if (space) {

			// Breathing in
			Player_Breathing_Text.gameObject.SetActive(true);
			Player_Exhaling_Text.gameObject.SetActive(false);
			Set_Player_Color( Color.green );

			if(curr_game_phase == game_phase.BREATH_IN){
				Correct_Input();
			}else{
				Wrong_Input();
			}
		} else {

			// Exhaling
			Player_Exhaling_Text.gameObject.SetActive(true);
			Player_Breathing_Text.gameObject.SetActive(false);
			Set_Player_Color( Color.blue );

			if(curr_game_phase == game_phase.BREATH_OUT){
				Correct_Input();
			}else{
				Wrong_Input();
			}
		}

	}

	void Correct_Input(){
		if (water.gameObject.transform.position.y > -4.75f) {
			water.gameObject.transform.Translate (0, waterFallSpeed, 0);

			if(water.gameObject.transform.position.y < -2.01f)
				StopSwim();
		}
	}

	void Wrong_Input(){
		if (water.gameObject.transform.position.y < 0f)
			water.gameObject.transform.Translate (0, waterRiseSpeed, 0);
		else{
			RegisterGameOver ();
		}
		if(water.gameObject.transform.position.y > -2f){
			Swim ();
		}

	}

	void Set_Player_Color(Color col){
		// colors
		player_cube_material.color = col;
		player_light_game_object.gameObject.GetComponent<Light> ().color = col;
	}

	public void Timing_And_Breath_Update(){
		timer += Time.deltaTime;
		
		if (timer >= 2) {
			//			Debug.Log("in timer reset.  Timer: " + timer);
			
			if(curr_game_phase == game_phase.BREATH_IN){
				Begin_Breath_Out();
			}else if(curr_game_phase == game_phase.BREATH_OUT){
				Begin_Breath_In();
			}
			
//			Debug.Log("Curr_game_phase: " + curr_game_phase);
			timer = 0;
		}
	}

	public void Begin_Breath_In(){
		// game phase
		curr_game_phase = game_phase.BREATH_IN;

		// colors
		breath_cube_material.color = Color.green;
		breath_light_game_object.gameObject.GetComponent<Light> ().color = Color.green;

		// text
		Breath_In_Text.gameObject.SetActive (true);
		Breath_Out_Text.gameObject.SetActive (false);

		// incr breath counter
		Breath_Counter_Text.text = "Breaths To Go: " + --breath_counter;

		if (breath_counter == 0) {
			RegisterGameWon();
		}
	}

	public void Begin_Breath_Out(){
		// game phase
		curr_game_phase = game_phase.BREATH_OUT;

		// colors
		breath_cube_material.color = Color.blue;
		breath_light_game_object.gameObject.GetComponent<Light> ().color = Color.blue;

		// text
		Breath_Out_Text.gameObject.SetActive (true);
		Breath_In_Text.gameObject.SetActive (false);
	}

	public void DisplayFrames() {
		if (curr_game_phase == game_phase.CUE_SELECT) {
			cues_select.SetActive (true);
			//frame_1.SetActive (true);
		}
	}

	public void RegisterCueSelect(GameObject frame){

		// disable others
		if (frame.name != frame_1.name) {
			frame_1.SetActive(false);
		}
		if (frame.name != frame_2.name) {
			frame_2.SetActive(false);
		}
		if (frame.name != frame_3.name) {
			frame_3.SetActive(false);
		}
		if (frame.name != frame_4.name) {
			frame_4.SetActive(false);
		}

		selectedFrame = frame;

		// disable select cue text
		Select_Cue_Text.text = "How ANGRY does the cue  \n make your character feel?";

		// move selected cue to bottom center
		frame.gameObject.transform.position = new Vector3 (0f, -1f, 0f);

		// guage
		//guage.gameObject.SetActive(true);
		//gauge
		gauge2.gameObject.SetActive(true);

		// increment curr game phase
		curr_game_phase = game_phase.FEELINGS;
	}

	public void RegisterGuageClick(){
		if (feelings_counter++ == 0) {
			Select_Cue_Text.text = "How FEARFUL does the cue \n make your character feel?";
		} else {
			curr_game_phase = game_phase.FEELINGS;
			// move selected cue to better position
			selectedFrame.gameObject.transform.position = new Vector3 (0.25f, 1.22f, 0f);
			StartCoroutine(FinishFeelings(0.1F));
		}
	}

	IEnumerator FinishFeelings(float waitTime) {
		Select_Cue_Grouper.SetActive(false);
		//guage.gameObject.SetActive(false);
		gauge2.gameObject.SetActive(false);
		if (first_time) {
			//Debug.Log ("first");
			ModalPanel modalPanel = ModalPanel.Instance ();
			curr_game_phase = game_phase.FEELINGS;
			modalPanel.Start ();
			while (curr_game_phase != game_phase.BREATH_OUT && curr_game_phase != game_phase.BREATH_IN) {
				yield return new WaitForSeconds (waitTime);
			}
		} else {
			curr_game_phase = game_phase.BREATH_OUT;
		}
		StartGame();
	
	}
		

	public void StartGame(){
		
		//curr_game_phase = game_phase.BREATH_OUT;

		Breath_Counter_Text.gameObject.SetActive (true);

		cubes.gameObject.SetActive (true);
		portal.gameObject.SetActive (true);
		//wind.gameObject.SetActive (true);

		water.gameObject.GetComponent<AudioSource> ().Play ();
		water_snap.TransitionTo(1f);

	}

	public void Swim(){
//		Debug.Log ("swim");
		player.gameObject.GetComponent<Animator>().SetBool ("is_swimming", true );
	}

	public void StopSwim(){
//		Debug.Log ("stopping swimming");
		player.gameObject.GetComponent<Animator>().SetBool ("is_swimming", false );
	}

	public void RegisterGameWon(){
		Invoke ("Reset", 2f);
		Game_Won_Text.gameObject.SetActive (true);
	}

	public void RegisterGameOver(){
		Invoke ("Reset", 2f);
		Game_Over_Text.gameObject.SetActive (true);
	}

	void Reset(){
		Game_Over_Text.gameObject.SetActive (false);
		//first_time = true;
		Application.LoadLevel (Application.loadedLevel);
	}
}
















