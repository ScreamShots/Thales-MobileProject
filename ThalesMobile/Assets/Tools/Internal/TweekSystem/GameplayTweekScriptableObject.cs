//Random Generatated char value: 689d42ca-8900-4d8b-8141-57fd56c7f4b8
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweek.ScoAttributes;
using PlayerEquipement;

[CreateAssetMenu(menuName ="Tweek/Gameplay Asset")]
public class GameplayTweekScriptableObject : ScriptableObject
{
[Header("Objects From Prefabs")]

[Space]
[Path("From")] public string s0_From = "Assets/Prefabs";

[Header("GameObject: Camera")]
[Id("Camera Component - ID:")] public string s1Id = "7daf19fe-7896-41d7-91af-86e6eb827d7e";

[Comp("CameraController Component - ID:")] public string s2Comp = "7daf19fe-7896-41d7-91af-86e6eb827d7e_16429c5f-0eb5-411a-8ee1-69f7caa274d1";

[Var("zoomSpeed")] [Tooltip("zoomSpeed")] public float zoomSpeed_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("camSett")] [Tooltip("camSett")] public CameraSettings camSett_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("minimalMoveFactor")] [Tooltip("minimalMoveFactor")] public float minimalMoveFactor_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("aimLerpSpeed")] [Tooltip("aimLerpSpeed")] public float aimLerpSpeed_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("moveSpeed")] [Tooltip("moveSpeed")] public float moveSpeed_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("refocusSpeed")] [Tooltip("refocusSpeed")] public float refocusSpeed_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;
[Var("mouvLerpSpeed")] [Tooltip("mouvLerpSpeed")] public float mouvLerpSpeed_7daf19feµ7896µ41d7µ91afµ86e6eb827d7e_16429c5fµ0eb5µ411aµ8ee1µ69f7caa274d1;

[Space]
[Path("From")] public string s3_From = "Assets/Prefabs/Assets/Prefabs/Equipement";

[Header("GameObject: Global DetectionPoint")]
[Id("Global DetectionPoint Component - ID:")] public string s4Id = "0fe29d07-2f86-4c84-9e21-362e9843ba29";

[Comp("GlobalDetectionPoint Component - ID:")] public string s5Comp = "0fe29d07-2f86-4c84-9e21-362e9843ba29_94c1c9f4-5b10-454a-afa5-97abe91fff0c";

[Var("expirationDuration")] [Tooltip("expirationDuration")] public float expirationDuration_0fe29d07µ2f86µ4c84µ9e21µ362e9843ba29_94c1c9f4µ5b10µ454aµafa5µ97abe91fff0c;
[Var("expirationWarningRatio")] [Tooltip("expirationWarningRatio")] public float expirationWarningRatio_0fe29d07µ2f86µ4c84µ9e21µ362e9843ba29_94c1c9f4µ5b10µ454aµafa5µ97abe91fff0c;
[Var("revealDuration")] [Tooltip("revealDuration")] public float revealDuration_0fe29d07µ2f86µ4c84µ9e21µ362e9843ba29_94c1c9f4µ5b10µ454aµafa5µ97abe91fff0c;

[Space]
[Path("From")] public string s6_From = "Assets/Prefabs/Assets/Prefabs/InterestPoints";

[Header("GameObject: InterestPoint(AircraftCarrier)")]
[Id("InterestPoint(AircraftCarrier) Component - ID:")] public string s7Id = "3e118160-a893-4445-9441-cc29276b98ec";

[Comp("InterestPoint Component - ID:")] public string s8Comp = "3e118160-a893-4445-9441-cc29276b98ec_c7b033f1-65cc-4b24-919d-f895b16b013e";

