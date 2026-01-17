using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public static class CommonExtensions {
    
    private static MonoBehaviour _entry;

    private static MonoBehaviour AppEntry {
        get {
            if(_entry == null) 
                _entry = Object.FindObjectOfType<AppEntry>(true);
            return _entry;
        }
    }
    
    private static readonly System.Random _random = new ();

    public static Coroutine PlayCoroutine(this IEnumerator routine) {
        if (routine == null) return null;
        
        return AppEntry.StartCoroutine(routine);
    }

    public static void StopCoroutine(this Coroutine routine) {
        if (routine == null) return;
        
        AppEntry.StopCoroutine(routine);
    }

    public static IEnumerable<T> Shuffle<T> (this IEnumerable<T> array, int shuffleCount = 1) {
        var buffer = array.ToArray();

        for (var i = 0; i < shuffleCount; i++) {
            
            var n = buffer.Length;
            
            while (n > 1) 
            {
                var k = _random.Next(n--);
                (buffer[n], buffer[k]) = (buffer[k], buffer[n]);
            }
        }

        return buffer;
    }
    
}
