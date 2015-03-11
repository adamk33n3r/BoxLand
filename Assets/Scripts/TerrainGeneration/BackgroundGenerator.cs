using UnityEngine;
using System.ComponentModel;

public class BackgroundGenerator {

//    public object data;
    private BackgroundThread thread;

    public BackgroundGenerator(object data = null) {
//        this.data = data;
        thread = new BackgroundThread(workerFunc, callbackFunc);
        thread.Start(data);
    }

    static void workerFunc(object sender, DoWorkEventArgs e) {
        Debug.Log("Starting BGGen: Got " + e.Argument);
        e.Result = e.Argument;
    }
    
    static void callbackFunc(object sender, RunWorkerCompletedEventArgs e) {
        Debug.Log("BGGen is done: Result is " + e.Result);
        thread.isDone = true;
    }
}
