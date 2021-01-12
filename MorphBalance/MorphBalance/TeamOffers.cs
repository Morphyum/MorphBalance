using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace MorphBalance
{
    class TeamOffers
    {
        [HarmonyPatch(typeof(TeamPrincipal), "canJoinTeam")]
        public static class TeamPrincipal_canJoinTeam_Patch
        {
            public static void Postfix(TeamPrincipal __instance, ref bool __result, Team inTeam) {
                if (__result == true) {
                    __result = false;
                    if (!Game.instance.player.IsUnemployed()) {
                        if (inTeam.teamStatistics.GetTeamStars() <= Game.instance.player.team.teamStatistics.GetTeamStars()) {
                            __result = true;
                        }
                    }
                    else if (inTeam.teamStatistics.GetTeamStars() <= Game.instance.player.careerHistory.previousTeam.teamStatistics.GetTeamStars()) {
                        __result = true;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(UIPlayerAvailableJobsWidget), "Setup")]
        public static class TeamPrincipal_UIPlayerAvailableJobsWidget_Setup
        {
            public static bool Prefix(UIPlayerAvailableJobsWidget __instance) {
                List<Team> mTeams = new List<Team>();
                __instance.grid.DestroyListItems();
                List<Championship> entityList = Game.instance.championshipManager.GetEntityList();
                GameUtility.SetActive(__instance.jobEntry, true);
                GameUtility.SetActive(__instance.noJobsEntry, true);
                GameUtility.SetActive(__instance.championshipEntry, true);
                for (int i = 0; i < entityList.Count; i++) {
                    Championship championship = entityList[i];
                    if (championship.isChoosable && App.instance.dlcManager.IsSeriesAvailable(championship.series)) {
                        mTeams.Clear();
                        int teamEntryCount = championship.standings.teamEntryCount;
                        int num = 0;
                        for (int j = 0; j < teamEntryCount; j++) {
                            Team entity = championship.standings.GetTeamEntry(j).GetEntity<Team>();
                            if (!Game.instance.player.HasAppliedForTeam(entity) && Game.instance.player.canJoinTeam(entity)) {
                                mTeams.Add(entity);
                                num++;
                            }
                        }
                        if (num > 0) {
                            __instance.grid.itemPrefab = __instance.championshipEntry;
                            UIPlayerJobChampionshipEntry uiplayerJobChampionshipEntry = __instance.grid.CreateListItem<UIPlayerJobChampionshipEntry>();
                            uiplayerJobChampionshipEntry.Setup(championship);
                            mTeams.Sort((Team x, Team y) => x.GetVacancyAppeal().CompareTo(y.GetVacancyAppeal()));
                            int count = mTeams.Count;
                            __instance.grid.itemPrefab = __instance.jobEntry;
                            for (int k = 0; k < count; k++) {
                                UIPlayerJobEntry uiplayerJobEntry = __instance.grid.CreateListItem<UIPlayerJobEntry>();
                                uiplayerJobEntry.Setup(mTeams[k]);
                            }
                            if (num <= 0) {
                                __instance.grid.itemPrefab = __instance.noJobsEntry;
                                __instance.grid.CreateListItem<Transform>();
                            }
                        }
                    }
                }
                GameUtility.SetActive(__instance.jobEntry, false);
                GameUtility.SetActive(__instance.noJobsEntry, false);
                GameUtility.SetActive(__instance.championshipEntry, false);
                return false;
            }
        }
    }
}
