using System.Reflection;
using UnityEngine;
using RarityLib.Utils;
using ModsPlus;
using UnboundLib;
using SMC.Cards;
using System.Linq;
using System.Collections.Generic;
using System;
using ClassesManagerReborn.Util;
using SMC.SwordScripts;

namespace SMC.Cards
{
    public class ShortSword : SimpleCard
    {
        internal static CardInfo card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = SwordClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Shortsword",
            Description = "Smaller sword but more attack speed",
            ModName = SMC.ModInitials,
            Art = SMC.ArtAssets.LoadAsset<GameObject>("C_Shortersword"),
            Rarity = CardInfo.Rarity.Rare,
            Theme = CardThemeColor.CardThemeColorType.TechWhite,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "-100%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf,
                    stat = "Attackspeed"
                },
                new CardInfoStat
                {
                    amount = "-2",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.slightlySmaller,
                    stat = "Swordsize"
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.attackSpeed = 0.5f;
        }
    }
}
