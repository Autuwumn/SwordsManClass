using BepInEx;
using HarmonyLib;
using SMC.Cards;
using UnboundLib.Cards;
using Photon.Pun;
using Jotunn.Utils;
using UnityEngine;
using ModsPlus;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using BepInEx.Configuration;
using UnboundLib.Utils.UI;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;

namespace SMC
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.rarity.lib", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.CrazyCoders.Rounds.RarityBundle", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willis.rounds.modsplus", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class SMC : BaseUnityPlugin
    {
        private const string ModId = "koala.swordsman.class";
        private const string ModName = "Swordsman Class";
        public const string Version = "1.0.7";
        public const string ModInitials = "SMC";

        public static int swordLength = 2;
        public static float swordWidth = 0.5f;
        public static bool stun = false;
        public static bool knock = false;

        internal static SMC instance;

        internal static AssetBundle ArtAssets;

        void Start()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
            instance = this;

            SMC.ArtAssets = AssetUtils.LoadAssetBundleFromResources("swordcards", typeof(SMC).Assembly);

            if (SMC.ArtAssets == null)
            {
                UnityEngine.Debug.Log("Chad Vanilla art asset bundle either doesn't exist or failed to load.");
            }
            PhotonNetwork.PrefabPool.RegisterPrefab("SMC_SwordHilt", ArtAssets.LoadAsset<GameObject>("srowdHilt"));
            PhotonNetwork.PrefabPool.RegisterPrefab("SMC_SwordSegment", ArtAssets.LoadAsset<GameObject>("srowdMiddle"));
            PhotonNetwork.PrefabPool.RegisterPrefab("SMC_SwordTip", ArtAssets.LoadAsset<GameObject>("srowdEnd"));


            CustomCard.BuildCard<SwordClassCard>((card) => { SwordClassCard.card = card; card.SetAbbreviation("Sc"); });
            CustomCard.BuildCard<BigerSword>((card) => { BigerSword.card = card; card.SetAbbreviation("Bs"); });
            CustomCard.BuildCard<ElectroSword>((card) => { ElectroSword.card = card; card.SetAbbreviation("Es"); });
            CustomCard.BuildCard<KnockSword>((card) => { KnockSword.card = card; card.SetAbbreviation("Ks"); });
            CustomCard.BuildCard<LongerSword>((card) => { LongerSword.card = card; card.SetAbbreviation("Ls"); });
            CustomCard.BuildCard<ShortSword>((card) => { ShortSword.card = card; card.SetAbbreviation("Ss"); });
            CustomCard.BuildCard<GreatSword>((card) => { GreatSword.card = card; card.SetAbbreviation("Gs"); });
            CustomCard.BuildCard<Dagger>((card) => { Dagger.card = card; card.SetAbbreviation("Da"); });
        }
        public static bool Debug = false;
    }
}