using System.Collections.Generic;
using Assets.DeadCell.Scripts.Data;
using Assets.DeadCell.Scripts.Enums;

namespace Assets.DeadCell.Scripts.GameData
{

    public class Items
    {
        public static readonly Dictionary<ItemId, ItemParams> Params = new Dictionary<ItemId, ItemParams>
        {
            {
                ItemId.RageScroll,
                new ItemParams
                {
                    Type = ItemType.Scroll,
                    Tags = new List<ItemTag>{ItemTag.Rage },
                    detailDescription = "This is the Scroll of Rage, when you pick it up you will use it to add 1 to your character's Rage attribute."+
                    "\nOnce picked up it is automatically used."
                }
            },{
                ItemId.TacticalScroll,
                new ItemParams
                {
                    Type = ItemType.Scroll,
                    Tags = new List<ItemTag>{ItemTag.Tactical },
                    detailDescription = "This is a tactical scroll. When you pick it up, it adds one to your character's tactical attributes."
                        +"\nOnce picked up it is automatically used."
                }
            },{
                ItemId.SurvivalScroll,
                new ItemParams
                {
                    Type = ItemType.Scroll,
                    Tags = new List<ItemTag>{ItemTag.Survival },
                    detailDescription = "This is a survival scroll. When you pick it up, it adds one to your character's survival attributes."
                        +"\nOnce picked up it is automatically used."
                }
            },
            {
                ItemId.Flute,
                new ItemParams
                {
                    Type = ItemType.Loot,
                }
            },
            {
                ItemId.Gold,
                new ItemParams
                {
                    Type = ItemType.Currency,
                    Properties = new List<Property> { new Property(PropertyId.Score, PlayerAttribute.Instance.goldCoins) },
                    detailDescription = "Gold represents your final score and is an important ranking decision variable."
                }
            },
            {
                ItemId.HealthPotion,
                new ItemParams
                {
                    Type = ItemType.Potion,
                    Properties = new List<Property> { new Property(PropertyId.RestoreHealth, 10) },
                    detailDescription = "Blood Bottles are capped at 3, and each use restores 10 points of a character's blood."
                }
            },
            {
                ItemId.Sword,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Sword,ItemTag.Survival },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 8) },
                    detailDescription = "This sword has many combos. " +
                    "When holding down the W and Attack buttons it triggers a pick off of enemies, " +
                    "when you press X it blocks, and when you use this skill in a sprint it triggers a sprint attack form."+
                    "It is a survival attribute weapon."
                }
            },
            {
                ItemId.Bow,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Bow, ItemTag.Tactical },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 5) },
                    detailDescription = "This is a bow and arrow with the attribute tactical that fires arrows to stab at enemies. It also has a knockback effect on enemies."+
                    "Each arrow deals 5 points of damage to the enemy."
                }
            },
            {
                ItemId.Gun,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Gun, ItemTag.Rage },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 3) },
                    detailDescription = "This is a gun with the attribute rage. It fires bullets to stab at enemies. It also has a knockback effect on enemies."+
                    "Each bullet deals 3 points of damage to the enemy."
                }
            },
            {
                ItemId.RainOfArrows,
                new ItemParams
                {
                    Type = ItemType.Skill,
                    Tags = new List<ItemTag> { ItemTag.Rage },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 5),new Property(PropertyId.MaxNum, 20),new Property(PropertyId.CoolDown, 10) },
                    detailDescription = "This is a skill with the attribute rage. It fires arrows to stab at enemies. It also has a knockback effect on enemies."+
                    "Each arrow deals 5 points of damage to the enemy."+
                    "It has a 10-second cooldown and instantly shoots 20 arrows at a time with a wide range and high damage."
                }
            },
            {
                ItemId.ProtectRing,
                new ItemParams
                {
                    Type = ItemType.Skill,
                    Tags = new List<ItemTag> { ItemTag.Survival },
                    Properties = new List<Property> { new Property(PropertyId.Duration, 5),new Property(PropertyId.CoolDown,20) },
                    detailDescription = "This is a skill with the attribute survival. It can be used to protect yourself from enemy attacks."+
                    "When you use this skill, you gain 5 seconds of invincibility, immunity to all attacks and bad controls, and a cooldown of 20 seconds."
                }
            },
            {
                ItemId.DestructionWings,
                new ItemParams
                {
                    Type = ItemType.Skill,
                    Tags = new List<ItemTag> { ItemTag.Rage },
                    Properties = new List<Property> { new Property(PropertyId.Duration, 7),new Property(PropertyId.CoolDown,20) },
                    detailDescription = "Wings of Destruction can damage all enemies within 1 yard, deducting 5 drops of enemy health each time"
                }
            },
            {
                ItemId.LightExplosion,
                new ItemParams
                {
                    Type = ItemType.Skill,
                    Tags = new List<ItemTag> { ItemTag.Tactical },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage,20),new Property(PropertyId.CoolDown,10) },
                    detailDescription = "Emit a light wave forward and explode, which can blow up 20 drops of enemy health and cause range damage, " +
                    "but can only affect the enemy directly in front."
                }
            },
            {
                ItemId.SwordLight,
                new ItemParams
                {
                    Type = ItemType.Skill,
                    Tags = new List<ItemTag> { ItemTag.Survival },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage,10),new Property(PropertyId.CoolDown,10) },
                    detailDescription = "Full picture skill, all monsters in the current scene will be damaged, but please note before " +
                    "use that monsters will attack you once they are damaged. Remember not to use it casually."
                }
            },
        };
    }
}