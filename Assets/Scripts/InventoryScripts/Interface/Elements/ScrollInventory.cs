using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.DeadCell.Scripts.Data;
using Assets.DeadCell.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DeadCell.Scripts.Interface.Elements
{
    /// <summary>
    /// Scrollable item container that can display item list. Automatic vertical scrolling.
    /// </summary>
    [RequireComponent(typeof(DragReceiver))]
    public class ScrollInventory : ItemContainer
    {
        public ScrollRect ScrollRect;
        public GridLayoutGroup Grid;
        public GameObject ItemPrefab;
        public GameObject CellPrefab;
        public int MinRows;
        public int ViewportOffset; // When scrollbar becomes visible

        private readonly List<ItemType> _sorting = new List<ItemType>
        {
            ItemType.Currency,
            ItemType.Loot,
            ItemType.Potion,
            ItemType.Scroll,
            ItemType.Weapon,
            ItemType.Skill,
        };
        private readonly List<InventoryItem> _items = new List<InventoryItem>();
        private List<ItemId> _hash = new List<ItemId>();

        public new void Initialize(ref List<Item> items)
        {
            base.Initialize(ref items);
        }
        
        public override void Refresh()
        {
            if (Items.Any(i => i.Count <= 0))
            {
                throw new Exception(string.Join(", ", Items.Where(i => i.Count <= 0).Select(i => i.Id.ToString()).ToArray()));
            }

            var refresh = Items.Select(i => i.Id).SequenceEqual(_hash);

            if (refresh && _items.Any())
            {
                foreach (var button in _items)
                {
                    button.Count.text = button.Item.Count.ToString();
                }
            }
            else
            {
                Reset();
            }
        }

        private void Reset()
        {
            _hash = Items.Select(i => i.Id).ToList();
            _items.Clear();

            foreach (Transform child in Grid.transform)
            {
                Destroy(child.gameObject);
            }

            var items = Items.OrderBy(i => _sorting.Contains(i.Params.Type) ? _sorting.IndexOf(i.Params.Type) : 0).ToList();
            var groups = items.GroupBy(i => i.Params.Type);

            items = new List<Item>();

            foreach (var group in groups)
            {
                items.AddRange(group.OrderBy(i => i.Params.detailDescription));
            }

            var dragReceiver = GetComponent<DragReceiver>();

            foreach (var item in items)
            {
                var button = Instantiate(ItemPrefab, Grid.transform).GetComponent<InventoryItem>();

                button.Item = item;
                button.Count.text = item.Count.ToString();
                button.Group = dragReceiver.Group;
                _items.Add(button);
            }

            if (Grid.constraint == GridLayoutGroup.Constraint.FixedColumnCount && MinRows > 0)
            {
                var columns = Grid.constraintCount;
                var rows = Mathf.Max(MinRows, Mathf.CeilToInt((float)items.Count / columns));

                for (var i = items.Count; i < columns * rows; i++)
                {
                    Instantiate(CellPrefab, Grid.transform);
                }
            }

            StartCoroutine(ResetScrollRect());
        }

        private IEnumerator ResetScrollRect()
        {
            yield return null;

            ScrollRect.verticalNormalizedPosition = 1;
            ScrollRect.horizontalNormalizedPosition = 0;

            yield return null;

            FixUnityScrollRectBug();
        }


        private void FixUnityScrollRectBug()
        {
            var scrollbar = ScrollRect.verticalScrollbar;
            var offset = scrollbar.IsActive() ? ViewportOffset : 0;

            ScrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ScrollRect.GetComponent<RectTransform>().rect.width - offset);
        }
    }
}