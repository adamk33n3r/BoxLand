using UnityEngine;
using System.Collections;

public class BackgroundSleeper : BackgroundThread {

    private BackgroundSleeper(object data) : base(data) {
    }

    public static BackgroundThread Go(object data) {
        return new BackgroundSleeper(data).Start();
    }
    
    protected override void ThreadFunction() {
        Debug.Log("Starting ThreadFunction: data = " + this.data);
        System.Threading.Thread.Sleep((int)this.data);
    }
    
    protected override void OnFinished() {
        Debug.Log("Finished ThreadFunction: data = " + this.data);
    }
}
