using System;
using System.Collections.Generic;
using Automata.Core.Types.Interfaces;
using Automata.Core.Utility.Extensions;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Automata.Core.Types
{
    [CreateAssetMenu(fileName = "TreeBlueprint", menuName = "Automata/TreeBlueprint")]
    public class TreeBlueprint : ScriptableObject, ITree<NodeBlueprint>
    {
        public NodeBlueprint Root;
        public List<NodeBlueprint> Nodes = new List<NodeBlueprint>();

#if UNITY_EDITOR

        public NodeBlueprint CreateNode(Type type, Vector2 graphPosition)
        {
            NodeBlueprint nodeBlueprint = NodeBlueprint.CreateFromType(type);
            if (nodeBlueprint != null)
            {
                nodeBlueprint.GraphPosition = graphPosition;
                Nodes.Add(nodeBlueprint);

                AssetDatabase.AddObjectToAsset(nodeBlueprint, this);
                AssetDatabase.SaveAssets();
            }

            return nodeBlueprint;
        }

        public bool DeleteNode(NodeBlueprint nodeBlueprint)
        {
            if (Nodes.Remove(nodeBlueprint))
            {
                AssetDatabase.RemoveObjectFromAsset(nodeBlueprint);
                AssetDatabase.SaveAssets();
                return true;
            }

            return false;
        }

        public bool CreateRoot()
        {
            NodeBlueprint nodeBlueprint = CreateNode(typeof(EntryPoint), Vector2.zero);
            if (nodeBlueprint != null)
            {
                Root = nodeBlueprint;
                return true;
            }

            return false;
        }

        public RuntimeTree CreateRuntimeTree()
        {
            if (Root != null)
            {
                RuntimeTree runtimeTree = new RuntimeTree();
                runtimeTree.Root = runtimeTree.CreateNode(Root.NodeType);

                CreateNodeRecursive(Root, runtimeTree.Root, ref runtimeTree);
            }

            return null;
        }

        public static void CreateNodeRecursive(NodeBlueprint nodeBlueprint, RuntimeNode parent, ref RuntimeTree runtimeTree)
        {
            var children = nodeBlueprint.GetChildren();
            foreach (var child in children)
            {
                RuntimeNode runtimeNode = runtimeTree.CreateNode(child.Base.NodeType);
                parent.AddChild(runtimeNode);
                CreateNodeRecursive(child.Base, runtimeNode, ref runtimeTree);
            }
        }

        public static void Traverse(NodeBlueprint node, Action<NodeBlueprint> visitor)
        {
            if (node != null)
            {
                node.Traverse((nodee) => { });

                visitor?.Invoke(node);
                INode<NodeBlueprint>[] children = node.GetChildren();
                foreach (INode<NodeBlueprint> child in children)
                {
                    Traverse(child.Base, visitor);
                }
            }
        }

#endif

        public void AddChild(INode<NodeBlueprint> parentNode, INode<NodeBlueprint> childNode) => parentNode.AddChild(childNode);

        public void RemoveChild(INode<NodeBlueprint> parentNode, INode<NodeBlueprint> childNode) => parentNode.RemoveChild(childNode);

        public INode<NodeBlueprint>[] GetChildren(INode<NodeBlueprint> node) => node.GetChildren();
    }
}