[Var("hackTime")] [Tooltip("hackTime")] public float hackTime_3e118160µa893µ4445µ9441µcc29276b98ec_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("hackingRange")] [Tooltip("hackingRange")] public float hackingRange_3e118160µa893µ4445µ9441µcc29276b98ec_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("sendAlert")] [Tooltip("sendAlert")] public bool sendAlert_3e118160µa893µ4445µ9441µcc29276b98ec_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("detectionAlertRange")] [Tooltip("detectionAlertRange")] public float detectionAlertRange_3e118160µa893µ4445µ9441µcc29276b98ec_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("timeInRangeBeforeAlert")] [Tooltip("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_3e118160µa893µ4445µ9441µcc29276b98ec_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;

[Header("GameObject: InterestPoint(Harbor)")]
[Id("InterestPoint(Harbor) Component - ID:")] public string s9Id = "8676da00-7e61-42e3-84e4-cd905a3e5e9d";

[Comp("InterestPoint Component - ID:")] public string s10Comp = "8676da00-7e61-42e3-84e4-cd905a3e5e9d_438901e8-fa6b-4245-aa3d-45df27cb0ad2";

[Var("hackTime")] [Tooltip("hackTime")] public float hackTime_8676da00µ7e61µ42e3µ84e4µcd905a3e5e9d_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("hackingRange")] [Tooltip("hackingRange")] public float hackingRange_8676da00µ7e61µ42e3µ84e4µcd905a3e5e9d_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("sendAlert")] [Tooltip("sendAlert")] public bool sendAlert_8676da00µ7e61µ42e3µ84e4µcd905a3e5e9d_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("detectionAlertRange")] [Tooltip("detectionAlertRange")] public float detectionAlertRange_8676da00µ7e61µ42e3µ84e4µcd905a3e5e9d_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("timeInRangeBeforeAlert")] [Tooltip("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_8676da00µ7e61µ42e3µ84e4µcd905a3e5e9d_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;

[Header("GameObject: InterestPoint(OilRig)")]
[Id("InterestPoint(OilRig) Component - ID:")] public string s11Id = "ce0166ac-9827-4c23-aead-513aca45b787";

[Comp("InterestPoint Component - ID:")] public string s12Comp = "ce0166ac-9827-4c23-aead-513aca45b787_99966c11-90f3-4aef-b323-beff7b8bcfd1";

[Var("hackTime")] [Tooltip("hackTime")] public float hackTime_ce0166acµ9827µ4c23µaeadµ513aca45b787_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("hackingRange")] [Tooltip("hackingRange")] public float hackingRange_ce0166acµ9827µ4c23µaeadµ513aca45b787_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("sendAlert")] [Tooltip("sendAlert")] public bool sendAlert_ce0166acµ9827µ4c23µaeadµ513aca45b787_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("detectionAlertRange")] [Tooltip("detectionAlertRange")] public float detectionAlertRange_ce0166acµ9827µ4c23µaeadµ513aca45b787_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("timeInRangeBeforeAlert")] [Tooltip("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_ce0166acµ9827µ4c23µaeadµ513aca45b787_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;

[Header("GameObject: InterestPoint(OilTanker)")]
[Id("InterestPoint(OilTanker) Component - ID:")] public string s13Id = "8eb177a3-aa98-46ea-b416-87f8372d9457";

[Comp("InterestPoint Component - ID:")] public string s14Comp = "8eb177a3-aa98-46ea-b416-87f8372d9457_7e1dbc6e-4348-4762-9233-30a708fa6302";

[Var("hackTime")] [Tooltip("hackTime")] public float hackTime_8eb177a3µaa98µ46eaµb416µ87f8372d9457_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("hackingRange")] [Tooltip("hackingRange")] public float hackingRange_8eb177a3µaa98µ46eaµb416µ87f8372d9457_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("sendAlert")] [Tooltip("sendAlert")] public bool sendAlert_8eb177a3µaa98µ46eaµb416µ87f8372d9457_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("detectionAlertRange")] [Tooltip("detectionAlertRange")] public float detectionAlertRange_8eb177a3µaa98µ46eaµb416µ87f8372d9457_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("timeInRangeBeforeAlert")] [Tooltip("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_8eb177a3µaa98µ46eaµb416µ87f8372d9457_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;

[Space]
[Path("From")] public string s15_From = "Assets/Prefabs/Assets/Prefabs/Player Ocean Entities";

[Header("GameObject: Helicopter")]
[Id("Helicopter Component - ID:")] public string s16Id = "a5135e0e-b0c8-4871-9721-538499ed3b5c";

[Comp("Helicopter Component - ID:")] public string s17Comp = "a5135e0e-b0c8-4871-9721-538499ed3b5c_4c652d5e-c239-4042-8a74-45e02cce3b57";

[Var("preparationDuration")] [Tooltip("preparationDuration")] public float preparationDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("alertDuration")] [Tooltip("alertDuration")] public float alertDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("flightDuration")] [Tooltip("flightDuration")] public float flightDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("cooldownDuration")] [Tooltip("cooldownDuration")] public float cooldownDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("activeEquipement")] [Tooltip("activeEquipement")] public Equipement activeEquipement_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("speed")] [Tooltip("speed")] public float speed_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("acceleration")] [Tooltip("acceleration")] public float acceleration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("rotateSpeed")] [Tooltip("rotateSpeed")] public float rotateSpeed_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;

