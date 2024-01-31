using System.Collections.Generic;
using System.Linq;
using Assets.DeadCell.Scripts.Enums;
using UnityEngine;

namespace Assets.DeadCell.Scripts.Interface.Elements
{
    /// <summary>
    /// Global object that automatically grabs all required images.
    /// </summary>
    public class ImageCollection : MonoBehaviour
    {
        public List<Sprite> ItemIcons;
        public List<Sprite> ItemPatterns;
        public Sprite DefaultItemIcon;
        public static ImageCollection Instance;

        public void Awake()
        {
            Instance = this;
        }

        public Sprite GetIcon(ItemId id)
        {
            var icon = ItemIcons.SingleOrDefault(i => i.name == id.ToString());

            return icon ?? DefaultItemIcon;
        }

        #if UNITY_EDITOR

        public void OnValidate()
        {
            ItemIcons = GetSprites("Assets/Arts/Inventory/Images/ItemIcons");
        }

        private List<Sprite> GetSprites(string path)
        {
            var sprites = new List<Sprite>();

            foreach (var file in System.IO.Directory.GetFiles(path, "*.png", System.IO.SearchOption.AllDirectories))
            {
                var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(file);

                if (sprite == null)
                {
                    Debug.LogWarningFormat("Please check sprite import settings: {0}", file);
                }
                else
                {
                    sprites.Add(sprite);
                }
            }

            return sprites;
        }  

        #endif
    }
}