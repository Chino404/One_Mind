using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    private Resolution[] _resolusions;

    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        _resolusions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolusions.Length; i++)
        {
            string option = _resolusions[i].width + "x" + _resolusions[i].height;
            if(!options.Contains(option))
            options.Add(option);

            if (_resolusions[i].width == Screen.currentResolution.width && _resolusions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }



        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(currentResolutionIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolusions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
