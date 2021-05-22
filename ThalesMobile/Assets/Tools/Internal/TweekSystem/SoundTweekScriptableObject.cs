//Random Generatated char value: 95a5c219-3634-48e5-a17e-ed0ea4ec27b9
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
[Path("From")] public string s0_From = "Assets/Prefabs";

[Header("GameObject: GameManager")]
[Id("GameManager Component - ID:")] public string s1Id = "a6528b1f-4f6b-4b49-945d-cb0befdd9c06";

[Comp("InputManager Component - ID:")] public string s2Comp = "a6528b1f-4f6b-4b49-945d-cb0befdd9c06_92be3b7b-140b-45d2-a849-6bcec2ae1e5c";

[Var("setTargetSound")] [Tooltip("setTargetSound")] public AudioClip setTargetSound_a6528b1fµ4f6bµ4b49µ945dµcb0befdd9c06_92be3b7bµ140bµ45d2µa849µ6bcec2ae1e5c;
[Var("setTargetSoundVolume")] [Tooltip("setTargetSoundVolume")] public float setTargetSoundVolume_a6528b1fµ4f6bµ4b49µ945dµcb0befdd9c06_92be3b7bµ140bµ45d2µa849µ6bcec2ae1e5c;

[Space]
[Path("From")] public string s3_From = "Assets/Prefabs/Assets/Prefabs/Equipement";

[Header("GameObject: Captas4 FeedBack Object")]
[Id("Captas4 FeedBack Object Component - ID:")] public string s4Id = "5914fa4a-db61-4779-95ef-ea095181a348";

[Comp("Captas4Feedback Component - ID:")] public string s5Comp = "5914fa4a-db61-4779-95ef-ea095181a348_95ec4c1f-68ae-4af3-8ebc-654fba5cc3d3";

[Var("waveSound")] [Tooltip("waveSound")] public AudioClip waveSound_5914fa4aµdb61µ4779µ95efµea095181a348_95ec4c1fµ68aeµ4af3µ8ebcµ654fba5cc3d3;
[Var("waveSoundVolume")] [Tooltip("waveSoundVolume")] public float waveSoundVolume_5914fa4aµdb61µ4779µ95efµea095181a348_95ec4c1fµ68aeµ4af3µ8ebcµ654fba5cc3d3;

[Header("GameObject: Flash Feedback Object")]
[Id("Flash Feedback Object Component - ID:")] public string s6Id = "c8eb3796-86ac-4dfb-8e47-3c5549d4586e";

[Comp("FlashFeedback Component - ID:")] public string s7Comp = "c8eb3796-86ac-4dfb-8e47-3c5549d4586e_74668d70-f533-4144-a8dc-7d906cd19f06";

[Var("flashSound")] [Tooltip("flashSound")] public AudioClip flashSound_c8eb3796µ86acµ4dfbµ8e47µ3c5549d4586e_74668d70µf533µ4144µa8dcµ7d906cd19f06;
[Var("flashSoundVolume")] [Tooltip("flashSoundVolume")] public float flashSoundVolume_c8eb3796µ86acµ4dfbµ8e47µ3c5549d4586e_74668d70µf533µ4144µa8dcµ7d906cd19f06;

[Header("GameObject: HullSonar DetectionPoint")]
[Id("HullSonar DetectionPoint Component - ID:")] public string s8Id = "cc955716-ca35-4f28-9fde-b81b263a273e";

[Comp("HullSonarPointFeedback Component - ID:")] public string s9Comp = "cc955716-ca35-4f28-9fde-b81b263a273e_2b53fb04-7a36-4328-bfb3-446c9dc848d5";

[Var("appearSound")] [Tooltip("appearSound")] public AudioClip appearSound_cc955716µca35µ4f28µ9fdeµb81b263a273e_2b53fb04µ7a36µ4328µbfb3µ446c9dc848d5;
[Var("appearSoundVolume")] [Tooltip("appearSoundVolume")] public float appearSoundVolume_cc955716µca35µ4f28µ9fdeµb81b263a273e_2b53fb04µ7a36µ4328µbfb3µ446c9dc848d5;

[Header("GameObject: Mad Feedback Object")]
[Id("Mad Feedback Object Component - ID:")] public string s10Id = "140e6581-079e-4773-9248-ad755d901d44";

[Comp("MadFeedback Component - ID:")] public string s11Comp = "140e6581-079e-4773-9248-ad755d901d44_73f3b4cf-18a6-45e8-be66-74a1e3e1492f";

[Var("revealSound")] [Tooltip("revealSound")] public AudioClip revealSound_140e6581µ079eµ4773µ9248µad755d901d44_73f3b4cfµ18a6µ45e8µbe66µ74a1e3e1492f;
[Var("revealSoundVolume")] [Tooltip("revealSoundVolume")] public float revealSoundVolume_140e6581µ079eµ4773µ9248µad755d901d44_73f3b4cfµ18a6µ45e8µbe66µ74a1e3e1492f;

[Header("GameObject: Sonobuoy Instance")]
[Id("Sonobuoy Instance Component - ID:")] public string s12Id = "c0ef1528-81d2-4358-b594-78eba75b8d57";

