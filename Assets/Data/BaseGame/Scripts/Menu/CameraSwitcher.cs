using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public bool StartFromUI;
    public Camera MainCamera;
    public Camera GUICamera;

    private AudioListener Listener;
    
    void Start()
    {
        if (!MainCamera)
            MainCamera = Camera.main;

        if (!GUICamera)
            GUICamera = GetComponentInChildren<Camera>() ?? GetComponentInParent<Camera>();
        
        if (GUICamera)
            Listener = GUICamera.GetComponent<AudioListener>();
        
        if (StartFromUI)
            SwitchToGUI();
        else
            SwitchToGame();
    }
    
    public void SwitchToGUI()
    {
        MainCamera.enabled = false;
        Listener.enabled = true;
    }

    public void SwitchToGame()
    {
        Listener.enabled = false;
        MainCamera.enabled = true;
    }
}
