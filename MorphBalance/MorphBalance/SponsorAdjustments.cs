using Harmony;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MorphBalance
{
    class SponsorAdjustments
    {
        [HarmonyPatch(typeof(UISponsorsSummary), "Setup")]
        public static class UISponsorsSummary_Setup_Patch
        {
            public static void Postfix(UISponsorsSummary __instance) {
                GameObject newLock1 = GameObject.Find("newLock1");
                if (newLock1 == null) {
                    newLock1 = Object.Instantiate(__instance.appealLock);
                    newLock1.transform.parent = __instance.appealLock.transform.parent;
                    newLock1.transform.localPosition = new Vector3(__instance.appealLock.transform.localPosition.x - 26,
                        __instance.appealLock.transform.localPosition.y,
                        __instance.appealLock.transform.localPosition.z);
                    newLock1.name = "newLock1";
                }
                GameObject newLock2 = GameObject.Find("newLock2");
                if (GameObject.Find("newLock2") == null) {
                    newLock2 = Object.Instantiate(__instance.appealLock);
                    newLock2.transform.parent = __instance.appealLock.transform.parent;
                    newLock2.transform.localPosition = new Vector3(__instance.appealLock.transform.localPosition.x - 52,
                        __instance.appealLock.transform.localPosition.y,
                        __instance.appealLock.transform.localPosition.z);
                    newLock2.name = "newLock2";
                }
                if (Game.instance.player.team.perksManager.CheckPerkUnlocked(TeamPerk.Type.SponsorsLevel5)) {
                    newLock1.transform.localPosition = new Vector3(__instance.appealLock.transform.localPosition.x + 26,
                        __instance.appealLock.transform.localPosition.y,
                        __instance.appealLock.transform.localPosition.z);
                    newLock2.transform.localPosition = new Vector3(__instance.appealLock.transform.localPosition.x + 26,
                        __instance.appealLock.transform.localPosition.y,
                        __instance.appealLock.transform.localPosition.z);
                }
                if (Game.instance.player.team.championship.championshipOrderRelative == 2) {
                    newLock1.SetActive(true);
                    newLock2.SetActive(true);
                }
                else if (Game.instance.player.team.championship.championshipOrderRelative == 1) {
                    newLock1.SetActive(true);
                    newLock2.SetActive(false);
                }
                else if (Game.instance.player.team.championship.championshipOrderRelative == 0) {
                    newLock1.SetActive(false);
                    newLock2.SetActive(false);
                }
            }
        }

        [HarmonyPatch(typeof(Team), "GetSponsorAppeal")]
        public static class Team_GetSponsorAppeal_Patch
        {
            public static void Postfix(Team __instance, ref int __result) {
                if (__instance.championship.championshipOrderRelative == 2) {
                    __result = Mathf.Clamp(__result, 1, 2);
                }
                else if (__instance.championship.championshipOrderRelative == 1) {
                    __result = Mathf.Clamp(__result, 1, 3);
                }
                else if (__instance.championship.championshipOrderRelative == 0) {
                    __result = Mathf.Clamp(__result, 1, 4);
                }

                if (__instance.perksManager.CheckPerkUnlocked(TeamPerk.Type.SponsorsLevel5)) {
                    __result++;
                }
            }
        }
    }
}
