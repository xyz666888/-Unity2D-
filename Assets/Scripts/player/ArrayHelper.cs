using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数组帮助类
/// 协助其他类完成数组的一些操作
/// </summary> 
///  <author>Xiao_Yanzhe</author>
/// <see cref="https://gitee.com/Xiao_pluto"/>
public static class ArrayHelper
{
    /// <summary>
    /// 查找满足条件(相等）的单个元素(有多个时返回第一个）
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="array">数组</param>
    /// <param name="condition">比较方法（委托）</param>
    /// <returns>返回目标对象</returns>
    public static T Find<T>(this T[] array, Func<T, bool> condition)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
            {
                return array[i];
            }
        }
        return default(T);
    }

    /// <summary>
    /// 查找满足条件(相等）的多个元素
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <param name="array">数组</param>
    /// <param name="condition">比较方法（委托）</param>
    /// <returns>返回目标对象</returns>
    public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
            {
                list.Add(array[i]);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// 查找数组中满足条件的最大值
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较依据的数据类型</typeparam>
    /// <param name="array">数组</param>
    /// <param name="condition">比较依据方法</param>
    /// <returns></returns>
    public static T GetMax<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        // 若数组为空则返回默认（null)
        if (array.Length == 0) return default(T);

        T maxT = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]).CompareTo(condition(maxT)) > 0)
                maxT = array[i];
        }
        return maxT;
    }

    /// <summary>
    /// 查找数组中满足条件的最小值
    /// </summary>
    /// <typeparam name="T">数组类型</typeparam>
    /// <typeparam name="Q">比较依据的数据类型</typeparam>
    /// <param name="array">数组</param>
    /// <param name="condition">比较依据方法</param>
    /// <returns></returns>
    public static T GetMin<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        // 若数组为空则返回默认（null)
        if (array.Length == 0) return default(T);

        T minT = array[0];
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]).CompareTo(condition(minT)) < 0)
                minT = array[i];
        }
        return minT;
    }

    /// <summary>
    /// 数组升序排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="Q">比较依据</typeparam>
    /// <param name="array">待排序数组</param>
    /// <param name="condition">排序依据方法</param>
    public static void Sort_ascending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        // 冒泡排序
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (condition(array[j]).CompareTo(condition(array[i])) > 0)
                {
                    T temp = array[i];
                    array[j] = array[j];
                    array[i] = array[j];
                }
            }
        }
    }

    /// <summary>
    /// 数组降序排序
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="Q">比较依据</typeparam>
    /// <param name="array">待排序数组</param>
    /// <param name="condition">排序依据方法</param>
    public static void Sort_descending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
    {
        // 冒泡排序
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (condition(array[j]).CompareTo(condition(array[i])) < 0)
                {
                    T temp = array[i];
                    array[j] = array[j];
                    array[i] = array[j];
                }
            }
        }
    }

    /// <summary>
    /// 在每个T中选出Q 返回Q[] ---例如在所有敌人物体中获得所有敌人的动画脚本
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    /// <typeparam name="Q">需要获取的类型</typeparam>
    /// <param name="array">原数组</param>
    /// <param name="condition">排序依据方法</param>
    /// <returns>返回需要获取的类型</returns>
    public static Q[] Select<T, Q>(this T[] array, Func<T, Q> condition)
    {
        Q[] res = new Q[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            res[i] = condition(array[i]);
        }
        return res;
    }
}
