using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode
{
    public ActionNode()
    {
        name = GetType().ToString();
    }
    public string name;
    public abstract void Action();
}
public class OpenApp : ActionNode
{
    public string packageName = "";
    public override void Action()
    {
        launchApp();
    }

    public void launchApp()
    {
        var fail = false;
        string bundleId = packageName; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch 
        {
            fail = true;
        }

        if (fail||launchIntent==null)
        { //open app in store
            Application.OpenURL($"https://play.google.com/store/apps/details?id={packageName}");
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }
}

public class FuncOne : ActionNode
{
    public override void Action()
    {
        Debug.Log("1");
    }
}

public class FuncTwo : ActionNode
{
    public override void Action()
    {
        Debug.Log("2");
    }
}

public class FuncThree : ActionNode
{
    public override void Action()
    {
        Debug.Log("3");
    }
}