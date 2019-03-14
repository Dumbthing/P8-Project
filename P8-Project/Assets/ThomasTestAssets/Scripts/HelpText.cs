using UnityEngine;

public class HelpText : MonoBehaviour 
{
	void Start () 
	{
		GetComponent<GUIText>().text = "Left-Click & drag to rotate (orbit) viewpoint around scene origin.\nRight-Click & drag up/down to zoom in/out.";
	}
	

}
