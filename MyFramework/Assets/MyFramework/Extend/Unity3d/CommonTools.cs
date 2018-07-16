using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyFramework
{
    public static class CommonTools
    {
        #region Object工具
        public static T Copy<T>(T RealObject)
        {

            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }
        #endregion

        #region Unity3d
        #region Unity3d Object 扩展
        /// <summary>
        /// 查找添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        static public T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }

        /// <summary>
        /// 创建游戏对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_Object"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject(UnityEngine.Object _Object, Transform Parent)
        {
            GameObject obj = GameObject.Instantiate(_Object) as GameObject;
            if (obj != null)
            {
                obj.SetParent(Parent);
            }
            return obj;
        }


        /// <summary>
        /// 設置父物体对象
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Parent"></param>
        public static void SetParent(this GameObject Target, Transform Parent)
        {
            Target.transform.parent = Parent;
            Target.transform.localPosition = Vector3.zero;
            Target.transform.localScale = Vector3.one;
            Target.transform.localRotation = Quaternion.identity;
        }



        /// <summary>
        /// 设置对象以及子对象层
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="layer"></param>
        public static void SetLayer(this GameObject Target, LayerMask layer)
        {
            Target.layer = layer;
            for (int i = 0; i < Target.transform.childCount; i++)
            {
                Target.transform.GetChild(i).gameObject.SetLayer(layer);
            }
        }

        /// <summary>
        /// 设置对象trans
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Parent"></param>
        public static void SetTrans(this GameObject Target, Transform Parent)
        {
            Target.transform.position = Parent.position;
            Target.transform.localScale = Vector3.one;
            Target.transform.rotation = Parent.rotation;
        }

        /// <summary>
        /// 创建子对象
        /// </summary>
        /// <param name="Parent"></param>
        public static GameObject CreateChild(this GameObject Parent, string name,params Type[] components)
        {
            GameObject child = new GameObject(name, components);
            child.SetParent(Parent.transform);
            return child;
        }

        /// <summary>
        /// 找到子节点的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Target"></param>
        /// <param name="Childpath"></param>
        /// <returns></returns>
        public static T OnSubmit<T>(this GameObject Target, string Childpath) where T : Component
        {
            Transform obj = Target.transform.Find(Childpath);
            if (obj != null)
            {
                return obj.GetComponent<T>();
            }
            return null;
        }
        #endregion

        #region 游戏工具

        /// <summary>
        /// UI移动动画
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="To"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static IEnumerator MoveTo(this RectTransform Target, Vector2 To, float time)
        {
            float begintime = Time.time;
            float aftertime = 0;
            float progress = 0;
            float Lerp = 0;
            Vector2 From = Target.anchoredPosition;
            while (aftertime < time)
            {
                aftertime = Time.time - begintime;
                progress = aftertime / time;

                Target.anchoredPosition = Vector2.Lerp(From, To, Lerp);
                yield return 3;
            }
            Target.anchoredPosition = To;
        }


        /// <summary>
        /// 对象运动过程
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="To"></param>
        /// <param name="_Curve"></param>
        /// <param name="time"></param>
        /// <param name="CallBack"></param>
        /// <returns></returns>
        public static IEnumerator MoveTo(this GameObject Target, Vector3 To, AnimationCurve _Curve, float time, Action CallBack = null)
        {
            float begintime = Time.time;
            float aftertime = 0;
            float progress = 0;
            float Lerp = 0;
            Vector3 From = Target.transform.position;
            while (aftertime < time)
            {
                aftertime = Time.time - begintime;
                progress = aftertime / time;
                if (_Curve != null)  
                    Lerp = _Curve.Evaluate(progress);
                else
                    Lerp = progress;
                Target.transform.position = Vector3.Lerp(From, To, Lerp);
                yield return 3;
            }
            Target.transform.position = To;
            if (CallBack != null)
            {
                CallBack();
            }
        }

        /// <summary>
        /// 对象运动过程
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="To"></param>
        /// <param name="_Curve"></param>
        /// <param name="time"></param>
        /// <param name="CallBack"></param>
        /// <returns></returns>
        public static IEnumerator RotaTo(this GameObject Target, Quaternion To, AnimationCurve _Curve, float time, Action CallBack = null)
        {
            float begintime = Time.time;
            float aftertime = 0;
            float progress = 0;
            float Lerp = 0;
            Quaternion From = Target.transform.rotation;
            while (aftertime < time)
            {
                aftertime = Time.time - begintime;
                progress = aftertime / time;
                if (_Curve != null)
                    Lerp = _Curve.Evaluate(progress);
                else
                    Lerp = progress;
                Target.transform.rotation = Quaternion.Lerp(From, To, Lerp);
                yield return 3;
            }
            Target.transform.rotation = To;
            if (CallBack != null)
            {
                CallBack();
            }
        }


        /// <summary>
        /// 对象运动过程
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="To"></param>
        /// <param name="_Curve"></param>
        /// <param name="time"></param>
        /// <param name="CallBack"></param>
        /// <returns></returns>
        public static IEnumerator SizeTo(this GameObject Target, Vector3 To, AnimationCurve _Curve, float time, Action CallBack = null)
        {
            float begintime = Time.time;
            float aftertime = 0;
            float progress = 0;
            float Lerp = 0;
            Vector3 From = Target.transform.localScale;
            while (aftertime < time)
            {
                aftertime = Time.time - begintime;
                progress = aftertime / time;
                if (_Curve != null)
                    Lerp = _Curve.Evaluate(progress);
                else
                    Lerp = progress;
                Target.transform.localScale = Vector3.Lerp(From, To, Lerp);
                yield return 3;
            }
            Target.transform.localScale = To;
            if (CallBack != null)
            {
                CallBack();
            }
        }

        /// <summary>
        /// 对象运动过程
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="To"></param>
        /// <param name="_Curve"></param>
        /// <param name="time"></param>
        /// <param name="CallBack"></param>
        /// <returns></returns>
        public static IEnumerator LookTo(GameObject Target, Vector3 To, AnimationCurve _Curve, float time, Action CallBack = null)
        {
            float begintime = Time.time;
            float aftertime = 0;
            float progress = 0;
            float Lerp = 0;
            Quaternion From = Target.transform.rotation;
            while (aftertime < time)
            {
                aftertime = Time.time - begintime;
                progress = aftertime / time;
                if (_Curve != null)
                    Lerp = _Curve.Evaluate(progress);
                else
                    Lerp = progress;

                Vector3 diff = To - Target.transform.position;
                Quaternion q = Quaternion.FromToRotation(Vector3.forward, diff);
                Vector3 newUp = q * Vector3.up;
                Vector3 n = q * Vector3.forward;
                Vector3 worldUp = Vector3.up;
                float dirDot = Vector3.Dot(n, worldUp);
                Vector3 vProj = worldUp - n * dirDot;    //worldUp在xy平面上的投影量  
                vProj.Normalize();
                float dotproj = Vector3.Dot(vProj, newUp);
                float theta = Mathf.Acos(dotproj) * Mathf.Rad2Deg;
                Quaternion qNew = Quaternion.AngleAxis(theta, n);
                Quaternion qall = qNew * q;

                Target.transform.rotation = Quaternion.Lerp(From, qall, Lerp);
                yield return 3;
            }
            Target.transform.position = To;
            if (CallBack != null)
            {
                CallBack();
            }
        }

        #endregion
        #endregion

        #region List
        public static void ApplyAllItem<T>(this List<T> sourceList, Action<T> action)
        {
            for (int i = 0; i < sourceList.Count; i++)
            {
                action(sourceList[i]);
            }
        }
        #endregion
    }
}
