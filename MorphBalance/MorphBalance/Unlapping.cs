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
    }
}
