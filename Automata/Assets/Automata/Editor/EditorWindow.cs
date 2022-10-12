using UnityEditor;

namespace Automata.Editor
{
    public class EditorWindow<T> : EditorWindow where T : EditorWindow
    {
        public static T Instance => s_Instance ??= GetWindow<T>();
        private static T s_Instance;

        public System.Action OnEnteredPlayMode;
        public System.Action OnEnteredEditMode;
        public System.Action OnExitingEditMode;
        public System.Action OnExitingPlayMode;

        public void OnEnable()
        {
            OnEditorEnable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        public void OnDisable()
        {
            OnEditorDisable();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        public void OnDestroy()
        {
            OnEditorDestroy();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            s_Instance = null;
        }

        // ReSharper disable once InconsistentNaming
        public void CreateGUI() => OnEditorCreate();

        public void OnSelectionChange() => OnEditorSelectionChange();

        protected virtual void OnEditorEnable()
        {/*void*/}

        protected virtual void OnEditorDisable()
        {/*void*/}

        protected virtual void OnEditorDestroy()
        {/*void*/}

        protected virtual void OnEditorCreate()
        {/*void*/}

        protected virtual void OnEditorSelectionChange()
        {/*void*/}

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnEnteredEditMode?.Invoke();
                    break;

                case PlayModeStateChange.EnteredPlayMode:
                    OnEnteredPlayMode?.Invoke();
                    break;

                case PlayModeStateChange.ExitingEditMode:
                    OnExitingEditMode?.Invoke();
                    break;

                case PlayModeStateChange.ExitingPlayMode:
                    OnExitingPlayMode?.Invoke();
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException(nameof(change), change, null);
            }
        }
    }
}