[Comp("SonobuoyInstanceFeedback Component - ID:")] public string s13Comp = "c0ef1528-81d2-4358-b594-78eba75b8d57_19a2cd0d-67fb-454d-8c46-bf59ceb8ca43";

[Var("detectionSound")] [Tooltip("detectionSound")] public AudioClip detectionSound_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;
[Var("detectionSoundVolume")] [Tooltip("detectionSoundVolume")] public float detectionSoundVolume_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;
[Var("dropSound")] [Tooltip("dropSound")] public AudioClip dropSound_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;
[Var("dropSoundVolume")] [Tooltip("dropSoundVolume")] public float dropSoundVolume_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;
[Var("backgroundSound")] [Tooltip("backgroundSound")] public AudioClip backgroundSound_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;
[Var("backgroundSoundVolume")] [Tooltip("backgroundSoundVolume")] public float backgroundSoundVolume_c0ef1528µ81d2µ4358µb594µ78eba75b8d57_19a2cd0dµ67fbµ454dµ8c46µbf59ceb8ca43;

[Space]
[Path("From")] public string s14_From = "Assets/Prefabs/Assets/Prefabs/Player Ocean Entities";

[Header("GameObject: Helicopter")]
[Id("Helicopter Component - ID:")] public string s15Id = "a5135e0e-b0c8-4871-9721-538499ed3b5c";

[Comp("Helicopter Component - ID:")] public string s16Comp = "a5135e0e-b0c8-4871-9721-538499ed3b5c_4c652d5e-c239-4042-8a74-45e02cce3b57";

[Var("preparationSound")] [Tooltip("preparationSound")] public AudioClip preparationSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("preparationSoundVolume")] [Tooltip("preparationSoundVolume")] public float preparationSoundVolume_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("takeOffSound")] [Tooltip("takeOffSound")] public AudioClip takeOffSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("takeOffSoundVolume")] [Tooltip("takeOffSoundVolume")] public float takeOffSoundVolume_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("landingSound")] [Tooltip("landingSound")] public AudioClip landingSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("landingSoundVolume")] [Tooltip("landingSoundVolume")] public float landingSoundVolume_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("movementSound")] [Tooltip("movementSound")] public AudioClip movementSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("movementSoundVolume")] [Tooltip("movementSoundVolume")] public float movementSoundVolume_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("waitingSound")] [Tooltip("waitingSound")] public AudioClip waitingSound_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;
[Var("waitingSoundVolume")] [Tooltip("waitingSoundVolume")] public float waitingSoundVolume_a5135e0eµb0c8µ4871µ9721µ538499ed3b5c_4c652d5eµc239µ4042µ8a74µ45e02cce3b57;

[Header("GameObject: Plane")]
[Id("Plane Component - ID:")] public string s17Id = "725a0485-6640-4345-aa9d-92857d162799";

[Comp("Plane Component - ID:")] public string s18Comp = "725a0485-6640-4345-aa9d-92857d162799_86e762ea-c3c0-4fde-b405-42694734145d";

[Var("movementSound")] [Tooltip("movementSound")] public AudioClip movementSound_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;
[Var("movementSoundVolume")] [Tooltip("movementSoundVolume")] public float movementSoundVolume_725a0485µ6640µ4345µaa9dµ92857d162799_86e762eaµc3c0µ4fdeµb405µ42694734145d;

[Header("GameObject: Ship")]
[Id("Ship Component - ID:")] public string s19Id = "eaa950de-a907-4c47-8e3b-43fcedc9cef6";

[Comp("Ship Component - ID:")] public string s20Comp = "eaa950de-a907-4c47-8e3b-43fcedc9cef6_d3d3ef92-cbe1-464f-a09f-4e65bb73f4a5";

[Var("waitingSound")] [Tooltip("waitingSound")] public AudioClip waitingSound_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("waitingSoundVolume")] [Tooltip("waitingSoundVolume")] public float waitingSoundVolume_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("movementSound")] [Tooltip("movementSound")] public AudioClip movementSound_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;
[Var("movementSoundVolume")] [Tooltip("movementSoundVolume")] public float movementSoundVolume_eaa950deµa907µ4c47µ8e3bµ43fcedc9cef6_d3d3ef92µcbe1µ464fµa09fµ4e65bb73f4a5;

[Space]
[Path("From")] public string s21_From = "Assets/Prefabs/Assets/Prefabs/Submarine Entities";

[Header("GameObject: Submarine")]
[Id("Submarine Component - ID:")] public string s22Id = "b37c2e93-aa03-4086-988d-7ac4188c2d2c";

[Comp("Submarine Component - ID:")] public string s23Comp = "b37c2e93-aa03-4086-988d-7ac4188c2d2c_bd1bd582-bd2f-42b4-a0c3-15dd959bd187";

