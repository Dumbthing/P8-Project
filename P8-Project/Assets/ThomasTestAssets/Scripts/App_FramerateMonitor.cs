// App_FramerateMonitor: Application Essentials Package [2012.06.16]
// Copyright: NoiseCrime

// History: 
// 2012.06.16: Updated to Application Essentails Package.
// 2012.02.16: Original Version.


using UnityEngine;

public class App_FramerateMonitor : MonoBehaviour 
{
	public	float 		_updateFrequency = 2.0f;		// Frequency in seconds betwen visual updates
	
	private	float 		_fpsMin = float.MaxValue;
	private	float 		_fpsMax = float.MinValue;
	private	float 		_fpsNow = 60.0f;
	private	float 		_fpsAvg = 60.0f;
	private	float 		_fpsAccumelated = 0.0f;
	private	int 		_fpsAccumCount = 0;	
	
	private	float 		_Interval = 0;
	private float 		_nextTime = 1;
	private string		_displayString = "Initialising fps....";
//	private bool		_showFPS = true;
			
	/*
	void Awake()
	{
		if( !guiText )
			{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
			}
	}	
	*/
	
	void Start()
	{
		_nextTime 		= Time.realtimeSinceStartup + _updateFrequency;
	}
	
	void OnGUI() 
	{
        GUI.depth = 1;
		GUI.Box(new Rect(8, Screen.height-32, 512, 22),_displayString);
       // GUI.Label(new Rect(8, Screen.height-32, 512, 20), _displayString);
    }
    
    
	void LateUpdate () 
	{
		_Interval 	= (Time.deltaTime + _Interval) * 0.5f;
		_fpsNow 	= 1.0f/_Interval;
		
		_fpsAccumelated += Time.deltaTime; // _Interval;
		_fpsAccumCount++;
		
		if(Time.realtimeSinceStartup > _updateFrequency)
		{
			if(_fpsNow > _fpsMax) _fpsMax = _fpsNow;
			if(_fpsNow < _fpsMin) _fpsMin = _fpsNow;
		}
		
		if(Time.realtimeSinceStartup > _nextTime)
		{
			_nextTime 		= Time.realtimeSinceStartup + _updateFrequency;					
			_fpsAvg   		= 1.0f/(_fpsAccumelated/_fpsAccumCount);			
			_displayString 	= string.Format("Now: {0:F2}  Min: {1:F2}  Max: {2:F2}  Avg: {3:F2}  Unity: {4:F2}", _fpsNow, _fpsMin, _fpsMax, _fpsAvg, 1.0f/Time.smoothDeltaTime );
			
			_fpsAccumelated = 0;
			_fpsAccumCount 	= 0;
			
			/*
			guiText.text	= _displayString;
			
			if(_fpsNow < 30)
				guiText.material.color = Color.yellow;
			else
				if(_fpsNow < 10)
					guiText.material.color = Color.red;
				else
					guiText.material.color = Color.green;				
			*/
		}		
	}
	
	/*
	void OnMouseDown()
	{
		// print("OnMouseDown");
		Reset();
	//	_showFPS = !_showFPS;
	//	guiText.enabled = _showFPS;		
	}*/
	
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(1)) Reset();
	}


	void Reset()
	{
		_fpsMin 		= 65535.0f;
		_fpsMax 		= 0.0f;
		_fpsNow 		= 60.0f;
		_fpsAvg 		= 60.0f;
		_fpsAccumelated = 0.0f;
		_fpsAccumCount 	= 0;
		_Interval 		= 0;	
		_displayString 	= "Resetting fps ...";
	//	guiText.text 	= _displayString;
		_nextTime 		= Time.realtimeSinceStartup + _updateFrequency;
	}
}

