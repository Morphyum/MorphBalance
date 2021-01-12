using Harmony;
using System.Reflection;

namespace MorphBalance
{
    public class MorphBalance
    {
        public static void Init() {
            var harmony = HarmonyInstance.Create("de.morphyum.MorphBalance");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
