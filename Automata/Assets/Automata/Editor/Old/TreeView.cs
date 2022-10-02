using Automata.Core.Types;
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
        private GridBackground _Grid;
        private ContentDragger _ContentDragger;
        private SelectionDragger _SelectionDragger;
        private RectangleSelector _RectangleSelector;
        private ContentZoomer _ContentZoomer;
        private TreeViewSearchWindow _SearchWindow;

        private TreeBlueprint _CurrentTree;
        private System.Action<NodeView> _OnNodeSelected;

        //private List<BlackboardValue> _ExposedValues = new List<BlackboardValue>();

        /*
         *  ARCHIVED CODE
         *  PURPOSE: BLACKBOARD VISUAL EDITOR
         *  ARCHIVED BY: JAMES GREENSILL
         */
        // private BlackboardView _Blackboard;

        public new class UxmlFactory : UxmlFactory<TreeView, GraphView.UxmlTraits>
        { }

        public TreeView()
        {
            _Grid = new GridBackground();
            _ContentDragger = new ContentDragger();
            _SelectionDragger = new SelectionDragger();
            _RectangleSelector = new RectangleSelector();
            _ContentZoomer = new ContentZoomer
            {
                minScale = 0.01f
            };
        }

        public void PopulateTree(TreeBlueprint tree)
        {
            _DeleteElements();
            _CreateTree(tree);

            /*
             *  ARCHIVED CODE
             *  PURPOSE: BLACKBOARD VISUAL EDITOR
             *  ARCHIVED BY: JAMES GREENSILL
             */
            // _CreateBlackboard(runtimeTree);
        }

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

        internal void Initialize()
        {
            this.Insert(0, _Grid);
            this.AddManipulator(_ContentZoomer);
            this.AddManipulator(_ContentDragger);
            this.AddManipulator(_SelectionDragger);
            this.AddManipulator(_RectangleSelector);
            this._AddSearchMenu();
            this._AddCallbacks();

            _LoadStyle("Assets/AI/BehaviourTreeV2/Editor/AutomataEditor.uss");
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
            _SortTree();
        }

        private void _CreateEdge(NodeBlueprint node)
        {
            // if (node == null)
            // {
            //     return;
            // }
            //
            // List<RuntimeNode> children = node.GetChildren();
            //
            // foreach (RuntimeNode child in children)
            // {
            //     NodeView parentView = _FindNodeView(node);
            //     NodeView childView = _FindNodeView(child);
            //
            //     if (parentView != null & childView != null)
            //     {
            //         Edge edge = parentView.OutputPortBlueprint.ConnectTo(childView.InputPortBlueprint);
            //         AddElement(edge);
            //     }
            // }
        }

        private NodeView _FindNodeView(NodeBlueprint node)
        {
            if (node != null)
            {
                return GetNodeByGuid(node.Guid) as NodeView;
            }

            return null;
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

        private void _AddCallbacks()
        {
            _OnNodeSelected = AutomataEditor.Instance.OnNodeSelectionChange;

            /*
             * ARCHIVED CODE
             * Purpose: Blackboard Visual Editor
             * Archived By: James Greensill.
             */

            // RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
            // RegisterCallback<DragPerformEvent>(OnDragPerformEvent);
        }

        private GraphViewChange _OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            _UpdateTree(graphViewChange.elementsToRemove);
            _UpdateTree(graphViewChange.edgesToCreate);
            _SortTree();
            return graphViewChange;
        }

        private void _SortTree()
        {
            nodes.ForEach(node =>
            {
                if (node is NodeView view)
                {
                    view.SortChildren(NodeView.SortByVerticalPosition);
                    // if (view.RuntimeNode is Composite composite)
                    // {
                    //     for (int i = 0; i < composite.Children.Count; i++)
                    //     {
                    //         var childView = _FindNodeView(composite.Children[i]);
                    //         if (childView != null)
                    //         {
                    //             childView.title = $"[{i}] - " + composite.Children[i].name;
                    //         }
                    //     }
                    //
                    //     view.title = view.RuntimeNode.name + $" - [{composite.Children.Count}]";
                    // }
                }
            });
        }

        private void _UpdateTree(List<GraphElement> elements)
        {
            if (elements == null)
                return;
            foreach (GraphElement element in elements)
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
        }

        private void _UpdateTree(List<Edge> elements)
        {
            if (elements == null)
                return;
            foreach (Edge element in elements)
            {
                if (element.input.node is NodeView childView && element.output.node is NodeView parentView)
                {
                    _CurrentTree.AddChild(parentView.Node, childView.Node);
                }
            }
        }

        private void _AddSearchMenu()
        {
            _SearchWindow = ScriptableObject.CreateInstance<TreeViewSearchWindow>();
            _SearchWindow.Configure(AutomataEditor.Instance, this, new[] { typeof(Action), typeof(Decorator), typeof(Composite), typeof(Predicate) }, new[] { typeof(EntryPoint) });
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _SearchWindow);
        }

        private void _LoadStyle(string stylePath)
        {
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(stylePath);
            if (styleSheet)
            {
                styleSheets.Add(styleSheet);
            }
        }

        private void _CreateNodeView(NodeBlueprint node)
        {
            if (node != null)
            {
                var nodeView = new NodeView(node)
                {
                    OnNodeSelected = _OnNodeSelected,
                };
                nodeView.capabilities = node.GetCapabilities(nodeView);
                AddElement(nodeView);
            }
        }

        /*
         *  ARCHIVED CODE
         *  PURPOSE: BLACKBOARD VISUAL EDITOR
         *  ARCHIVED BY: JAMES GREENSILL
         */

        // public void OnDragUpdatedEvent(DragUpdatedEvent e)
        // {
        //     if (DragAndDrop.GetGenericData("DragSelection") is List<ISelectable> iselection && (iselection.OfType<BlackboardField>().Count() >= 0))
        //     {
        //         DragAndDrop.visualMode = e.actionKey ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Move;
        //     }
        // }

        // public void OnDragPerformEvent(DragPerformEvent e)
        // {
        //     var select = DragAndDrop.GetGenericData("DragSelection") as List<ISelectable>;
        //     IEnumerable<BlackboardField> fields = select.OfType<BlackboardField>();
        //     foreach (var field in fields)
        //     {
        //         //var node = CreateNode(typeof(Data), e.mousePosition, node1 =>
        //         //{
        //         //    var no = node1 as Data;
        //         //    if (no != null)
        //         //    {
        //         //        no.Initialize(field.Value);
        //         //    }
        //         //});
        //     }
        // }

        // private void _CreateBlackboard(RuntimeTree runtimeTree)
        // {
        //    // if (_Blackboard != null)
        //    // {
        //    //     _Blackboard.Initialize(_EditorWindow);
        //    //     Add(_Blackboard);
        //    //     return;
        //    // }
        //    // runtimeTree.BlackboardContainer = runtimeTree.CreateBlackboard();
        //    //
        //    // _Blackboard = new BlackboardView(this);
        //    // _Blackboard.Initialize(_EditorWindow);
        //    // //_Blackboard.PopulateBlackboard(runtimeTree.BlackboardContainer);
        //    //
        //    // Add(_Blackboard);
        // }
    }
}