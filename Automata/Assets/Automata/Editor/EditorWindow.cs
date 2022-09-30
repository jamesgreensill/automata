using UnityEditor;

namespace Automata.Editor
{
    public class EditorWindow<T> : EditorWindow where T : EditorWindow
    {
        public static T Instance => s_Instance ??= GetWindow<T>();
        private static T s_Instance;

        private void OnDestroy()
        {
            s_Instance = null;
        }
    }
}