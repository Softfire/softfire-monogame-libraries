using System.Threading.Tasks;

namespace Softfire.MonoGame.SM.V2
{
    public abstract partial class State
    {
        #region Effects

        /// <summary>
        /// Load Transition.
        /// Adds the provided Transition to the State's Loaded Transitions Dictionary and modifying the Order Number to be equal to the number of current transitions, if Order Number is found to be 0.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Transition to run. Intaken as a <see cref="string"/>.</param>
        /// <param name="transition">The Transition to be loaded.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Transition was added.</returns>
        public bool LoadTransition(string identifier, Transition transition)
        {
            var result = false;

            if (!LoadedTransitionExists(identifier))
            {
                if (transition.OrderNumber == 0)
                {
                    transition.OrderNumber = ActiveTransitions.Count + 1;
                }

                LoadedTransitions.Add(identifier, transition);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Get Loaded Transition.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Transition to retrieve. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a Transition, if found, otherwise null.</returns>
        public Transition GetLoadedTransition(string identifier)
        {
            Transition result = null;

            if (LoadedTransitionExists(identifier))
            {
                result = LoadedTransitions[identifier];
            }

            return result;
        }

        /// <summary>
        /// Determines whether the loaded transition exists, by name.
        /// </summary>
        /// <param name="identifier">The unique identifier used to search for the Transition. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Transition has been loaded.</returns>
        public bool LoadedTransitionExists(string identifier)
        {
            return LoadedTransitions.ContainsKey(identifier);
        }

        /// <summary>
        /// Remove Transition.
        /// Removes the Transition from Loaded Transitions using the provided identifier.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Transition to remove. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the Transition was removed.</returns>
        private bool RemoveTransition(string identifier)
        {
            var result = false;

            if (LoadedTransitionExists(identifier))
            {
                result = LoadedTransitions.Remove(identifier);
            }

            return result;
        }

        /// <summary>
        /// Activate Transition.
        /// Called to activate a loaded Transition.
        /// </summary>
        /// <param name="identifier">The unique identifier used to select the Transition to activate. Intaken as a <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether the transition was activated.</returns>
        public bool ActivateLoadedTransition(string identifier)
        {
            var result = false;

            if (LoadedTransitionExists(identifier))
            {
                ActiveTransitions.Add(LoadedTransitions[identifier]);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Run Transitions.
        /// Called to run all loaded Transitions by ascending Order Number.
        /// </summary>
        /// <param name="inSequentialOrder">Indicates whether the Transitions will run sequentially or all at once. Intaken as a <see cref="bool"/>. Default is true.</param>
        /// <returns>Returns a <see cref="bool"/> indicating whether all of the loaded transitions completed.</returns>
        public async Task<bool> RunActiveTransitions(bool inSequentialOrder = true)
        {
            var result = false;

            if (inSequentialOrder)
            {
                if (ActiveTransitions.Count > 0)
                {
                    if (await ActiveTransitions[0].Run())
                    {
                        ActiveTransitions.RemoveAt(0);
                        IsTransitioning = false;
                    }
                    else
                    {
                        IsTransitioning = true;
                    }
                }
            }
            else
            {
                for (var index = 0; index < ActiveTransitions.Count; index++)
                {
                    var activeTransition = ActiveTransitions[index];

                    if (await activeTransition.Run())
                    {
                        ActiveTransitions.Remove(activeTransition);
                    }
                    else
                    {
                        IsTransitioning = true;
                    }
                }
            }

            if (ActiveTransitions.Count == 0)
            {
                IsTransitioning = false;
                ActivateTransitions = false;
                result = true;
            }

            return result;
        }

        #endregion
    }
}
