using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    int previousSetting = -1;

    public void SetResolution(int setting)
    {
        // return if the setting is the same as before
        if (setting == previousSetting) return;

        // initialize variables
        previousSetting = setting;
        bool isFullScreen = false;
        int width = 0;
        int height = 0;

        // configure resolution variables
        switch (setting)
        {
            case 0: // Full Screen
                isFullScreen = true;
                width = Screen.width;
                height = Screen.height;
                break;

            case 1: // Windowed (1920 x 1080)
                isFullScreen = false;
                width = 1920;
                height = 1080;
                break;

            case 2: // Windowed (800 x 600)
                isFullScreen = false;
                width = 1600;
                height = 900;
                break;

            case 3: // Windowed (800 x 600)
                isFullScreen = false;
                width = 1280;
                height = 720;
                break;
        }

        // set screen size
        Screen.SetResolution(width, height, isFullScreen);
    }
}
