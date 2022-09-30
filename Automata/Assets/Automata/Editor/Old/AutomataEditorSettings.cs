using System.Collections.Generic;
using Automata.Core.Utility.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Automata/Settings")]
    internal class AutomataEditorSettings : ScriptableObject
    {
#pragma warning disable 0414

        [SerializeField] public VisualTreeAsset AutomataUxml;
        [SerializeField] public StyleSheet AutomataUss;

#pragma warning restore 0414

        public static bool LoadSettings(out AutomataEditorSettings settings)
        {
            List<AutomataEditorSettings> settingsList = ScriptableObjectEx.LoadAssets<AutomataEditorSettings>();
            if (settingsList.Count > 0)
            {
                settings = settingsList[0];
                if (settingsList.Count != 1)
                {
                    Debug.LogWarning($"There are more than one instance of {typeof(AutomataEditorSettings)}, using the first one at: {AssetDatabase.GetAssetPath(settings)}.");
                }
                return true;
            }
            Debug.LogError($"Please create a {typeof(AutomataEditorSettings)} asset, before opening Automata.");
            settings = null;
            return false;
        }
    }
}