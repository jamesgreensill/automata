using Automata.Core.Types;
using Automata.Core.Types.Interfaces;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class AutomataEditor : EditorWindow<AutomataEditor>
    {
        public Action<TreeBlueprint, TreeBlueprint> OnTreeChanged;

        public TreeView TreeView;
        public InspectorView InspectorView;
        public AssetView AssetView;
        public Label CurrentlyEditingLabel;

        public TreeBlueprint CurrentTree;
        public bool DoReload;

        public static AutomataEditorSettings Settings
        {
            get
            {
                if (Instance._Settings == null)
                {
                    if (!AutomataEditorSettings.LoadSettings(out Instance._Settings))
                    {
                        Instance.Close();
                    }
                }

                return Instance._Settings;
            }
        }

        private AutomataEditorSettings _Settings;

        [MenuItem("Automata/Editor")]
        public static void OpenWindow()
        {
            Instance.titleContent = new GUIContent("Automata Editor");
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

        internal void ChangeTree(TreeBlueprint tree)
        {
            if (tree == null)
            {
                return;
            }
            TreeBlueprint oldTree = CurrentTree;
            CurrentTree = tree;
            OnTreeChanged?.Invoke(oldTree, CurrentTree);

            CurrentlyEditingLabel.text = $"Currently Editing: {CurrentTree.name}";
        }

        protected override void OnEditorCreate()
        {
            VisualElement root = rootVisualElement;

            // Set the editor settings.
            var visualTree = Settings.EditorSettings.AutomataUxml;
            visualTree.CloneTree(root);
            root.styleSheets.Add(Settings.EditorSettings.AutomataUss);

            TreeView = root.Q<TreeView>();
            AssetView = root.Q<AssetView>();
            InspectorView = root.Q<InspectorView>();
            CurrentlyEditingLabel = root.Q<Label>("currently-editing");

            TreeView.Initialize();
            AssetView.Initialize();
            InspectorView.Initialize();

            Instance.DoReload = true;
            Reload();
            Instance.DoReload = false;

            // Ping editor to update.
            OnSelectionChange();
        }

        protected override void OnEditorDestroy()
        {
            InspectorView?.Deinitialize();
        }

        protected override void OnEditorEnable()
        {
            // bug: Weird Unity issue with Enable & Disable. Have to Deregister & Then Register.
            OnEnteredEditMode -= OnSelectionChange;
            OnEnteredPlayMode -= OnSelectionChange;
            OnEnteredEditMode += OnSelectionChange;
            OnEnteredPlayMode += OnSelectionChange;
        }

        protected override void OnEditorDisable()
        {
            OnEnteredEditMode -= OnSelectionChange;
            OnEnteredPlayMode -= OnSelectionChange;
        }

        protected override void OnEditorSelectionChange()
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
                            tree = activator.RuntimeTree.Blueprint;
                        }
                    }
                }

                if (tree != null)
                {
                    ChangeTree(tree);
                }
            };
        }

        private void Reload()
        {
            SaveTree();
            ChangeTree(CurrentTree);
        }

        private void SaveTree() => TreeView.Save();
    }
}