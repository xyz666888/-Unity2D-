namespace Assets.DeadCell.Scripts.Enums
{
    /// <summary>
    /// Item tags can be used for implementing custom logic (special cases).
    /// Use constant integer values for enums to avoid data distortion when adding/removing new values.
    /// </summary>
    public enum ItemTag
    {
        Undefined   = 0,
        Rage        = 1,
        Tactical    = 2,
        Survival    = 3,
        Sword       = 4,
        Bow         = 5,
        Gun         = 6,
        Usable      = 7,
    }
}