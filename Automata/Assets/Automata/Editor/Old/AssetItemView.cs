using Automata.Core.Types;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class AssetItemView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AssetItemView, UxmlTraits>
        { };

        public System.Action<TreeBlueprint> OnClick;

        public Label Label;
        public Button Button;
        public TreeBlueprint Tree;

        public AssetItemView() : this(null, null)
        { }

        public AssetItemView(TreeBlueprint tree, System.Action<TreeBlueprint> action)
        {
            Tree = tree;
            OnClick = action;
            Label = new Label
            {
                text = tree != null ? tree.name : string.Empty
            };
            Button = new Button(() => OnClick?.Invoke(Tree));
            Add(Button);
            Add(Label);
        }
    }
}