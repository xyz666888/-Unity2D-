using System;
using Assets.DeadCell.Scripts.Enums;
using Assets.DeadCell.Scripts.GameData;

namespace Assets.DeadCell.Scripts.Data
{
    /// <summary>
    /// Represents item object for storing with game profile (please note, that item params are stored separately in params database).
    /// </summary>
    [Serializable]
    public class Item
    {
        public ItemId Id;
        public int Count;

        public ItemParams Params => Items.Params[Id];

        public Item()
        {
        }

        public Item(ItemId id, int count)
        {
            Id = id;
            Count = count;
        }
    }
}