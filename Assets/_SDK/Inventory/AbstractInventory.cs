using System.Collections.Generic;
using Assets._SDK.Inventory.Interfaces;
using UnityEngine;

namespace _SDK.Inventory
{
    public abstract class AbstractInventory<T> where T : IGameItem
    {
    }
}