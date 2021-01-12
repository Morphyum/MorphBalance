using Harmony;
using System.Collections.Generic;

namespace MorphBalance
{
    class ChampionshipPriceFunds
    {
        [HarmonyPatch(typeof(ChampionshipManager), "OnStart")]
        public static class ChampionshipManager_OnStart_Patch
        {
            public static void Postfix(ref ChampionshipManager __instance) {
                List<Championship> list = __instance.GetChampionshipsForSeries(Championship.Series.GTSeries);
                foreach (Championship championship in list) {
                    if (championship.championshipOrderRelative == 0) {
                        championship.prizeFund += 125000000;
                    }
                    else if (championship.championshipOrderRelative == 1) {
                        championship.prizeFund += 75000000;
                    }
                }

                list = __instance.GetChampionshipsForSeries(Championship.Series.EnduranceSeries);
                foreach (Championship championship in list) {
                    if (championship.championshipOrderRelative == 0) {
                        championship.prizeFund += 100000000;
                    }
                    else if (championship.championshipOrderRelative == 1) {
                        championship.prizeFund += 50000000;
                    }
                }
            }
        }
    }
}
