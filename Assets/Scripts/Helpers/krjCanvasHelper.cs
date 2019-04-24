using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krjCanvasHelper : MonoBehaviour
{
    public Canvas unityCanvas;
    public GUISkin GeneralSkin;
    public GUISkin MainMenuSkin;
    public krjMainCircle mainCircle;
    public krjGUICanvas krjCanvas;
    
    private void OnGUI()
    {
        //GUI.skin = GeneralSkin;
        krjCanvas.draw();
    }

    // Use this for initialization
    void Start ()
    {
        krjCanvas = new krjGUICanvas(this);
        krjCanvas.GeneralSkin = GeneralSkin;
        krjCanvas.MainMenuSkin = MainMenuSkin;
        krjCanvas.init();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
