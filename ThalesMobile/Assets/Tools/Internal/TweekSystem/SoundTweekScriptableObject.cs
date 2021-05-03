//Random Generatated char value: 87f62821-4fd9-4cd8-9bfb-1bf91ed73af9
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tweek.ScoAttributes;
using PlayerEquipement;

[CreateAssetMenu(menuName ="Tweek/Sound Asset")]
public class SoundTweekScriptableObject : ScriptableObject
{
[Header("Objects From Prefabs")]

[Space]
[Path("From")] public string s0_From = "Assets/Prefabs/Assets/Prefabs/Player Ocean Entities";

[Header("GameObject: Helicopter")]
[Id("Helicopter Component - ID:")] public string s1Id = "a5135e0e-b0c8-4871-9721-538499ed3b5c";

[Comp("Helicopter Component - ID:")] public string s2Comp = "a5135e0e-b0c8-4871-9721-538499ed3b5c_0de50deb-025d-4dc7-a0a2-bc9c1e3892f1";

[Var("preparationSound")] public AudioClip preparationSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("takeOffSound")] public AudioClip takeOffSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("landingSound")] public AudioClip landingSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("movementSound")] public AudioClip movementSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;
[Var("waitingSound")] public AudioClip waitingSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_0de50debµ025dµ4dc7µa0a2µbc9c1e3892f1;

[Header("GameObject: Plane")]
[Id("Plane Component - ID:")] public string s3Id = "cfa900bd-9810-4d08-b5f6-23efaf541d01";

[Comp("Plane Component - ID:")] public string s4Comp = "cfa900bd-9810-4d08-b5f6-23efaf541d01_86e762ea-c3c0-4fde-b405-42694734145d";

[Var("movementSound")] public AudioClip movementSound_cfa900bdµ9810µ4d08µb5f6µ23efaf541d01_86e762eaµc3c0µ4fdeµb405µ42694734145d;

[Header("GameObject: Ship")]
[Id("Ship Component - ID:")] public string s5Id = "eaa950de-a907-4c47-8e3b-43fcedc9cef6";

[Comp("Ship Component - ID:")] public string s6Comp = "eaa950de-a907-4c47-8e3b-43fcedc9cef6_d3d3ef92-cbe1-464f-a09f-4e65bb73f4a5";

[Var("waitingSound")] public AudioClip waitingSound_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("movementSound")] public AudioClip movementSound_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;

[Space]
[Path("From")] public string s7_From = "Assets/Prefabs/Assets/Prefabs/Submarine Entities";

[Header("GameObject: Submarine")]
[Id("Submarine Component - ID:")] public string s8Id = "b37c2e93-aa03-4086-988d-7ac4188c2d2c";

[Comp("Submarine Component - ID:")] public string s9Comp = "b37c2e93-aa03-4086-988d-7ac4188c2d2c_bd1bd582-bd2f-42b4-a0c3-15dd959bd187";

[Var("inHackClip")] public AudioClip inHackClip_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("doneHackClip")] public AudioClip doneHackClip_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;

}
