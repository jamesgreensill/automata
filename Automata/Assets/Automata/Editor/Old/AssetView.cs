using System.Collections.Generic;
using Automata.Core.Types;
using Automata.Core.Utility.Extensions;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class AssetView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AssetView, UxmlTraits>
        { }

        private ScrollView _ScrollView;
        private AutomataEditor _AutomataEditor;

        private List<AssetItemView> _ItemAssets = new List<AssetItemView>();

        public void Initialize()
        {
            _ScrollView = this.Q<ScrollView>("asset-view");
            _SetupAssetView();
        }

        public void OnSelectionChanged()
        {
            _UpdateSelection();
        }

        private void _SetupAssetView()
        {
            var assets = ScriptableObjectEx.LoadAssets<TreeBlueprint>();
            foreach (var asset in assets)
            {
                AssetItemView assetItemView = new AssetItemView(asset, (tree) =>
                    {
                        _AutomataEditor.ChangeTree(tree);
                        _UpdateSelection();
                    }
                );
                assetItemView.Button.AddToClassList("automata-asset-button");
                assetItemView.Label.AddToClassList("automata-asset-label");

                _ItemAssets.Add(assetItemView);
                _ScrollView.Add(assetItemView);
            }
        }

        private void _UpdateSelection()
        {
            foreach (var itemAsset in _ItemAssets)
            {
                if (itemAsset.Tree == _AutomataEditor.CurrentTree)
                {
                    itemAsset.Button.AddToClassList("automata-asset-button-selected");
                    itemAsset.Label.AddToClassList("automata-asset-label-selected");
                    continue;
                }
                itemAsset.Button.RemoveFromClassList("automata-asset-button-selected");
                itemAsset.Button.RemoveFromClassList("automata-asset-label-selected");
            }
        }
    }
}