using UnityEngine;

namespace Bernadetta.Utilities
{
    public class GlobalConfig : PersistentSingleton<GlobalConfig>
    {
#if IS_2D
        public const bool IS_2D = true;
#else
        public const bool Is2D = false;
#endif
    }
}