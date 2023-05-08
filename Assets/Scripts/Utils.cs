using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shove any static helper functions here
public class Utils
{
    public static Vector3[] Vec2ArrToVec3Arr(Vector2[] arr)
    {
        Vector3[] outArr = new Vector3[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            outArr[i] = (Vector3)arr[i];
        }
        return outArr;
    }

    public static Vector2[] Vec3ArrToVec2Arr(Vector3[] arr)
    {
        Vector2[] outArr = new Vector2[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            outArr[i] = arr[i];
        }
        return outArr;
    }

    public static Vector3 GetWorldPoint(Vector3 point, GameObject source)
    { 
        return source.transform.localToWorldMatrix.MultiplyPoint3x4(point);
    }
    public static Vector3 GetLocalPoint(Vector3 point, GameObject source)
    {
        return source.transform.worldToLocalMatrix.MultiplyPoint3x4(point);
    }

    public static Vector2 GetWorldPoint(Vector2 point, GameObject source)
    {
        return source.transform.localToWorldMatrix.MultiplyPoint3x4(point);
    }
    public static Vector2 GetLocalPoint(Vector2 point, GameObject source)
    {
        return source.transform.worldToLocalMatrix.MultiplyPoint3x4(point);
    }

    public static Vector3[] GetWorldPoints(Vector3[] points, GameObject source)
    {
        Vector3[] outArr = new Vector3[points.Length];
        for (int i = 0; i < outArr.Length; i++)
        {
            outArr[i] = GetWorldPoint(points[i], source);
        }
        return outArr;
    }

    public static Vector2[] GetWorldPoints(Vector2[] points, GameObject source)
    {
        Vector2[] outArr = new Vector2[points.Length];
        for (int i = 0; i < outArr.Length; i++)
        {
            outArr[i] = GetWorldPoint(points[i], source);
        }
        return outArr;
    }

    public static Vector2 PerpendicularClockwise(Vector2 vector2)
    {
        return new Vector2(vector2.y, -vector2.x);
    }

    public static Vector2 PerpendicularCounterClockwise(Vector2 vector2)
    {
        return new Vector2(-vector2.y, vector2.x);
    }
}
