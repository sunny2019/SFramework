﻿using System.Reflection;
using UnityEngine;


namespace Ex
{
    public static class ExTransform
    {
        /// <summary>
        /// 对象池加载重置Z轴
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Transform ExUIResetZ(this Transform trans)
        {
            Vector3 v3 = trans.GetComponent<RectTransform>().localPosition;
            v3.z = 0;
            trans.GetComponent<RectTransform>().localPosition = v3;
            return trans;
        }


        public static Transform ExSetAsFirstSibling(this Transform trans)
        {
            trans.SetAsFirstSibling();
            return trans;
        }

        public static Transform ExSetAsLastSibling(this Transform trans)
        {
            trans.SetAsLastSibling();
            return trans;
        }

        /// <summary>
        /// 对象池加载重置XYZ轴
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static Transform ExUIResetXYZ(this Transform trans, Vector3 localPosition)
        {
            trans.GetComponent<RectTransform>().localPosition = localPosition;
            trans.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            return trans;
        }

        public static Transform ExUIResetSizeDelta(this Transform trans, Vector2 sizeDelta)
        {
            trans.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            return trans;
        }
        
        
        public static Vector3 GetInspectorRotationValue(Transform transform)
        {
            System.Type transformType = transform.GetType();
            PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
            object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
            MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles",BindingFlags.Instance | BindingFlags.NonPublic);
            object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] {m_OldRotationOrder});
            return (Vector3) value;
        }
         
        
        
        
        /// <summary>
        /// 返回物体正前方的位置
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector3 Forward(this Transform trans, float distance)
        {
            return trans.position+ trans.TransformDirection(Vector3.forward) * distance;
        }
        
    }
}
