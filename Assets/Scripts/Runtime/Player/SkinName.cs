using System;

namespace Core.Player
{
    [Flags]
    public enum SkinName
    {
        Capy = 1 << 0,
        Greenbara = 1 << 1,
        Purplebara = 1 << 2,
        Pinkbara = 1 << 3,
        Darkbara = 1 << 4,
        Streetbara = 1 << 5,
        Redbara = 1 << 6
    }
}