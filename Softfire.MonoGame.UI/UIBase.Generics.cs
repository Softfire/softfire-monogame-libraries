using System.Collections.Generic;
using System.Linq;

namespace Softfire.MonoGame.UI
{
    public partial class UIBase
    {
        /// <summary>
        /// Get Next Valid Item Id.
        /// </summary>
        /// <returns>Returns a valid item id as an int.</returns>
        internal static int GetNextValidItemId<T>(IList<T> list) where T : IUIIdentifier
        {
            var nextId = 1;
            while (list.Any(item => item.Id == nextId))
            {
                nextId++;
            }

            return nextId;
        }

        /// <summary>
        /// Get Item By Id.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="id">The id of the requested item.</param>
        /// <returns>Returns an object of Type T.</returns>
        internal static T GetItemById<T>(IList<T> list, int id) where T : IUIIdentifier
        {
            return list.FirstOrDefault(item => item.Id == id);
        }

        /// <summary>
        /// Get Item By Name.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="name">The name of the requested item.</param>
        /// <returns>Returns an object of Type T.</returns>
        internal static T GetItemByName<T>(IList<T> list, string name) where T : IUIIdentifier
        {
            return list.FirstOrDefault(item => item.Name == name);
        }

        /// <summary>
        /// Remove Item By Id.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="id">The id of the requested item.</param>
        /// <returns>Returns a boolean indicating whether the item was removed.</returns>
        internal static bool RemoveItemById<T>(IList<T> list, int id) where T : IUIIdentifier
        {
            var result = false;
            var itemToRemove = GetItemById(list, id);

            if (itemToRemove != null)
            {
                result = list.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Remove Item By Name.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="name">The name of the requested item.</param>
        /// <returns>Returns a boolean indicating whether the item was removed.</returns>
        internal static bool RemoveItemByName<T>(IList<T> list, string name) where T : IUIIdentifier
        {
            var result = false;
            var itemToRemove = GetItemByName(list, name);

            if (itemToRemove != null)
            {
                result = list.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Increase Item Order Number.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="itemId">The id of the item to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the item's Order Number was increased.</returns>
        internal static bool IncreaseItemOrderNumber<T>(IList<T> list, int itemId) where T : IUIIdentifier
        {
            var result = false;
            T itemToMove;

            if ((itemToMove = GetItemById(list, itemId)) != null)
            {
                T switchingItem;

                if ((switchingItem = list.SingleOrDefault(oneGroupUp => oneGroupUp.OrderNumber == itemToMove.OrderNumber + 1)) != null)
                {
                    switchingItem.OrderNumber--;
                    itemToMove.OrderNumber++;
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Increase Item Order Number.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="itemName">The name of the item to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the item's Order Number was increased.</returns>
        internal static bool IncreaseItemOrderNumber<T>(IList<T> list, string itemName) where T : IUIIdentifier
        {
            var result = false;
            T itemToMove;

            if ((itemToMove = GetItemByName(list, itemName)) != null)
            {
                T switchingItem;

                if ((switchingItem = list.SingleOrDefault(oneItemUp => oneItemUp.OrderNumber == itemToMove.OrderNumber + 1)) != null)
                {
                    switchingItem.OrderNumber--;
                    itemToMove.OrderNumber++;
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Decrease Item Order Number.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="itemId">The id of the item to retrieve. Intaken as an int.</param>
        /// <returns>Returns a boolean indicating whether the group's Order Number was decreased.</returns>
        internal static bool DecreaseItemOrderNumber<T>(IList<T> list, int itemId) where T : IUIIdentifier
        {
            var result = false;
            T itemToMove;

            if ((itemToMove = GetItemById(list, itemId)) != null)
            {
                T switchingGroup;

                if ((switchingGroup = list.SingleOrDefault(oneItemDown => oneItemDown.OrderNumber == itemToMove.OrderNumber - 1)) != null)
                {
                    switchingGroup.OrderNumber++;
                    itemToMove.OrderNumber--;
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Decrease Item Order Number.
        /// </summary>
        /// <typeparam name="T">Type of IUIdentifier.</typeparam>
        /// <param name="list">The list to check against.</param>
        /// <param name="itemName">The name of the item to retrieve. Intaken as a string.</param>
        /// <returns>Returns a boolean indicating whether the group's Order Number was decreased.</returns>
        internal static bool DecreaseItemOrderNumber<T>(IList<T> list, string itemName) where T : IUIIdentifier
        {
            var result = false;
            T itemToMove;

            if ((itemToMove = GetItemByName(list, itemName)) != null)
            {
                T switchingGroup;

                if ((switchingGroup = list.SingleOrDefault(oneItemDown => oneItemDown.OrderNumber == itemToMove.OrderNumber - 1)) != null)
                {
                    switchingGroup.OrderNumber++;
                    itemToMove.OrderNumber--;
                    result = true;
                }
            }

            return result;
        }
    }
}