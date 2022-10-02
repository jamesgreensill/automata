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
        /// RuntimeTree this Activator will evalulate.
        /// </summary>
        public RuntimeTree RuntimeTree { get; set; }

        /// <summary>
        /// Before the RuntimeTree is evaluated.
        /// </summary>
        public Action OnPreActivated { get; set; }

        /// <summary>
        /// After the RuntimeTree is evaluated.
        /// </summary>
        public Action<RuntimeNode.State> OnActivated { get; set; }

        /// <summary>
        /// Ping this Activator to Evaluate the RuntimeTree.
        /// </summary>
        /// <returns>RuntimeTree Evaluation</returns>
        public RuntimeNode.State Activate();
    }
}