[Var("inHackClip")] [Tooltip("inHackClip")] public AudioClip inHackClip_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("doneHackClip")] [Tooltip("doneHackClip")] public AudioClip doneHackClip_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("inHackVolume")] [Tooltip("inHackVolume")] public float inHackVolume_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;
[Var("doneHackVolume")] [Tooltip("doneHackVolume")] public float doneHackVolume_b37c2e93µaa03µ4086µ988dµ7ac4188c2d2c_bd1bd582µbd2fµ42b4µa0c3µ15dd959bd187;

[Space]
[Path("From")] public string s24_From = "Assets/Prefabs/Assets/Prefabs/UI";

[Header("GameObject: PlaneCardDeck")]
[Id("PlaneCardDeck Component - ID:")] public string s25Id = "92aed549-710c-4b52-af1e-49823502f66d";

[Comp("MovementCard Component - ID:")] public string s26Comp = "92aed549-710c-4b52-af1e-49823502f66d_c3952464-85f7-4a59-af45-7f462aab49a8";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_c3952464µ85f7µ4a59µaf45µ7f462aab49a8;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_c3952464µ85f7µ4a59µaf45µ7f462aab49a8;
[Var("cardSelectionSound")] [Tooltip("cardSelectionSound")] public AudioClip cardSelectionSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_c3952464µ85f7µ4a59µaf45µ7f462aab49a8;
[Var("cardSelectionSoundVolume")] [Tooltip("cardSelectionSoundVolume")] public float cardSelectionSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_c3952464µ85f7µ4a59µaf45µ7f462aab49a8;

[Comp("SonobuyDeployerCard Component - ID:")] public string s27Comp = "92aed549-710c-4b52-af1e-49823502f66d_876dc186-4669-4d21-9bc8-471f40405807";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;
[Var("cardSelectionSound")] [Tooltip("cardSelectionSound")] public AudioClip cardSelectionSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;
[Var("cardSelectionSoundVolume")] [Tooltip("cardSelectionSoundVolume")] public float cardSelectionSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;
[Var("outOfChargeSound")] [Tooltip("outOfChargeSound")] public AudioClip outOfChargeSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;
[Var("outOfChargeSoundVolume")] [Tooltip("outOfChargeSoundVolume")] public float outOfChargeSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_876dc186µ4669µ4d21µ9bc8µ471f40405807;

[Comp("PassiveCard Component - ID:")] public string s28Comp = "92aed549-710c-4b52-af1e-49823502f66d_8c6491d7-8c85-4bee-9891-2fd50a02d390";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_92aed549µ710cµ4b52µaf1eµ49823502f66d_8c6491d7µ8c85µ4beeµ9891µ2fd50a02d390;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_92aed549µ710cµ4b52µaf1eµ49823502f66d_8c6491d7µ8c85µ4beeµ9891µ2fd50a02d390;

[Header("GameObject: ShipCardDeck")]
[Id("ShipCardDeck Component - ID:")] public string s29Id = "7ccdb8a7-dab1-4599-94a0-aa84230bb67b";

[Comp("MovementCard Component - ID:")] public string s30Comp = "7ccdb8a7-dab1-4599-94a0-aa84230bb67b_58dd5121-54aa-4c59-9aa7-d4c3a84990ab";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_58dd5121µ54aaµ4c59µ9aa7µd4c3a84990ab;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_58dd5121µ54aaµ4c59µ9aa7µd4c3a84990ab;
[Var("cardSelectionSound")] [Tooltip("cardSelectionSound")] public AudioClip cardSelectionSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_58dd5121µ54aaµ4c59µ9aa7µd4c3a84990ab;
[Var("cardSelectionSoundVolume")] [Tooltip("cardSelectionSoundVolume")] public float cardSelectionSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_58dd5121µ54aaµ4c59µ9aa7µd4c3a84990ab;

[Comp("CaptasCard Component - ID:")] public string s31Comp = "7ccdb8a7-dab1-4599-94a0-aa84230bb67b_c75c7e4d-ad0b-41d2-a90b-b0e5c1c95670";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;
[Var("cardSelectionSound")] [Tooltip("cardSelectionSound")] public AudioClip cardSelectionSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;
[Var("cardSelectionSoundVolume")] [Tooltip("cardSelectionSoundVolume")] public float cardSelectionSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;
[Var("outOfChargeSound")] [Tooltip("outOfChargeSound")] public AudioClip outOfChargeSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;
[Var("outOfChargeSoundVolume")] [Tooltip("outOfChargeSoundVolume")] public float outOfChargeSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_c75c7e4dµad0bµ41d2µa90bµb0e5c1c95670;

[Comp("PassiveCard Component - ID:")] public string s32Comp = "7ccdb8a7-dab1-4599-94a0-aa84230bb67b_4c31bf26-ee57-4dcc-8b0f-c0e1bbb4908b";

[Var("descriptionAppearSound")] [Tooltip("descriptionAppearSound")] public AudioClip descriptionAppearSound_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_4c31bf26µee57µ4dccµ8b0fµc0e1bbb4908b;
[Var("descriptionAppearSoundVolume")] [Tooltip("descriptionAppearSoundVolume")] public float descriptionAppearSoundVolume_7ccdb8a7µdab1µ4599µ94a0µaa84230bb67b_4c31bf26µee57µ4dccµ8b0fµc0e1bbb4908b;

}
