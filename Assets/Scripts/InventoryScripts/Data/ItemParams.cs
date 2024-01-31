using System;
using System.Collections.Generic;
using Assets.DeadCell.Scripts.Enums;

namespace Assets.DeadCell.Scripts.Data
{
    /// <summary>
    /// Represents generic item params (common for all items).
    /// </summary>
    [Serializable]
    public class ItemParams
    {
        public ItemType Type;
        public List<ItemTag> Tags = new List<ItemTag>();
        public List<Property> Properties = new List<Property>();
        public string detailDescription;
    }
}