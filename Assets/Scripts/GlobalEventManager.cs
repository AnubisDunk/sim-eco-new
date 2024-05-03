using System;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static UnityEvent OnCreatureKilled = new();
    public static UnityEvent OnCreatureBorn = new();
    public static UnityEvent OnFood = new();
    public static UnityEvent OnPause = new();

    public static void SendCreatureKilled()
    {
        OnCreatureKilled.Invoke();
    }
    public static void SendCreatureBorn()
    {
        OnCreatureBorn.Invoke();
    }
    public static void SendFood()
    {
        OnFood.Invoke();
    }
    public static void SendPause()
    {
        OnPause.Invoke();
    }

}
