﻿namespace BeautyGo.Domain.Patterns.Singletons;

public class Singleton<T> : BaseSingleton
{
    private static T instance;

    public static T Instance
    {
        get => instance;
        set
        {
            instance = value;
            AllSingletons[typeof(T)] = value;
        }
    }
}
