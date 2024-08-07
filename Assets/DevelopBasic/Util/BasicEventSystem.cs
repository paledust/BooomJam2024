using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
    public static event Action E_BeforeUnloadScene;
    public static void Call_BeforeUnloadScene(){E_BeforeUnloadScene?.Invoke();}
    public static event Action E_AfterLoadScene;
    public static void Call_AfterLoadScene(){E_AfterLoadScene?.Invoke();}
#region Interaction Event
    public static event Action E_OnPlayerInteract;
    public static void Call_OnPlayerInteract()=>E_OnPlayerInteract?.Invoke();
#endregion

#region UI Event
    public static event Action<Vector2> E_UI_OnCursorPosChange;
    public static void Call_UI_OnCursorPosChange(Vector2 newPos)=>E_UI_OnCursorPosChange?.Invoke(newPos);
#endregion
}