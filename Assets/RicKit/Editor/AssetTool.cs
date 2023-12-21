using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RicKit.EditorTools
{
    public static class AssetTool
    {
        /// <summary>
        /// 找到texture对应的sprite；用于在编辑器中获取sprite引用
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public static bool TryGetSprite(Texture2D tex, out Sprite sprite)
        {
            sprite = null;
            var path = AssetDatabase.GetAssetPath(tex);
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            return sprite;
        }
    }
}

