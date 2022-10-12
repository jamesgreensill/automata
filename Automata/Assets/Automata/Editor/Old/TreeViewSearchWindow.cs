using Automata.Core.Types;
using Automata.Core.Types.Attributes;
using Automata.Core.Utility.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class TreeViewSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private EditorWindow _EditorWindow;
        private TreeView _TreeView;
        private Texture2D _IndentationIcon;
        private Type[] _ContextTypes;
        private Type[] _BlacklistedTypes;

        public TreeViewSearchWindow Create(TreeView treeView)
        {
            _TreeView = treeView;
            _EditorWindow = AutomataEditor.Instance;

            _IndentationIcon = new Texture2D(1, 1);
            _IndentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _IndentationIcon.Apply();
            return this;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create RuntimeNode"), 0)
            };

            // Get every type derived from node that is abstract and has the Category attribute.
            // Add Type to RuntimeTree
            // For every attribute get every type inherited from the type that contains that attribute.
            // Add every type to search runtimeTree

            var types = TypeEx.GetDerivedTypes(typeof(Automata.Core.Types.RuntimeNode)).Where(t => t.IsAbstract);

            List<CategoryAttribute> categories = new List<CategoryAttribute>();
            foreach (var type in types)
            {
                var attributes = TypeEx.GetAttributes(type).Where(a => a is CategoryAttribute);
                categories.AddRange(attributes.Select(attribute => attribute as CategoryAttribute));
            }

            foreach (var category in categories)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent(category.Name), 1));

                var derivedTypes = TypeEx.GetDerivedTypes(category.Type).Where(t => !t.IsAbstract && t.HasDefaultConstructor());
                foreach (var derivedType in derivedTypes)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(derivedType.Name, _IndentationIcon)
                    )
                    {
                        level = 2,
                        userData = $"{derivedType.FullName}"
                    });
                }
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is string typeString)
            {
                Vector2 mousePosition = _EditorWindow.rootVisualElement.ChangeCoordinatesTo(
                    _EditorWindow.rootVisualElement.parent, context.screenMousePosition - _EditorWindow.position.position);

                Vector2 graphMousePosition = _TreeView.contentViewContainer.WorldToLocal((mousePosition));
                NodeBlueprint node = _TreeView.CreateNode(typeString, graphMousePosition);

                return node != null;
            }

            return false;
        }
    }
}