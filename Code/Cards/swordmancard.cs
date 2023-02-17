using System.Reflection;
using UnityEngine;
using RarityLib.Utils;
using ModsPlus;
using UnboundLib;
using SMC.Cards;
using SMC.SwordScripts;
using System.Linq;
using System.Collections.Generic;
using System;
using ClassesManagerReborn.Util;
using Photon.Pun;
using System.Collections;
using UnboundLib.GameModes;
using Photon.Realtime;
using System.Xml.Schema;

namespace SMC.Cards
{
    public class SwordClassCard : CustomEffectCard<SwordCard>
    {
        internal static CardInfo card = null;
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>();
        }
        public override CardDetails Details => new CardDetails
        {
            Title = "Swordman Class",
            Description = "Sheathe your gun and draw a sword, all gun stats are combined and used for damage",
            ModName = SMC.ModInitials,
            Art = SMC.ArtAssets.LoadAsset<GameObject>("C_SwordmanClass"),
            Rarity = CardInfo.Rarity.Uncommon,
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed,
            Stats = new[]
            {
                new CardInfoStat
                {
                    amount = "+3",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.aLotOf,
                    stat = "Swordsize"
                }
            },
            OwnerOnly = true
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            gun.destroyBulletAfter = 0.001f;
            gun.projectileSize = 0.001f;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var sdt = player.gameObject.GetOrAddComponent<SwordDataTracker>();
            sdt.size += 0.6f;
        }
    }
}

namespace SMC.SwordScripts
{
    public class SwordCard : CardEffect
    {
        private SwordHandler theSrowd;
        protected override void Start()
        {
            base.Start();
            var sh = PhotonNetwork.Instantiate("SMC_Sword", player.data.hand.position, Quaternion.identity);
            theSrowd = sh.AddComponent<SwordHandler>();
            theSrowd.owner = player;
            FixSword();
        }
        private void FixSword()
        {
            var poli = theSrowd.gameObject.GetComponentInChildren<PolygonCollider2D>();
            foreach (var colli in player.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(poli, colli);
            }
        }
        public override IEnumerator OnPointStart(IGameModeHandler gameModeHandler)
        {
            FixSword();
            yield break;
        }
        public override void OnRevive()
        {
            FixSword();
            base.OnRevive();
        }
        protected override void OnDestroy()
        {
            PhotonNetwork.Destroy(theSrowd.gameObject);
            base.OnDestroy();
        }
    }
    public class SwordHandler : MonoBehaviour
    {
        public Player owner;
        private SwordDataTracker stats;
        private Rigidbody2D rigid;
        private DamageBox damagebox;

        public void Start()
        {
            rigid = gameObject.GetComponent<Rigidbody2D>();
            damagebox = gameObject.GetComponentInChildren<DamageBox>();
            stats = owner.gameObject.GetComponent<SwordDataTracker>();
        }
        public void Update()
        {
            rigid.transform.localScale = new Vector3(stats.size, stats.size, stats.size);
            rigid.position = owner.data.hand.position;
            rigid.transform.up = -owner.data.aimDirection;
            damagebox.damage = owner.data.weaponHandler.gun.damage * 55f;
            damagebox.cd = owner.data.weaponHandler.gun.attackSpeed;
            if (!stats.knock) damagebox.force = 0;
            if (!stats.stun) damagebox.setFlyingFor = 0;
            if (stats.knock) damagebox.force = 5000;
            if (stats.stun) damagebox.setFlyingFor = 1;
        }
    }
    public class SwordDataTracker : MonoBehaviour
    {
        public float size = 0.6f;
        public bool knock = false;
        public bool stun = false;
    }
}