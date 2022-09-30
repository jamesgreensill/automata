using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Automata.Core.Utility.Extensions
{
    public static class ScriptableObjectEx
    {
#if UNITY_EDITOR

        public static List<T> LoadAssets<T>() where T : UnityEngine.Object
        {
            Type type = typeof(T);
            string[] assetIds = AssetDatabase.FindAssets($"t:{type.Namespace}.{type.Name}");
            var assets = new List<T>(assetIds.Length);
            assets.AddRange(assetIds.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>));
            return assets;
        }

#endif
    }
}