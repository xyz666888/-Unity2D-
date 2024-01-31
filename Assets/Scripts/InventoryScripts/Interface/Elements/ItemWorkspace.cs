using System.Linq;
using Assets.DeadCell.Scripts.Data;
using Assets.DeadCell.Scripts.Enums;
using UnityEngine;

namespace Assets.DeadCell.Scripts.Interface.Elements
{
    /// <summary>
    /// Abstract item workspace. It can be shop or player inventory. Items can be managed here (selected, moved and so on).
    /// </summary>
    public abstract class ItemWorkspace : MonoBehaviour
    {
        public ItemInfo ItemInfo;

        protected ItemId SelectedItem;
        protected ItemParams SelectedItemParams;

        public abstract void Refresh();

        protected void Reset()
        {
            SelectedItem = ItemId.Undefined;
            ItemInfo.Reset();
        }
    }
}