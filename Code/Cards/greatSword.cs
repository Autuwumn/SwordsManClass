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
    public class GreatSword : SimpleCard
    {
        internal static CardInfo card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = SwordClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Greatsword",
            Description = "Big Sword",
            ModName = SMC.ModInitials,
            Art = SMC.ArtAssets.LoadAsset<GameObject>("C_Greatsword"),
            Rarity = RarityUtils.GetRarity("Legendary"),
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "+100%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf,
                    stat = "Damage"
                },
                new CardInfoStat
                {
                    amount = "x2",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.aHugeAmountOf,
                    stat = "Swordsize"
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.damage = 2f;
        }
    }
}
