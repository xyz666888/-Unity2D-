using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.DeadCell.Scripts.Data;
using Assets.DeadCell.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.DeadCell.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents item when it was selected. Displays icon, name, price and properties.
    /// </summary>
    public class ItemInfo : MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public Text detailDescription;
        public Image Icon;

        public void Reset()
        {
            Name.text = Description.text = detailDescription.text = null;
            Icon.sprite = ImageCollection.Instance.DefaultItemIcon;
        }

        public void Initialize(ItemId itemId, ItemParams itemParams)
        {
            Icon.sprite = ImageCollection.Instance.GetIcon(itemId);
            Name.text = SplitName(itemId.ToString());
            Description.text = $"Here will be {itemId} description soon...";
            detailDescription.text = $"Detail Description: {itemParams.detailDescription}";

            var description = new List<string> {$"Type: {itemParams.Type}"};

            if (itemParams.Tags.Any())
            {
                description[description.Count - 1] += $" <color=grey>[{string.Join(", ", itemParams.Tags.Select(i => $"{i}").ToArray())}]</color>";
            }

            foreach (var attribute in itemParams.Properties)
            {
                description.Add($"{SplitName(attribute.Id.ToString())}: {attribute.Value}");
            }

            Description.text = string.Join(Environment.NewLine, description.ToArray());
        }
        
        public static string SplitName(string name)
        {
            return Regex.Replace(Regex.Replace(name, "[A-Z]", " $0"), "([a-z])([1-9])", "$1 $2").Trim();
        }
    }
}