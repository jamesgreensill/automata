using Automata.Core.Types;
using Automata.Core.Utility.Extensions;

using System.Collections.Generic;

using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class AssetView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AssetView, UxmlTraits>
        { }

        public List<AssetItemView> ItemAssets = new List<AssetItemView>();
        private ScrollView _ScrollView;

        public void Initialize()
        {
            _ScrollView = this.Q<ScrollView>("asset-view");

            AutomataEditor.Instance.OnTreeChanged += OnTreeChanged;

            _SetupAssetView();
        }

        public void OnTreeChanged(TreeBlueprint oldTree, TreeBlueprint newTree)
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
                        AutomataEditor.Instance.ChangeTree(tree);
                        _UpdateSelection();
                    }
                );
                assetItemView.Button.AddToClassList("automata-asset-button");
                assetItemView.Label.AddToClassList("automata-asset-label");

                ItemAssets.Add(assetItemView);
                _ScrollView.Add(assetItemView);
            }
        }

        private void _UpdateSelection()
        {
            foreach (AssetItemView itemAsset in ItemAssets)
            {
                if (itemAsset.Tree == AutomataEditor.Instance.CurrentTree)
                {
                    itemAsset.Button.AddToClassList(AutomataEditor.Settings.AssetViewSettings.AssetButtonSelectedId);
                    itemAsset.Label.AddToClassList(AutomataEditor.Settings.AssetViewSettings.AssetLabelSelectedId);
                    continue;
                }
                itemAsset.Button.RemoveFromClassList(AutomataEditor.Settings.AssetViewSettings.AssetButtonSelectedId);
                itemAsset.Button.RemoveFromClassList(AutomataEditor.Settings.AssetViewSettings.AssetLabelSelectedId);
            }
        }
    }
}