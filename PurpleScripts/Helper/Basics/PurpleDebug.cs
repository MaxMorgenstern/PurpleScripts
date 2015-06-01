using System;
using UnityEngine;

public class PurpleDebug : MonoBehaviour
{
    public static int CurrentDebugLevel = PurpleConfig.Build.DebugLevel;
    public static int DefaultDebugLevel = 2;

    public static void SetDefaultLevel()
    {
        CurrentDebugLevel = PurpleConfig.Build.DebugLevel;
        DefaultDebugLevel = 2;
    }
    
    public static void SetLevel(int Level)
    {
        CurrentDebugLevel = Level;
    }

    public static void SetDefaultDebugCallLevel(int Level)
    {
        DefaultDebugLevel = Level;
    }


    // Debug Calls /////////////////////////

    // Log
    public static void Log(object message)
    {
        Log(message, DefaultDebugLevel);
    }
    public static void Log(object message, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.Log(message);
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        Log(message, context, DefaultDebugLevel);
    }
    public static void Log(object message, UnityEngine.Object context, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.Log(message);
    }


    // LogError
    public static void LogError(object message)
    {
        LogError(message, DefaultDebugLevel);
    }
    public static void LogError(object message, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogError(message);
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        LogError(message, context, DefaultDebugLevel);
    }
    public static void LogError(object message, UnityEngine.Object context, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogError(message, context);
    }


    // LogException
    public static void LogException(Exception exception)
    {
        LogException(exception, DefaultDebugLevel);
    }
    public static void LogException(Exception exception, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogException(exception);
    }

    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        LogException(exception, context, DefaultDebugLevel);
    }
    public static void LogException(Exception exception, UnityEngine.Object context, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogException(exception, context);
    }


    // LogError
    public static void LogWarning(object message)
    {
        LogWarning(message, DefaultDebugLevel);
    }
    public static void LogWarning(object message, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogWarning(message);
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        LogWarning(message, context, DefaultDebugLevel);
    }
    public static void LogWarning(object message, UnityEngine.Object context, int level)
    {
        if (Debug.isDebugBuild && level <= CurrentDebugLevel)
            Debug.LogWarning(message, context);
    }
}
