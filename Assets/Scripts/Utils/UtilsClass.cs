using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Text;

//辅助工具集
public static class UtilsClass
{
    /// <summary>
    /// Use reflection to copy all properties from one object to another
    /// </summary>
    /// <param name="_object"></param>
    /// <returns></returns>
    public static object DeepCopy(object _object)
    {
        Type T = _object.GetType();
        object Obj = Activator.CreateInstance(T);

        PropertyInfo[] PI = T.GetProperties();
        for (int i = 0; i < PI.Length; i++)
        {
            PropertyInfo P = PI[i];
            if (P != null)
            {
                P.SetValue(Obj, P.GetValue(_object));
            }
        }
        return Obj;
    }

    //Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPostition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPostition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        UnityEngine.GameObject gameObject = new UnityEngine.GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    //Get Mouse Position in World with Z = 0f, used in 2D
    public static Vector3 GetMouseWorldPosition2D()
    {
        Vector3 vec = GetMouseWorldPosition3D(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPosition3D()
    {
        return GetMouseWorldPosition3D(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPosition3D(Camera worldCamera)
    {
        return GetMouseWorldPosition3D(Input.mousePosition, worldCamera);
    }


    public static Vector3 GetMouseWorldPosition3D(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    /// <summary>
    /// 将向量转化为角度
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    /// <summary>
    /// 将向量转化为角度
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static int GetAngleFromVectorInt(Vector3 direction)
    {
        int angle = Mathf.RoundToInt(GetAngleFromVectorFloat(direction));
        return angle;
    }

    /// <summary>
    /// 返回从shootPos到targetPos的单位向量
    /// </summary>
    /// <param name="startPos">起始位置</param>
    /// <param name="endPos">目标位置</param>
    /// <returns></returns>
    public static Vector3 GetPosToDirection(Vector3 startPos, Vector3 endPos)
    {
        return (endPos - startPos).normalized;
    }


    /// <summary>
    /// 获取animator下某个动画片段的时长
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static float GetAnimatorLength(Animator animator, string name)
    {
        float length = 0;
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        return length;
    }

    /// <summary>
    /// 控制某个动画的播放速度
    /// </summary>
    /// <param name="ani"></param>
    /// <param name="name"></param>
    /// <param name="speed"></param>
    public static void SetAnimationSpeed(Animation ani, string name, float speed)
    {
        if (ani == null)
        {
            return;
        }
        AnimationState state = ani[name];
        if (!state) state.speed = speed;
    }

    
    public static void Shuffle<T>(this IList<T> list, System.Random random)
    {
        int currentIndex = list.Count;
        while (currentIndex > 1)
        {
            currentIndex--;
            int randomIndex = random.Next(currentIndex + 1);

            // Swap
            T temp = list[randomIndex];
            list[randomIndex] = list[currentIndex];
            list[currentIndex] = temp;
        }
    }

    public static T RandomPickOne<T>(this IList<T> list, System.Random random)
    {
        if (list.Count == 0)
        {
            return default;
        }

        int randomIndex = random.Next(0, list.Count);
        return list[randomIndex];
    }

    public static T RandomPickOne<T>(this IReadOnlyList<T> list, System.Random random)
    {
        if (list.Count == 0)
        {
            return default;
        }

        int randomIndex = random.Next(0, list.Count);
        return list[randomIndex];
    }

    public static T RandomPickOne<T>(this IEnumerable<T> enumerable, System.Random random)
    {
        int count = enumerable.Count();
        if (count == 0)
        {
            return default;
        }

        int randomIndex = random.Next(0, count);
        var enumerator = enumerable.GetEnumerator();
        for (var i = 0; i < randomIndex + 1; i++)
        {
            enumerator.MoveNext();
        }

        return enumerator.Current;
    }

    public static string ToContentString<T>(this IList<T> list)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < list.Count; i++)
        {
            sb.Append(list[i].ToString());
            if (i != list.Count - 1)
            {
                sb.Append(",");
            }
        }

        return sb.ToString();
    }

    public static string ToContentString<T>(this IReadOnlyList<T> list)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < list.Count; i++)
        {
            sb.Append(list[i].ToString());
            if (i != list.Count - 1)
            {
                sb.Append(",");
            }
        }

        return sb.ToString();
    }

    public static string ToContentString<T>(this IEnumerable<T> enumerable)
    {
        var sb = new StringBuilder();

        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if (enumerator.MoveNext())
        {
            sb.Append(enumerator.Current.ToString());
            while (enumerator.MoveNext())
            {
                sb.Append(",");
                sb.Append(enumerator.Current.ToString());
            }
        }

        return sb.ToString();
    }

}
/// <summary>
/// Vector3 Utils
/// </summary>
public static class VectorHelperExtensions
{
    public static Vector3 Flatten(this Vector3 vector)
    {
        vector.y = 0.0f;
        return vector;
    }

    public static float Distance2D(this Vector3 from, in Vector3 to)
    {
        return Vector3.Distance(Flatten(from), Flatten(to));
    }

    public static Vector3 Direction2D(this Vector3 from, in Vector3 to)
    {
        return (Flatten(to) - Flatten(from)).normalized;
    }

    public static Vector3 RotateAboutUp(this Vector3 currentRot, in float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.up) * currentRot;
    }

}