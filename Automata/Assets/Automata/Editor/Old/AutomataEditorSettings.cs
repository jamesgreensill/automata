using Automata.Core.Types;
using Automata.Core.Utility.Extensions;

using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Automata/Settings")]
    public class AutomataEditorSettings : ScriptableObject
    {
        [Serializable]
        public class AssetSettings
        {
            public ResetableValue<string> AssetButtonSelectedId = new ResetableValue<string>("automata-asset-button-selected");
            public ResetableValue<string> AssetLabelSelectedId = new ResetableValue<string>("automata-asset-label-selected");
        }

        [Serializable]
        public class TreeSettings
        { }

        [Serializable]
        public class InspectorSettings
        { }

        [Serializable]
        public class Settings
        {
            [SerializeField] public VisualTreeAsset AutomataUxml;
            [SerializeField] public StyleSheet AutomataUss;

            [Header("Tree")]
            [SerializeField] public ResetableValue<Orientation> Orientation;

            [Header("Grid")]
            [SerializeField] public ResetableValue<bool> GridVisible;

            [Header("Content Size Fitter")]
            [SerializeField] public ResetableValue<float> MinScale = new ResetableValue<float>(0.1f);

            [Header("Content Dragger")]
            [SerializeField] public ResetableValue<Vector2> PanSpeed = new ResetableValue<Vector2>(Vector2.one);

            [SerializeField] public ResetableValue<bool> ClampToParentEdges = new ResetableValue<bool>(false);
        }

#pragma warning disable 0414

        public Settings EditorSettings;

        [Header("Internal, Do not modify unless you know what you are doing.")]
        public AssetSettings AssetViewSettings;

        public TreeSettings TreeViewSettings;
        public InspectorSettings InspectorViewSettings;

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