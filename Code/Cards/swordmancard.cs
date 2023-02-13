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
            Rarity = RarityUtils.GetRarity("Rare"),
            Theme = CardThemeColor.CardThemeColorType.DestructiveRed
        };
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
        }
        protected override void Added(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            SMC.swordLength = 2;
            SMC.swordWidth = 0.5f;
        }
    }
}

namespace SMC.SwordScripts
{
    public class SwordCard : CardEffect
    {
        private GameObject Hilt;
        private GameObject[] Segment;
        private GameObject Tip;
        private float damageMult;
        public override void OnShoot(GameObject projectile)
        {
            PhotonNetwork.Destroy(projectile);
        }
        private void SpawnSword()
        {
            if (!player.data.view.IsMine) return;
            damageMult = gun.damage*gun.projectileSpeed/gunAmmo.reloadTime*gunAmmo.maxAmmo/3f*55f;
            Hilt = PhotonNetwork.Instantiate("SMC_SwordHilt", player.data.hand.position,Quaternion.identity);
            Hilt.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(SMC.swordWidth,SMC.swordWidth,SMC.swordWidth);
            Tip = PhotonNetwork.Instantiate("SMC_SwordTip", player.data.hand.position, Quaternion.identity);
            Tip.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(SMC.swordWidth, SMC.swordWidth, SMC.swordWidth);
            Tip.GetComponent<DamageBox>().damage = damageMult;
            if (SMC.stun) Tip.GetComponent<DamageBox>().setFlyingFor = gun.attackSpeed*0.75f;
            if (SMC.knock) Tip.GetComponent<DamageBox>().force = 10000f;
            Segment = new GameObject[SMC.swordLength];
            for (var i = 0; i < SMC.swordLength; i++)
            {
                Segment[i] = PhotonNetwork.Instantiate("SMC_SwordSegment", player.data.hand.position, Quaternion.identity);
                Segment[i].GetComponent<Rigidbody2D>().transform.localScale = new Vector3(SMC.swordWidth, SMC.swordWidth, SMC.swordWidth);
                Segment[i].GetComponent<DamageBox>().setFlyingFor = 0f;
                Segment[i].GetComponent<DamageBox>().force = 0f;
                Segment[i].GetComponent<DamageBox>().damage = damageMult;
                Segment[i].GetComponent<DamageBox>().cd = gun.attackSpeed * 2f;
                if (SMC.stun) Segment[i].GetComponent<DamageBox>().setFlyingFor = gun.attackSpeed*1.5f;
                if (SMC.knock) Segment[i].GetComponent<DamageBox>().force = 10000f;
            }
            var polli = Segment[0].gameObject.GetComponent<Collider2D>();
            foreach (var colli in player.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(polli, colli);
            }
        }
        private void Update()
        {
            if (!Hilt) return;
            Hilt.GetComponent<Rigidbody2D>().position = player.data.hand.position;
            Hilt.GetComponent<Rigidbody2D>().transform.up = -player.data.aimDirection;
            for(var i = 0; i < Segment.Length; i++)
            {
                var item = Segment[i].GetComponent<Rigidbody2D>();
                item.position = player.data.hand.position + (player.data.aimDirection.normalized * 1.7f * SMC.swordWidth * (i+1));
                item.transform.up = -player.data.aimDirection;
            }
            Tip.GetComponent<Rigidbody2D>().position = player.data.hand.position+ (player.data.aimDirection.normalized * 1.7f * SMC.swordWidth * (SMC.swordLength+1));
            Tip.GetComponent<Rigidbody2D>().transform.up = -player.data.aimDirection;
        }
        private void DestroySword()
        {
            PhotonNetwork.Destroy(Hilt);
            PhotonNetwork.Destroy(Tip);
            foreach (var item in Segment)
            {
                PhotonNetwork.Destroy(item);
            }
        }
        public override void OnJump()
        {
            base.OnJump();
            if(!Hilt) SpawnSword();
        }
        public override IEnumerator OnPointStart(IGameModeHandler gameModeHandler)
        {
            SpawnSword();
            return base.OnPointStart(gameModeHandler);
        }
        public override IEnumerator OnPointEnd(IGameModeHandler gameModeHandler)
        {
            DestroySword();
            return base.OnPointEnd(gameModeHandler);
        }
    }
}