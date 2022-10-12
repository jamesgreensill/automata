using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
        { }

        private UnityEditor.Editor _Editor;

        public void Initialize()
        {
            AutomataEditor.Instance.OnExitingEditMode += ClearView;
            AutomataEditor.Instance.OnExitingPlayMode += ClearView;
        }

        public void Deinitialize()
        {
            AutomataEditor.Instance.OnExitingEditMode -= ClearView;
            AutomataEditor.Instance.OnExitingPlayMode -= ClearView;
        }

        public void ChangeView(NodeView nodeView)
        {
            // Clear the current View.
            Clear();

            // Destroy the Previous Editor Instance.
            UnityEngine.Object.DestroyImmediate(_Editor);

            // Create a a new Editor Instance.
            _Editor = UnityEditor.Editor.CreateEditor(nodeView.Node);

            // Use the scriptable object editor GUI as the inspector.
            var container = new IMGUIContainer(() => { if (_Editor != null) _Editor.OnInspectorGUI(); });

            // Add the inspector as a child of this view.
            Add(container);
        }

        public void ClearView()
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_Editor);
        }
    }
}