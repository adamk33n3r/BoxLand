using System.ComponentModel;
using System;

public class BackgroundThread {

    BackgroundWorker worker;
    private bool done = false;
    private object handle = new object();
    public bool isDone {
        get {
            bool tmp;
            lock (handle) {
                tmp = done;
            }
            return tmp;
        }
        set {
            lock (handle) {
                done = value;
            }
        }
    }

    public BackgroundThread(DoWorkEventHandler workerFunction, RunWorkerCompletedEventHandler workerCallback = null) {
        this.worker = new BackgroundWorker();
        this.worker.DoWork += workerFunction;
        if (workerCallback)
            this.worker.RunWorkerCompleted += workerCallback;
    }

    public void Start(object argument) {
        this.worker.RunWorkerAsync(argument);
    }
}
