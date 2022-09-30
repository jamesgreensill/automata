using Automata.Core.Types.Attributes;
using Automata.Core.Utility.Extensions;
using System;
using System.Collections.Generic;
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

        public void Configure(EditorWindow window, TreeView treeView, Type[] contextTypes, Type[] blackListedTypes = null)
        {
            _TreeView = treeView;
            _EditorWindow = window;
            _ContextTypes = contextTypes;
            _BlacklistedTypes = blackListedTypes;

            _IndentationIcon = new Texture2D(1, 1);
            _IndentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _IndentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0)
            };

            var types = TypeEx.GetDerivedTypes(typeof(Node));
            foreach (var type in types)
            {
                _AddTypeDataToSearchTree(type, ref tree);
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePosition = _EditorWindow.rootVisualElement.ChangeCoordinatesTo(
                _EditorWindow.rootVisualElement.parent, context.screenMousePosition - _EditorWindow.position.position);

            Vector2 graphMousePosition = _TreeView.contentViewContainer.WorldToLocal((mousePosition));

            //if (searchTreeEntry.userData as Node)
            //{
            //    Node node = _TreeView.CreateNode(searchTreeEntry.userData.GetType(), graphMousePosition);
            //    return node != null;
            //}

            return false;
        }

        private void _AddTypeDataToSearchTree(Type type, ref List<SearchTreeEntry> tree)
        {
            tree.Add(new SearchTreeGroupEntry(new GUIContent(type.Name), 1));

            var categoryAttributes = TypeEx.GetAttributes(type);
            foreach (var attribute in categoryAttributes)
            {
                CategoryAttribute categoryAttribute = (CategoryAttribute)attribute;

                if (categoryAttribute != null)
                {
                    var categoryDervivedType = TypeEx.GetDerivedTypes(type);
                    foreach (var categoryDerivedType in categoryDervivedType)
                    {
                        tree.Add(new SearchTreeEntry(new GUIContent(categoryAttribute.Name, _IndentationIcon)
                        )
                        {
                            level = 2,
                            userData = CreateInstance(categoryDerivedType)
                        });
                    }
                }
            }
        }
    }
}