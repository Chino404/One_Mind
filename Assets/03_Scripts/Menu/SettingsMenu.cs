using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public List<string> resolutions;
    public TMP_Dropdown resolutionDropdown;
    private List<Resolution> _resolutions=new();
    private int _currentResolutionIndex;


    private void Start()
    {
        //_resolutions = Screen.resolutions;

        //resolutionDropdown.ClearOptions();

        //List<string> options = new();

        //int currentResolutionIndex = 0;
        //for (int i = 0; i < _resolutions.Length; i++)
        //{
        //    string option = _resolutions[i].width + "x" + _resolutions[i].height;
        //    if(!options.Contains(option))
        //    options.Add(option);

        //    if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
        //        currentResolutionIndex = i;
        //}




        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
        //SetResolution(currentResolutionIndex);

        resolutionDropdown.ClearOptions();
        List<string> validOptions = new();

        for (int i = 0; i < resolutions.Count; i++)
        {
            string[] dims = resolutions[i].Split('x');
            

            if (int.TryParse(dims[0], out int width) && int.TryParse(dims[1], out int height))
            {
                Resolution res = new Resolution { width = width, height = height };
                _resolutions.Add(res);
                validOptions.Add(resolutions[i]);

                //if (Screen.currentResolution.width == width && Screen.currentResolution.height == height)
                //    _currentResolutionIndex = i;
                if (width == Screen.currentResolution.width && height == Screen.currentResolution.height)
                    _currentResolutionIndex = i;

               
            }
        }

        resolutionDropdown.AddOptions(validOptions);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(_currentResolutionIndex);


    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
