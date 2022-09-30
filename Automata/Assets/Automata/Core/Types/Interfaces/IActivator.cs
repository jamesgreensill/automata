using System;
using UnityEngine;

namespace Automata.Core.Types.Interfaces
{
    public interface IActivator
    {
        /// <summary>
        /// Scene Object this Activator is linked to.
        /// </summary>
        public GameObject SceneObject { get; }

        /// <summary>
        /// Tree this Activator will evalulate.
        /// </summary>
        public Tree Tree { get; set; }

        /// <summary>
        /// Before the Tree is evaluated.
        /// </summary>
        public Action OnPreActivated { get; set; }

        /// <summary>
        /// After the Tree is evaluated.
        /// </summary>
        public Action<Node.State> OnActivated { get; set; }

        /// <summary>
        /// Ping this Activator to Evaluate the Tree.
        /// </summary>
        /// <returns>Tree Evaluation</returns>
        public Node.State Activate();
    }
}