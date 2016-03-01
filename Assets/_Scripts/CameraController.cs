using UnityEngine;
using System.Collections;

public enum CameraFocus{ZOOMIN, ZOOMOUT, RESET}
public class CameraController : MonoBehaviour {

	private float initMouse;
	private float currentMouse;
	private CameraFocus focus;
	private Vector3 initCameraPos;
	private Vector3 tempCameraPos;

	//private const float MINBOUNDARY = 0.5f;
	//private const float MAXBOUNDARY = 3.5f;
	private const float MINZOOM = 1.0f;
	private const float MAXZOOM = 3.0f;

	public float cameraZoomCoefficient;	//default = 0.2f
	public GameObject player;

	// Use this for initialization
	void Start () {
		initMouse = Input.GetAxis ("Mouse ScrollWheel");
		currentMouse = initMouse;	
		focus = CameraFocus.RESET;
		initCameraPos = Camera.main.transform.position;
		tempCameraPos = new Vector3 ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("distance: " + Vector3.Distance(Camera.main.transform.position, player.transform.position));
		currentMouse = initMouse;
		initMouse = Input.GetAxis ("Mouse ScrollWheel");
		if(initMouse != 0)
		{
			if (currentMouse + initMouse < 0) 
				focus = CameraFocus.ZOOMOUT;
			else if(currentMouse + initMouse > 0)
				focus = CameraFocus.ZOOMIN;
			CameraZoom ();
		}
	}

	private void CameraZoom()
	{		
		tempCameraPos = player.transform.position;
		tempCameraPos.z = -10;

		if (focus == CameraFocus.ZOOMOUT)
		{
			if(Camera.main.orthographicSize < MAXZOOM) 
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, initCameraPos, 0.20f);
				Camera.main.orthographicSize += cameraZoomCoefficient;
			}
			else
			{
				Camera.main.orthographicSize = MAXZOOM;
				Camera.main.transform.position = initCameraPos;
			}	
		}
		else if(focus == CameraFocus.ZOOMIN)
		{
			if(Camera.main.orthographicSize > MINZOOM)
			{			
				Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, player.transform.position, 0.20f);
				Camera.main.orthographicSize -= cameraZoomCoefficient;
			}
			else
			{
				Camera.main.transform.position = tempCameraPos;
				Camera.main.orthographicSize = MINZOOM;
			}
		}
		else if(focus == CameraFocus.RESET)
		{
			Camera.main.orthographicSize = 3;
		}

	}

}
