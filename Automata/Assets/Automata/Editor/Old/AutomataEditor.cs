using Automata.Core.Types;
using Automata.Core.Types.Interfaces;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class AutomataEditor : EditorWindow<AutomataEditor>
    {
        internal TreeBlueprint CurrentTree;

        private TreeView _TreeView;
        private InspectorView _InspectorView;
        private AssetView _AssetView;
        private Label _CurrentlyEditingLabel;
        private AutomataEditorSettings _Settings;

        /*
         *  ARCHIVED CODE
         *  PURPOSE: BLACKBOARD VISUAL EDITOR
         *  ARCHIVED BY: JAMES GREENSILL
         */
        // private BlackboardView _BlackboardView;

        [MenuItem("Automata/Editor")]
        public static void OpenWindow()
        {
            Instance.titleContent = new GUIContent("Automata Editor");
            Instance.LoadSettings();
        }

        [OnOpenAsset]
        public static bool OpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is TreeBlueprint)
            {
                OpenWindow();
                return true;
            }
            return false;
        }

        public void CreateGUI()
        {
            if (_Settings == null)
            {
                LoadSettings();
            }

            VisualElement root = rootVisualElement;

            // Set the editor settings.
            var visualTree = _Settings.AutomataUxml;
            root.styleSheets.Add(_Settings.AutomataUss);

            visualTree.CloneTree(root);

            _TreeView = root.Q<TreeView>();
            _AssetView = root.Q<AssetView>();
            _InspectorView = root.Q<InspectorView>();
            _CurrentlyEditingLabel = root.Q<Label>("currently-editing");

            /*
             *  ARCHIVED CODE
             *  PURPOSE: BLACKBOARD VISUAL EDITOR
             *  ARCHIVED BY: JAMES GREENSILL
             */
            // _BlackboardView = root.Q<BlackboardView>();

            _TreeView.Initialize();
            _AssetView.Initialize();

            // Ping editor to update.
            OnSelectionChange();
        }

        internal void ChangeTree(TreeBlueprint tree)
        {
            CurrentTree = tree;
            _TreeView.PopulateTree(CurrentTree);
            _CurrentlyEditingLabel.text = $"Currently Editing: {CurrentTree.name}";
        }

        internal void OnNodeSelectionChange(NodeView nodeView)
        {
            _InspectorView.ChangeView(nodeView);
        }

        private void OnSelectionChange()
        {
            EditorApplication.delayCall += () =>
            {
                TreeBlueprint tree = Selection.activeObject as TreeBlueprint;
                if (tree == null)
                {
                    if (Selection.activeGameObject != null)
                    {
                        IActivator activator = Selection.activeGameObject.GetComponent<IActivator>();
                        if (activator != null)
                        {
                            tree = activator.Tree.Blueprint;
                        }
                    }
                }

                if (tree != null)
                {
                    ChangeTree(tree);
                    _AssetView.OnSelectionChanged();
                }
            };
        }

        private void OnEnable()
        {
            // Ensure it is removed before adding it again. (werid unity issue).
            EditorApplication.playModeStateChanged -= _OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += _OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= _OnPlayModeStateChanged;
        }

        private void _OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;

                case PlayModeStateChange.ExitingEditMode:
                    _InspectorView?.ClearView();
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    _InspectorView?.ClearView();

                    break;
            }
        }

        private void LoadSettings()
        {
            if (!AutomataEditorSettings.LoadSettings(out _Settings))
            {
                Instance.Close();
            }
        }
    }
}