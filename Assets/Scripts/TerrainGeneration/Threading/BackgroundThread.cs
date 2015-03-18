using System.ComponentModel;
using System;
using UnityEngine;

public abstract class BackgroundThread {

    private bool done = false;
    private object handle = new object();
    private System.Threading.Thread thread = null;

    public object data;
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

    public BackgroundThread(object data = null) {
        this.data = data;
    }

    public virtual BackgroundThread Start() {
        this.thread = new System.Threading.Thread(Run);
        this.thread.Start();
        return this;
    }

    public virtual void Abort() {
        this.thread.Abort();
    }

    protected abstract void ThreadFunction();
    protected abstract void OnFinished();

    private void Run() {
        ThreadFunction();
        this.isDone = true;
    }

    public virtual bool Update() {
        if (this.isDone) {
            OnFinished();
            return true;
        }
        return false;
    }
}
