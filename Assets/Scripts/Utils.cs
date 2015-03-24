using UnityEngine;
using System.Collections;
using System;

public class Utils {
    public class Colors {
        public static Color mountainGreen = new Color(30 / 255f, 92 / 255f, 49 / 255f);
        public static Color green = new Color(0, 125 / 255f, 0);
        public static Color sand = new Color(248 / 255f, 224 / 255f, 99 / 255f);
        public static Color water = new Color(0, 0, 1);
    }
    private Utils() {
    }
    public static TimeSpan timeFunction(Action func) {
        DateTime before = DateTime.Now;
        func.Invoke();
        return DateTime.Now - before;
    }
}