[Header("GameObject: Plane")]
[Id("Plane Component - ID:")] public string s18Id = "725a0485-6640-4345-aa9d-92857d162799";

[Comp("Plane Component - ID:")] public string s19Comp = "725a0485-6640-4345-aa9d-92857d162799_86e762ea-c3c0-4fde-b405-42694734145d";

[Var("passiveEquipement")] [Tooltip("passiveEquipement")] public Equipement passiveEquipement_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("activeEquipement")] [Tooltip("activeEquipement")] public Equipement activeEquipement_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("speed")] [Tooltip("speed")] public float speed_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("acceleration")] [Tooltip("acceleration")] public float acceleration_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("rotateSpeed")] [Tooltip("rotateSpeed")] public float rotateSpeed_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;

[Header("GameObject: Ship")]
[Id("Ship Component - ID:")] public string s20Id = "eaa950de-a907-4c47-8e3b-43fcedc9cef6";

[Comp("Ship Component - ID:")] public string s21Comp = "eaa950de-a907-4c47-8e3b-43fcedc9cef6_d3d3ef92-cbe1-464f-a09f-4e65bb73f4a5";

[Var("passiveEquipement")] [Tooltip("passiveEquipement")] public Equipement passiveEquipement_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("activeEquipement")] [Tooltip("activeEquipement")] public Equipement activeEquipement_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("speed")] [Tooltip("speed")] public float speed_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("acceleration")] [Tooltip("acceleration")] public float acceleration_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("rotateSpeed")] [Tooltip("rotateSpeed")] public float rotateSpeed_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;

[Space]
[Path("From")] public string s22_From = "Assets/Prefabs/Assets/Prefabs/Submarine Entities";

[Header("GameObject: Boat")]
[Id("Boat Component - ID:")] public string s23Id = "bfcba3e2-73b1-4875-918c-ca96d28a17f1";

[Comp("Boat Component - ID:")] public string s24Comp = "bfcba3e2-73b1-4875-918c-ca96d28a17f1_b1c13a34-fa1b-4d94-928d-f3c20266a45c";

[Var("speed")] [Tooltip("speed")] public float speed_bfcba3e2µ73b1µ4875µ918cµca96d28a17f1_b1c13a34µfa1bµ4d94µ928dµf3c20266a45c;

[Header("GameObject: Dolphin")]
[Id("Dolphin Component - ID:")] public string s25Id = "e8a197d5-188f-4d3e-8f98-3674bfe31ee8";

[Comp("Dolphin Component - ID:")] public string s26Comp = "e8a197d5-188f-4d3e-8f98-3674bfe31ee8_7541256e-ebd4-4b0c-acb5-2dcc28bd16d2";

[Var("speed")] [Tooltip("speed")] public float speed_e8a197d5µ188fµ4d3eµ8f98µ3674bfe31ee8_7541256eµebd4µ4b0cµacb5µ2dcc28bd16d2;

[Header("GameObject: SpermWhale")]
[Id("SpermWhale Component - ID:")] public string s27Id = "bfcba3e2-73b1-4875-918c-ca96d28a17f1";

[Comp("SpermWhale Component - ID:")] public string s28Comp = "bfcba3e2-73b1-4875-918c-ca96d28a17f1_7c993a60-b0bf-49f5-9e06-69d66101a1db";

[Var("speed")] [Tooltip("speed")] public float speed_bfcba3e2µ73b1µ4875µ918cµca96d28a17f1_7c993a60µb0bfµ49f5µ9e06µ69d66101a1db;

