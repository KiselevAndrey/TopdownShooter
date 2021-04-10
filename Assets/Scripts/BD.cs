using System;
using System.Collections.Generic;
using UnityEngine;

#region Helper Classes
public static class HelperList
{
    static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #region List<T>.Ind
    /// <summary>
    /// Определяет элемент от любого числа без IndexOutRange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T Ind<T>(this List<T> list, int index)
    {
        index = Math.Abs(index);
        return list[index % list.Count];
    }

    /// <summary>
    /// Определяет элемент от любого числа без IndexOutRange
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static T Ind<T>(this List<T> list, ref int index, bool shuffle = false)
    {
        if (shuffle && index >= list.Count) list.Shuffle();

        index = Math.Abs(index) % list.Count;
        return list[index];
    }
    #endregion
}

public static class HelperBool
{
    static System.Random rng = new System.Random();

    #region RandomBool
    public static bool RandomBool() => rng.Next(2) != 0;

    /// <summary>
    /// Шанс получить true в соотношение value к 100
    /// </summary>
    public static bool RandomBoolPercent(float value) => rng.Next(100) <= value;
    #endregion
}

public static class HelperVector
{
    /// <summary>
    /// возвращает новую позицию в требуемом радиусе
    /// </summary>
    public static Vector3 NewPointFromRange(Vector3 startPos, float range)
    {
        Vector3 temp = startPos;

        temp.x += (UnityEngine.Random.value - 0.5f) * range;
        temp.y += (UnityEngine.Random.value - 0.5f) * range;
        temp.z += (UnityEngine.Random.value - 0.5f) * range;

        return temp;
    }
}
#endregion


#region Strings Classes
public static class AxesNames
{
    public static string Horizontal = "Horizontal";
    public static string Vertical = "Vertical";
    public static string Fire1 = "Fire1";
}

public static class TagsNames
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Bonus = "Bonus";
    public const string Bullet = "Bullet";
    public const string Weapon = "Weapon";
}

public static class MixerGroup
{
    public static string MusicVolume = "Volume Music";
    public static string MasterVolume = "Volume Master";
    public static string EffectsVolume = "Volume Effects";
}

public static class AnimParam
{
    public static string Speed = "Speed";
    public static string Attack = "Attack";
    public static string Shot = "Shot";
    public static string Dead = "Dead";
}

public static class LayersNames
{
    public static string Human = "Human";
    public static string Zombie = "Zombie";
}
#endregion