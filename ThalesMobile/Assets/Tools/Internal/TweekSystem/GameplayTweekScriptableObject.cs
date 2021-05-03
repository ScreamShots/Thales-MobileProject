//Random Generatated char value: c338ca7f-2122-4226-9ec7-a25b48db9318
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
[Path("From")] public string s0_From = "Assets/Prefabs/Assets/Prefabs/InterestPoints";

[Header("GameObject: InterestPoint(AircraftCarrier)")]
[Id("InterestPoint(AircraftCarrier) Component - ID:")] public string s1Id = "44b66497-e961-461e-a000-367bfbf368cc";

[Comp("InterestPoint Component - ID:")] public string s2Comp = "44b66497-e961-461e-a000-367bfbf368cc_c7b033f1-65cc-4b24-919d-f895b16b013e";

[Var("hackTime")] public float hackTime_44b66497µe961µ461eµa000µ367bfbf368cc_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("hackingRange")] public float hackingRange_44b66497µe961µ461eµa000µ367bfbf368cc_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("sendAlert")] public bool sendAlert_44b66497µe961µ461eµa000µ367bfbf368cc_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("detectionAlertRange")] public float detectionAlertRange_44b66497µe961µ461eµa000µ367bfbf368cc_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;
[Var("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_44b66497µe961µ461eµa000µ367bfbf368cc_c7b033f1µ65ccµ4b24µ919dµf895b16b013e;

[Header("GameObject: InterestPoint(Harbor)")]
[Id("InterestPoint(Harbor) Component - ID:")] public string s3Id = "c94bef12-2884-4ce8-b00c-bbf99bfdc668";

[Comp("InterestPoint Component - ID:")] public string s4Comp = "c94bef12-2884-4ce8-b00c-bbf99bfdc668_438901e8-fa6b-4245-aa3d-45df27cb0ad2";

[Var("hackTime")] public float hackTime_c94bef12µ2884µ4ce8µb00cµbbf99bfdc668_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("hackingRange")] public float hackingRange_c94bef12µ2884µ4ce8µb00cµbbf99bfdc668_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("sendAlert")] public bool sendAlert_c94bef12µ2884µ4ce8µb00cµbbf99bfdc668_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("detectionAlertRange")] public float detectionAlertRange_c94bef12µ2884µ4ce8µb00cµbbf99bfdc668_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;
[Var("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_c94bef12µ2884µ4ce8µb00cµbbf99bfdc668_438901e8µfa6bµ4245µaa3dµ45df27cb0ad2;

[Header("GameObject: InterestPoint(OilRig)")]
[Id("InterestPoint(OilRig) Component - ID:")] public string s5Id = "b62ef7f2-46d8-4b6f-8c3c-9cbb3ca95283";

[Comp("InterestPoint Component - ID:")] public string s6Comp = "b62ef7f2-46d8-4b6f-8c3c-9cbb3ca95283_99966c11-90f3-4aef-b323-beff7b8bcfd1";

[Var("hackTime")] public float hackTime_b62ef7f2µ46d8µ4b6fµ8c3cµ9cbb3ca95283_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("hackingRange")] public float hackingRange_b62ef7f2µ46d8µ4b6fµ8c3cµ9cbb3ca95283_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("sendAlert")] public bool sendAlert_b62ef7f2µ46d8µ4b6fµ8c3cµ9cbb3ca95283_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("detectionAlertRange")] public float detectionAlertRange_b62ef7f2µ46d8µ4b6fµ8c3cµ9cbb3ca95283_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;
[Var("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_b62ef7f2µ46d8µ4b6fµ8c3cµ9cbb3ca95283_99966c11µ90f3µ4aefµb323µbeff7b8bcfd1;

[Header("GameObject: InterestPoint(OilTanker)")]
[Id("InterestPoint(OilTanker) Component - ID:")] public string s7Id = "2b28cc6c-ae52-4e97-8b68-7f530b1ab67c";

[Comp("InterestPoint Component - ID:")] public string s8Comp = "2b28cc6c-ae52-4e97-8b68-7f530b1ab67c_7e1dbc6e-4348-4762-9233-30a708fa6302";

[Var("hackTime")] public float hackTime_2b28cc6cµae52µ4e97µ8b68µ7f530b1ab67c_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("hackingRange")] public float hackingRange_2b28cc6cµae52µ4e97µ8b68µ7f530b1ab67c_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("sendAlert")] public bool sendAlert_2b28cc6cµae52µ4e97µ8b68µ7f530b1ab67c_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("detectionAlertRange")] public float detectionAlertRange_2b28cc6cµae52µ4e97µ8b68µ7f530b1ab67c_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;
[Var("timeInRangeBeforeAlert")] public float timeInRangeBeforeAlert_2b28cc6cµae52µ4e97µ8b68µ7f530b1ab67c_7e1dbc6eµ4348µ4762µ9233µ30a708fa6302;

[Space]
[Path("From")] public string s9_From = "Assets/Prefabs/Assets/Prefabs/Player Ocean Entities";

[Header("GameObject: Helicopter")]
[Id("Helicopter Component - ID:")] public string s10Id = "a5135e0e-b0c8-4871-9721-538499ed3b5c";

[Comp("Helicopter Component - ID:")] public string s11Comp = "a5135e0e-b0c8-4871-9721-538499ed3b5c_0de50deb-025d-4dc7-a0a2-bc9c1e3892f1";