[Header("GameObject: Submarine")]
[Id("Submarine Component - ID:")] public string s29Id = "96749c2f-3e89-4552-ad57-8a253754f207";

[Comp("Submarine Component - ID:")] public string s30Comp = "96749c2f-3e89-4552-ad57-8a253754f207_bd1bd582-bd2f-42b4-a0c3-15dd959bd187";

[Var("maxSpeed")] [Tooltip("maxSpeed")] public float maxSpeed_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("acceleration")] [Tooltip("acceleration")] public float acceleration_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("incraseVigilanceShipRange")] [Tooltip("incraseVigilanceShipRange")] public float incraseVigilanceShipRange_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangeCalm")] [Tooltip("detectionRangeCalm")] public float detectionRangeCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangeWorried")] [Tooltip("detectionRangeWorried")] public float detectionRangeWorried_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangePanicked")] [Tooltip("detectionRangePanicked")] public float detectionRangePanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("sonobuoyVigiIncr")] [Tooltip("sonobuoyVigiIncr")] public float sonobuoyVigiIncr_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("fregateStationaryVigiIncr")] [Tooltip("fregateStationaryVigiIncr")] public float fregateStationaryVigiIncr_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("fregateMoveVigiIncr")] [Tooltip("fregateMoveVigiIncr")] public float fregateMoveVigiIncr_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("headingChange")] [Tooltip("headingChange")] public CounterMeasure headingChange_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("radioSilence")] [Tooltip("radioSilence")] public CounterMeasure radioSilence_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("baitDecoy")] [Tooltip("baitDecoy")] public CounterMeasure baitDecoy_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("minRange")] [Tooltip("minRange")] public float minRange_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("subZone12Subdivision")] [Tooltip("subZone12Subdivision")] public int subZone12Subdivision_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("subZone3SubSubdivision")] [Tooltip("subZone3SubSubdivision")] public int subZone3SubSubdivision_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("avoidEffectSliceReach")] [Tooltip("avoidEffectSliceReach")] public int avoidEffectSliceReach_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("intermediatePosRefreshRate")] [Tooltip("intermediatePosRefreshRate")] public float intermediatePosRefreshRate_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceToRefrehIntemediatePos")] [Tooltip("distanceToRefrehIntemediatePos")] public float distanceToRefrehIntemediatePos_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("benefPointFactorBioCalm")] [Tooltip("benefPointFactorBioCalm")] public float benefPointFactorBioCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("benefPointFactorBioPanicked")] [Tooltip("benefPointFactorBioPanicked")] public float benefPointFactorBioPanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSonobuoyCalm")] [Tooltip("beneftPointFactorSonobuoyCalm")] public float beneftPointFactorSonobuoyCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSonobuoyPanicked")] [Tooltip("beneftPointFactorSonobuoyPanicked")] public float beneftPointFactorSonobuoyPanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaWayCalm")] [Tooltip("beneftPointFactorSeaWayCalm")] public float beneftPointFactorSeaWayCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaWayPanicked")] [Tooltip("beneftPointFactorSeaWayPanicked")] public float beneftPointFactorSeaWayPanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaTurbulentCalm")] [Tooltip("beneftPointFactorSeaTurbulentCalm")] public float beneftPointFactorSeaTurbulentCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaTurbulentPanicked")] [Tooltip("beneftPointFactorSeaTurbulentPanicked")] public float beneftPointFactorSeaTurbulentPanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorWindyZoneCalm")] [Tooltip("beneftPointFactorWindyZoneCalm")] public float beneftPointFactorWindyZoneCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorWindyZonePanicked")] [Tooltip("beneftPointFactorWindyZonePanicked")] public float beneftPointFactorWindyZonePanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceFactorWeightWhileCalm")] [Tooltip("distanceFactorWeightWhileCalm")] public float distanceFactorWeightWhileCalm_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceFactorWeightWhilePanicked")] [Tooltip("distanceFactorWeightWhilePanicked")] public float distanceFactorWeightWhilePanicked_96749c2fµ3e89µ4552µad57µ8a253754f207_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;

}
