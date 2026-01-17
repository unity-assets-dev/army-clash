using System;
using System.Collections;
using UnityEngine;

public static class Utils {

    public static void Delay(float time, Action onComplete) {
        Start().PlayCoroutine();
        
        return;
        IEnumerator Start() {
            yield return new WaitForSeconds(time);
            
            onComplete?.Invoke();
        }
    }
    
}