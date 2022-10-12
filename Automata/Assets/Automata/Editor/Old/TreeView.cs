using Automata.Core.Types;
using Automata.Core.Types.Interfaces;
using Automata.Core.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Automata.Editor
{
    public class TreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<TreeView, GraphView.UxmlTraits>
        {
        }

        /// <summary>
        /// GraphView grid background.
        /// </summary>
        public GridBackground Grid = new GridBackground
        {
            visible = AutomataEditor.Settings.EditorSettings.GridVisible
        };

        /// <summary>
        /// Content Dragger
        /// </summary>
        public ContentDragger ContentDragger = new ContentDragger
        {
            panSpeed = AutomataEditor.Settings.EditorSettings.PanSpeed,
            clampToParentEdges = AutomataEditor.Settings.EditorSettings.ClampToParentEdges
        };

        /// <summary>
        /// Selection Dragger
        /// </summary>
        public SelectionDragger SelectionDragger = new SelectionDragger
        {
            panSpeed = AutomataEditor.Settings.EditorSettings.PanSpeed,
            clampToParentEdges = AutomataEditor.Settings.EditorSettings.ClampToParentEdges
        };

        /// <summary>
        /// Content Zoomer
        /// </summary>
        public ContentZoomer ContentZoomer = new ContentZoomer
        {
            minScale = AutomataEditor.Settings.EditorSettings.MinScale
        };

        /// <summary>
        /// RectangleSelector
        /// </summary>
        public RectangleSelector RectangleSelector = new RectangleSelector();

        /// <summary>
        /// SearchWindow
        /// </summary>
        public TreeViewSearchWindow SearchWindow;

        /// <summary>
        /// Current Tree Instance
        /// </summary>
        private TreeBlueprint _CurrentTree;

        /// <summary>
        /// Initialize the TreeView
        /// </summary>
        internal void Initialize()
        {
            // Add Manipulators
            this.AddManipulator(ContentZoomer);
            this.AddManipulator(ContentDragger);
            this.AddManipulator(SelectionDragger);
            this.AddManipulator(RectangleSelector);

            // Add Search Menu
            this._AddSearchMenu();

            // Add Callbacks
            this._AddCallbacks();

            // Add grid
            this.Insert(0, Grid);
        }

        /// <summary>
        /// Activated when the Tree has changed.
        /// </summary>
        /// <param name="oldTree">Previous Tree</param>
        /// <param name="newTree">Next Tree</param>
        public void OnTreeChanged(TreeBlueprint oldTree, TreeBlueprint newTree)
        {
            SetTree(newTree);
        }

        /// <summary>
        /// Populate the view with a new Tree.
        /// </summary>
        /// <param name="tree"></param>
        public void SetTree(TreeBlueprint tree)
        {
            _DeleteElements();
            _CreateTree(tree);
        }

        /// <summary>
        /// Save current Tree.
        /// </summary>
        public void Save()
        {
            if (_CurrentTree != null)
            {
                _CurrentTree.Root.Traverse((node) =>
                {
                    EditorUtility.SetDirty(node.Base);
                });
                EditorUtility.SetDirty(_CurrentTree);
                AssetDatabase.SaveAssets();
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) => ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort != startPort).ToList();

        public NodeBlueprint CreateNode(string typeString, Vector2 graphMousePosition, System.Action<NodeBlueprint> creationDelegate = null)
        {
            NodeBlueprint node = _CurrentTree.CreateNode(typeString, graphMousePosition);
            if (node != null)
            {
                creationDelegate?.Invoke(node);
                _CreateNodeView(node);
            }

            return node;
        }

        private void _AddSearchMenu()
        {
            SearchWindow = ScriptableObject.CreateInstance<TreeViewSearchWindow>().Create(this);
            nodeCreationRequest = context =>
                UnityEditor.Experimental.GraphView.SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), SearchWindow);
        }

        private void _AddCallbacks()
        {
            AutomataEditor.Instance.OnTreeChanged += OnTreeChanged;
        }

        private GraphViewChange _OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // Remove elements from the Graph
            if (graphViewChange.elementsToRemove != null)
                foreach (GraphElement element in graphViewChange.elementsToRemove)
                {
                    switch (element)
                    {
                        case NodeView view:
                            if (view.Node.IsDeletable(view))
                            {
                                _CurrentTree.DeleteNode(view.Node);
                            }
                            break;

                        case Edge edge:
                            if (edge.input.node is NodeView childView && edge.output.node is NodeView parentView)
                            {
                                _CurrentTree.RemoveChild(parentView.Node, childView.Node);
                            }
                            break;
                    }
                }

            if (graphViewChange.edgesToCreate != null)
                // Add all the edges
                foreach (Edge element in graphViewChange.edgesToCreate)
                {
                    if (element.input.node is NodeView childView && element.output.node is NodeView parentView)
                    {
                        _CurrentTree.AddChild(parentView.Node, childView.Node);
                    }
                }

            // Sort the tree.
            nodes.ForEach(node =>
            {
                if (node is NodeView view)
                {
                    view.SortChildren(NodeView.SortByVerticalPosition);
                }
            });

            return graphViewChange;
        }

        private void _DeleteElements()
        {
            // Stop the editor from listening to changes on the graph
            graphViewChanged -= _OnGraphViewChanged;
            // Remove all elements from the graph
            DeleteElements(graphElements.ToList());
            // Allow the editor to listen to changes on the graph
            graphViewChanged += _OnGraphViewChanged;
        }

        private void _CreateEdge(NodeBlueprint node)
        {
            if (node == null)
            {
                return;
            }

            var children = node.GetChildren();

            foreach (INode<NodeBlueprint> child in children)
            {
                NodeView parentView = GetNodeByGuid(node.Guid) as NodeView;
                NodeView childView = GetNodeByGuid(child.Base.Guid) as NodeView;

                if (parentView != null && childView != null)
                {
                    foreach (Port inputPort in childView.InputPorts)
                    {
                        if (inputPort.portType == typeof(NodeBlueprint))
                        {
                            AddElement(parentView.OutputPort.ConnectTo(inputPort));
                        }
                    }
                }
            }
        }

        private void _CreateNodeView(NodeBlueprint node)
        {
            if (node != null)
            {
                var nodeView = new NodeView(node)
                {
                    OnNodeSelected = AutomataEditor.Instance.InspectorView.ChangeView,
                };
                nodeView.capabilities = node.GetCapabilities(nodeView);
                AddElement(nodeView);
            }
        }

        private void _CreateTree(TreeBlueprint tree)
        {
            if (tree == null)
                return;

            _CurrentTree = tree;
            _CurrentTree.Nodes.ForEach(_CreateNodeView);
            _CurrentTree.Nodes.ForEach(_CreateEdge);

            if (_CurrentTree.Root == null)
            {
                if (_CurrentTree.CreateRoot())
                {
                    EditorUtility.SetDirty(_CurrentTree);
                    AssetDatabase.SaveAssets();
                }
            }

            nodes.ForEach(node =>
            {
                if (node is NodeView view)
                {
                    view.SortChildren(NodeView.SortByVerticalPosition);
                }
            });
        }
    }
}