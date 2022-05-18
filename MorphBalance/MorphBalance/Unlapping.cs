using Harmony;

namespace MorphBalance
{
    class Unlapping
    {
        [HarmonyPatch(typeof(AISafetyFlagBehaviour), "SimulationUpdate")]
        public static class AISafetyFlagBehaviour_SimulationUpdate_Patch
        {
            public static void Postfix(AISafetyFlagBehaviour __instance, ref Vehicle ___mVehicle, ref RacingVehicle ___mRacingVehicle, ref bool ___mCanOverTake) {
                if (__instance.isInSafetyTrain && (___mRacingVehicle.GetLapsBehindLeader() > 0 || ___mRacingVehicle.timer.lap < ___mRacingVehicle.leader.timer.lap)) {
                    Vehicle safetyVehicle = Game.instance.vehicleManager.safetyVehicle;
                    SafetyVehicle safetyVehicle2 = safetyVehicle as SafetyVehicle;
                    safetyVehicle2.FlashGreenLights();
                    ___mCanOverTake = true;
                    ___mRacingVehicle.behaviourManager.SetCanAttackVehicle(true);
                    ___mVehicle.speedManager.GetController<SafetyCarSpeedController>().topSpeed = GameUtility.MilesPerHourToMetersPerSecond(200f);
                }
            }
        }

        [HarmonyPatch(typeof(AIBlueFlagBehaviour), "IsInBlueFlagZoneOf")]
        public static class AIBlueFlagBehaviour_IsInBlueFlagZoneOf_Patch {
            public static bool Prefix(UITeamScreenTeamInfoWidget __instance, Vehicle inVehicleBehind, Vehicle inVehicleAhead, ref bool __result) {
                float minTimeAhead = 1f;
                if(inVehicleBehind.pathController.IsOnComparablePath(inVehicleAhead)) {
                    float pathDistanceToVehicle = inVehicleBehind.pathController.GetPathDistanceToVehicle(inVehicleAhead);
                    SessionManager sessionManager = Game.instance.sessionManager;
                    if(pathDistanceToVehicle > 0 && !sessionManager.hasSessionEnded) {
                        if(sessionManager.sessionType == SessionDetails.SessionType.Race) {
                            GateInfo gateTimer = sessionManager.GetGateTimer(inVehicleBehind.pathController.GetPreviousGate().id);
                            float timeGapBetweenVehicles = gateTimer.GetTimeGapBetweenVehicles(inVehicleAhead, inVehicleBehind);
                            if(timeGapBetweenVehicles < minTimeAhead || inVehicleAhead.performance.IsExperiencingCriticalIssue()) {
                                __result = true;
                                return false;
                            }
                        } else {
                            if(inVehicleAhead.pathController.IsOnStraight() && inVehicleAhead.speed < inVehicleBehind.speed) {
                                float num = inVehicleBehind.speed - inVehicleAhead.speed;
                                float num2 = (pathDistanceToVehicle - VehicleConstants.vehicleLength) / num;
                                if(num2 < 4f) {
                                    __result = true;
                                    return false;
                                }
                            }
                            bool flag = pathDistanceToVehicle - VehicleConstants.vehicleLength * 6f < 0f;
                            if(flag) {
                                __result = true;
                                return false;
                            }
                        }
                    }
                }
                __result = false;
                return false;
            }
        }
    }
}
