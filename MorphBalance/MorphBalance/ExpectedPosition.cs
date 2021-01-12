using Harmony;
using UnityEngine;

namespace MorphBalance
{
    class ExpectedPosition
    {
        [HarmonyPatch(typeof(Chairman), "GetEstimatedPosition")]
        public static class Chairman_GetEstimatedPosition_Patch
        {
            public static void Postfix(Chairman __instance, ref int __result, Chairman.EstimatedPosition inPosition, Team inTeam) {
                int startOfSeasonExpectedChampionshipResult = inTeam.startOfSeasonExpectedChampionshipResult;
                int teamEntryCount = inTeam.championship.standings.teamEntryCount;
                switch (inPosition) {
                    case Chairman.EstimatedPosition.Low:
                        __result = Mathf.Clamp(startOfSeasonExpectedChampionshipResult, 1, teamEntryCount);
                        break;
                    case Chairman.EstimatedPosition.Medium:
                        __result = Mathf.Clamp(startOfSeasonExpectedChampionshipResult - 1, 1, teamEntryCount);
                        break;
                    case Chairman.EstimatedPosition.High:
                        __result = Mathf.Clamp(startOfSeasonExpectedChampionshipResult - 2, 1, teamEntryCount);
                        break;
                    default:
                        __result = 0;
                        break;
                }
            }
        }
    }
}
