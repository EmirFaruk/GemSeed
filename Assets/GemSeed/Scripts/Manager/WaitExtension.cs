using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class WaitExtension
{   
    private static int _loopCount;
    private static bool _completed;

    public static void Wait(this MonoBehaviour mono, float delay, UnityAction action)
    {
        mono.StartCoroutine(ExecuteAction(delay, action));
    }

    private static IEnumerator ExecuteAction(float delay, UnityAction aciton)
    {
        yield return new WaitForSecondsRealtime(delay);
        aciton.Invoke();
        yield break;
    }


    public static void Call(this MonoBehaviour mono, float remainder, UnityAction action)
    {
        mono.StartCoroutine(CallAction(remainder));

        if (_completed == false)
        {
            action.Invoke();
        }        
    }

    private static IEnumerator CallAction(float remainder)
    {
        yield return new WaitForSecondsRealtime(remainder);

        _completed = true;
        
        yield break;
    }

    public static void Func(this MonoBehaviour mono, UnityAction action, int loopCount)
    {
        if (loopCount <= _loopCount)
        {
            return;
        }
        else
        {
            action.Invoke();

            _loopCount++;            
        }        
    }
}
