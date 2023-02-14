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

namespace SMC.Cards
{
    public class Dagger : SimpleCard
    {
        internal static CardInfo card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = SwordClass.name;
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Dagger",
            Description = "Do the stab",
            ModName = SMC.ModInitials,
            Art = SMC.ArtAssets.LoadAsset<GameObject>("C_Dagger"),
            Rarity = RarityUtils.GetRarity("Legendary"),
            Theme = CardThemeColor.CardThemeColorType.DefensiveBlue,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "-100%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Size"
                },
                new CardInfoStat
                {
                    amount = "+100%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Movement speed"
                },
                new CardInfoStat
                {
                    amount = "-400%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Attack Speed"
                }
            }
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.attackSpeed = 0.2f;
            statModifiers.movementSpeed = 2f;
            statModifiers.sizeMultiplier = 0.5f;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (!player.data.view.IsMine) return;
            SMC.swordWidth -= 0.5f;
        }
        protected override void Removed(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (!player.data.view.IsMine) return;
            SMC.swordWidth -= 0.5f;
        }
    }
}
