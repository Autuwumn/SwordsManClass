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
    public class BigerSword : SimpleCard
    {
        internal static CardInfo card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = SwordClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Bigger Sword",
            Description = "Why what a big sword you have",
            ModName = SMC.ModInitials,
            Art = SMC.ArtAssets.LoadAsset<GameObject>("C_Longsword"),
            Rarity = CardInfo.Rarity.Uncommon,
            Theme = CardThemeColor.CardThemeColorType.TechWhite,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "+1",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.Some,
                    stat = "Swordsize"
                }
            }
        };
    }
}
