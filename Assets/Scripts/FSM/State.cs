using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    public virtual void EnterState(T context){}
    public virtual State<T> UpdateState(T context){return null;}
}
