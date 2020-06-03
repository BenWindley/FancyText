using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FModUtility
{
    public static void PlayOneShot(string soundRef)
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(soundRef);
        audioInstance.start();
        audioInstance.release();
    }

    public static void PlayOneShot(string soundRef, string paramName, float paramValue)
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(soundRef);
        audioInstance.setParameterByName(paramName, paramValue);
        audioInstance.start();
        audioInstance.release();
    }

    public static void PlayOneShot(string soundRef, Vector3 position)
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(soundRef);
        audioInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        audioInstance.start();
        audioInstance.release();
    }

    public static void PlayOneShot(string soundRef, Vector3 position, string paramName, float paramValue)
    {
        EventInstance audioInstance = RuntimeManager.CreateInstance(soundRef);
        audioInstance.setParameterByName(paramName, paramValue);
        audioInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        audioInstance.start();
        audioInstance.release();
    }
}
