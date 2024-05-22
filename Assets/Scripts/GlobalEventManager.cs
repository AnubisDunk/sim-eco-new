using System;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static UnityEvent OnHerbivoreKilled = new();
    public static UnityEvent OnCarnivoreKilled = new();
    public static UnityEvent OnHerbivoreBorn = new();
    public static UnityEvent OnCarnivoreBorn = new();

    public static UnityEvent OnTime = new();
    public static UnityEvent OnFood = new();
    public static UnityEvent OnPause = new();

    public static void SendHerbivoreKilled()
    {
        OnHerbivoreKilled.Invoke();
    }
    public static void SendCarnivoreKilled()
    {
        OnCarnivoreKilled.Invoke();
    }
    public static void SendHerbivoreBorn()
    {
        OnHerbivoreBorn.Invoke();
    }
    public static void SendCarnivoreBorn()
    {
        OnCarnivoreBorn.Invoke();
    }
    public static void SendFood()
    {
        OnFood.Invoke();
    }
     public static void SendTime()
    {
        OnTime.Invoke();
    }
    public static void SendPause()
    {
        OnPause.Invoke();
    }

}