[Var("preparationDuration")] public float preparationDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("alertDuration")] public float alertDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("flightDuration")] public float flightDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("cooldownDuration")] public float cooldownDuration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("activeEquipement")] public Equipement activeEquipement_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("speed")] public float speed_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("acceleration")] public float acceleration_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("rotateSpeed")] public float rotateSpeed_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;

[Header("GameObject: Plane")]
[Id("Plane Component - ID:")] public string s12Id = "cfa900bd-9810-4d08-b5f6-23efaf541d01";

[Comp("Plane Component - ID:")] public string s13Comp = "cfa900bd-9810-4d08-b5f6-23efaf541d01_86e762ea-c3c0-4fde-b405-42694734145d";

[Var("passiveEquipement")] public Equipement passiveEquipement_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("activeEquipement")] public Equipement activeEquipement_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("speed")] public float speed_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("acceleration")] public float acceleration_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("rotateSpeed")] public float rotateSpeed_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;

[Header("GameObject: Ship")]
[Id("Ship Component - ID:")] public string s14Id = "eaa950de-a907-4c47-8e3b-43fcedc9cef6";

[Comp("Ship Component - ID:")] public string s15Comp = "eaa950de-a907-4c47-8e3b-43fcedc9cef6_d3d3ef92-cbe1-464f-a09f-4e65bb73f4a5";

[Var("passiveEquipement")] public Equipement passiveEquipement_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("activeEquipement")] public Equipement activeEquipement_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("speed")] public float speed_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("acceleration")] public float acceleration_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("rotateSpeed")] public float rotateSpeed_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;

[Space]
[Path("From")] public string s16_From = "Assets/Prefabs/Assets/Prefabs/Submarine Entities";

[Header("GameObject: Dolphin")]
[Id("Dolphin Component - ID:")] public string s17Id = "e8a197d5-188f-4d3e-8f98-3674bfe31ee8";

[Comp("Dolphin Component - ID:")] public string s18Comp = "e8a197d5-188f-4d3e-8f98-3674bfe31ee8_7541256e-ebd4-4b0c-acb5-2dcc28bd16d2";

[Var("speed")] public float speed_e8a197d5µ188fµ4d3eµ8f98µ3674bfe31ee8_7541256eµebd4µ4b0cµacb5µ2dcc28bd16d2;

[Header("GameObject: SpermWhale")]
[Id("SpermWhale Component - ID:")] public string s19Id = "bfcba3e2-73b1-4875-918c-ca96d28a17f1";

[Comp("SpermWhale Component - ID:")] public string s20Comp = "bfcba3e2-73b1-4875-918c-ca96d28a17f1_7c993a60-b0bf-49f5-9e06-69d66101a1db";

[Var("speed")] public float speed_bfcba3e2µ73b1µ4875µ918cµca96d28a17f1_7c993a60µb0bfµ49f5µ9e06µ69d66101a1db;

[Header("GameObject: Submarine")]
[Id("Submarine Component - ID:")] public string s21Id = "b37c2e93-aa03-4086-988d-7ac4188c2d2c";

[Comp("Submarine Component - ID:")] public string s22Comp = "b37c2e93-aa03-4086-988d-7ac4188c2d2c_bd1bd582-bd2f-42b4-a0c3-15dd959bd187";

[Var("maxSpeed")] public float maxSpeed_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("acceleration")] public float acceleration_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangeCalm")] public float detectionRangeCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangeWorried")] public float detectionRangeWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("detectionRangePanicked")] public float detectionRangePanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("sonobuoyVigiIncr")] public float sonobuoyVigiIncr_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("fregateStationaryVigiIncr")] public float fregateStationaryVigiIncr_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("fregateMoveVigiIncr")] public float fregateMoveVigiIncr_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("minRange")] public float minRange_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("subZone12Subdivision")] public int subZone12Subdivision_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("subZone3SubSubdivision")] public int subZone3SubSubdivision_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("avoidEffectSliceReach")] public int avoidEffectSliceReach_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("intermediatePosRefreshRate")] public float intermediatePosRefreshRate_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceToRefrehIntemediatePos")] public float distanceToRefrehIntemediatePos_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("benefPointFactorBioCalm")] public float benefPointFactorBioCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("benefPointFactorBioWorried")] public float benefPointFactorBioWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("benefPointFactorBioPanicked")] public float benefPointFactorBioPanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSonobuoyCalm")] public float beneftPointFactorSonobuoyCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSonobuoyWorried")] public float beneftPointFactorSonobuoyWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSonobuoyPanicked")] public float beneftPointFactorSonobuoyPanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaWayCalm")] public float beneftPointFactorSeaWayCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaWayWorried")] public float beneftPointFactorSeaWayWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaWayPanicked")] public float beneftPointFactorSeaWayPanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaTurbulentCalm")] public float beneftPointFactorSeaTurbulentCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaTurbulentWorried")] public float beneftPointFactorSeaTurbulentWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorSeaTurbulentPanicked")] public float beneftPointFactorSeaTurbulentPanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorWindyZoneCalm")] public float beneftPointFactorWindyZoneCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorWindyZoneWorried")] public float beneftPointFactorWindyZoneWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("beneftPointFactorWindyZonePanicked")] public float beneftPointFactorWindyZonePanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceFactorWeightWhileCalm")] public float distanceFactorWeightWhileCalm_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceFactorWeightWhileWorried")] public float distanceFactorWeightWhileWorried_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("distanceFactorWeightWhilePanicked")] public float distanceFactorWeightWhilePanicked_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;

}
