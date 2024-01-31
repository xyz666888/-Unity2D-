using System;
using System.Collections.Generic;
using Assets.DeadCell.Scripts.Data;
using Assets.DeadCell.Scripts.Enums;
using Assets.DeadCell.Scripts.GameData;
using Assets.DeadCell.Scripts.Interface.Elements;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DeadCell.Scripts.Interface
{
    /// <summary>
    /// High-level inventory inverface.
    /// </summary>
    public class Inventory : ItemWorkspace
    {
        public ScrollInventory Bag;
        public Text playerInfo;

        private static Inventory instance;

        /// <summary>
        /// Initialize owned items (just for example).
        /// </summary>


        private void UpdateInventory()
        {
            var inventory = new List<Item>();
            /*var inventory = new List<Item>
            {
                new Item(ItemId.RageScroll, PlayerAttribute.Instance.attribute[0]),
                new Item(ItemId.TacticalScroll, PlayerAttribute.Instance.attribute[1]),
                new Item(ItemId.SurvivalScroll, PlayerAttribute.Instance.attribute[2]),
            };*/
            if(PlayerAttribute.Instance.bloodBottles > 0)
            {
                inventory.Add(new Item(ItemId.HealthPotion, PlayerAttribute.Instance.bloodBottles));
            }
            if(PlayerAttribute.Instance.goldCoins > 0)
            {
                inventory.Add(new Item(ItemId.Gold,PlayerAttribute.Instance.goldCoins));
            }
            if(PlayerAttribute.Instance.weapons.Count > 0)
            {
                foreach(Weapons weapon in PlayerAttribute.Instance.weapons)
                {
                    ItemId itemid = (ItemId)Enum.Parse(typeof(ItemId), weapon.weaponName);
                    inventory.Add(new Item(itemid, 1));
                }
            }
            if(PlayerAttribute.Instance.skills.Count > 0)
            {
                foreach(Skill skill in PlayerAttribute.Instance.skills)
                {
                    ItemId itemid = (ItemId)Enum.Parse(typeof(ItemId), skill.skillName);
                    inventory.Add(new Item(itemid, 1));
                }
            }
            Bag.Initialize(ref inventory);
            this.playerInfo.text = PlayerAttribute.Instance.currentBlood + "\n" + PlayerAttribute.Instance.attribute[0].ToString()
                +"\n" + PlayerAttribute.Instance.attribute[1].ToString() + "\n" + PlayerAttribute.Instance.attribute[2].ToString();
        }

        public void OpenBag()
        {
            this.gameObject.SetActive(true);
            UpdateInventory();
        }

        public void CloseBag()
        {
            this.gameObject.SetActive(false);
        }

        protected void Start()
        {
            Reset();
            InventoryItem.OnItemSelected = SelectItem;
            InventoryItem.OnDragStarted = SelectItem;
        }

        public void SelectItem(Item item)
        {
            SelectItem(item.Id);
        }

        public void SelectItem(ItemId itemId)
        {
            SelectedItem = itemId;
            SelectedItemParams = Items.Params[itemId];
            ItemInfo.Initialize(SelectedItem, SelectedItemParams);
            Refresh();
        }


        public override void Refresh()
        {
            if (SelectedItem == ItemId.Undefined)
            {
                ItemInfo.Reset();
            }
        }

        public static Inventory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<Inventory>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(Inventory).ToString();
                        instance = obj.AddComponent<Inventory>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    }
}