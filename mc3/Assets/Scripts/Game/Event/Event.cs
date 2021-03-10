using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event {
    protected float duration;
    protected float startTime;
    protected float endTime;

    public bool isFinished{ get { return Time.time >= endTime; } }

    
    public void StartEvent() {
        startTime = Time.time;
        endTime = startTime + duration;
        OnStart();
    }

    public bool RunStep() {
        if(isFinished) {
            OnFinish();
            return false;
        } else {
            Step((Time.time - startTime) / duration);
            return true;
        }
    }

    public abstract void OnStart();
    public abstract void Step(float progress);
    public abstract void OnFinish();
}
