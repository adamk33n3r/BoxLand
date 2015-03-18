using UnityEngine;
using System.Collections;
using System;

public class Utils {
    private Utils() {
    }
    public static TimeSpan timeFunction(Action func) {
        DateTime before = DateTime.Now;
        func.Invoke();
        return DateTime.Now - before;
    }
}
