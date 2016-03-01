using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ModalPanel : MonoBehaviour {

	public Text displayText;
	//public Image iconImage;
	public Button yesButton;
	public Button noButton;
	public Button nextButton;
	public Text nextButtonText;
	public GameObject modalPanelObject;

	private static ModalPanel modalPanel;

	private UnityAction myYesAction;
	private UnityAction myNoAction;
	private UnityAction myNextAction;

	private string first_time = "Is this your first time playing?";
	private string direction_text = "Select any cue you want.  After you click on it, you will help your character complete a challenge!";
	private string breathing_text = "Let's begin the challenge!  We're going to practice breathing.  Here's how.";
	private string breathing_text_2 = "First, breathe out.  Next, breathe in while holding down the mouse.  And then repeat.";
	private string breathing_text_3 = "We'll let you know when to breathe in and out, as well as how many breaths you have left.  Let's go!";


	private int direction_font_size = 35;

	public static ModalPanel Instance () {
		if (!modalPanel) {
			modalPanel = FindObjectOfType(typeof (ModalPanel)) as ModalPanel;
			if (!modalPanel)
				Debug.LogError ("There needs to be one active ModalPanel script on a GameObject in your scene.");
		}

		return modalPanel;
	}
		
	void ClosePanel () {
		modalPanelObject.SetActive (false);
	}

	void YesFunction () {
		//Debug.Log ("Yes");
		myNextAction = new UnityAction (NextFunction);

		GM.first_time = true;
		this.displayText.fontSize = direction_font_size;
		this.displayText.text = direction_text;
		this.yesButton.gameObject.SetActive (false);
		this.noButton.gameObject.SetActive (false);
		this.nextButton.onClick.RemoveAllListeners ();
		this.nextButton.onClick.AddListener (myNextAction);
		this.nextButton.gameObject.SetActive (true);
	}

	void NoFunction () {
		GM.first_time = false;
		GM.instance.curr_game_phase = GM.game_phase.CUE_SELECT;
		SetupFrames ();
	}

	void NextFunction () {
		ClosePanel ();
		GM.instance.curr_game_phase = GM.game_phase.CUE_SELECT;
		SetupFrames ();
	}

	void MoreDirectionsFunction () {
		myNextAction = new UnityAction (Directions3Function);
		GM.instance.curr_game_phase = GM.game_phase.FEELINGS;
		this.displayText.text = breathing_text_2;
		this.yesButton.gameObject.SetActive (false);
		this.noButton.gameObject.SetActive (false);
		this.nextButton.onClick.RemoveAllListeners ();
		this.nextButton.onClick.AddListener (myNextAction);
		this.nextButton.gameObject.SetActive (true);
	}

	void Directions3Function () {
		myNextAction = new UnityAction (ToBreathingFunction);
		GM.instance.curr_game_phase = GM.game_phase.FEELINGS;
		this.displayText.text = breathing_text_3;
		this.yesButton.gameObject.SetActive (false);
		this.noButton.gameObject.SetActive (false);
		this.nextButton.onClick.RemoveAllListeners ();
		this.nextButton.onClick.AddListener (myNextAction);
		this.nextButton.gameObject.SetActive (true);
	}

	void ToBreathingFunction () {
		ClosePanel ();
		GM.instance.curr_game_phase = GM.game_phase.BREATH_OUT;
	}

	void SetupFrames () {
		GM.instance.DisplayFrames ();
		//Debug.Log ("setup in modal");
	}

	// Use this for initialization
	public void Start () {
		if (GM.instance.curr_game_phase == GM.game_phase.INTRO) {
			myYesAction = new UnityAction (YesFunction);
			myNoAction = new UnityAction (NoFunction);

			modalPanelObject.SetActive (true);

			this.yesButton.onClick.RemoveAllListeners ();
			this.yesButton.onClick.AddListener (myYesAction);

			this.noButton.onClick.RemoveAllListeners ();
			this.noButton.onClick.AddListener (myNoAction);
			this.noButton.onClick.AddListener (ClosePanel);

			this.displayText.text = first_time;
			this.yesButton.gameObject.SetActive (true);
			this.noButton.gameObject.SetActive (true);
			this.nextButton.gameObject.SetActive (false);
		
		} else {
		
			myNextAction = new UnityAction (MoreDirectionsFunction);

			modalPanelObject.SetActive (true);
			this.displayText.fontSize = direction_font_size;
			this.displayText.text = breathing_text;
			this.yesButton.gameObject.SetActive (false);
			this.noButton.gameObject.SetActive (false);
			this.nextButtonText.text = "Continue";
			this.nextButton.onClick.RemoveAllListeners ();
			this.nextButton.onClick.AddListener (myNextAction);
			this.nextButton.gameObject.SetActive (true);

		}


		//Debug.Log ("in start");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
