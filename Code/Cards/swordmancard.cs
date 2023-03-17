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
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon.StructWrapping;
using UnboundLib.Networking;
using System.Security.Cryptography.X509Certificates;

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
    }
}

namespace SMC.SwordScripts
{
    public class SwordCard : CardEffect
    {
        private SwordHandler theSrowd;
        protected override void Start()
        {
            var swo = PhotonNetwork.Instantiate("SMC_Sword", player.data.hand.position, Quaternion.identity);
            print(swo.name + ", " + nameof(swo));
            SMC.instance.ExecuteAfterFrames(20, () =>
            {
                NetworkingManager.RPC(typeof(SwordCard), nameof(RPC_SwordWork), swo.name, player.playerID);
            });
            theSrowd = swo.GetComponent<SwordHandler>();
            FixSword();
            //var swo = Instantiate(SMC.ArtAssets.LoadAsset<GameObject>("leStabber"));
            //theSrowd = swo.GetOrAddComponent<SwordHandler>();
            //theSrowd.owner = player;
        }
        [UnboundRPC]
        public static void RPC_SwordWork(string swordName, int playerid)
        {
            var obejcts = UnityEngine.GameObject.FindGameObjectsWithTag("Bullet");
            List<GameObject> srowds = new List<GameObject>();
            foreach(var obj in obejcts)
            {
                if(obj.name == swordName)
                {
                    srowds.Add(obj);
                }
            }
            foreach(var swo in srowds)
            {
                if(swo.GetComponent<SwordHandler>() == null)
                {
                    swo.AddComponent<SwordHandler>().owner = PlayerManager.instance.players.Where((p) => p.playerID == playerid).ToArray()[0];
                }
            }
            //UnityEngine.GameObject.Find(swordName).AddComponent<SwordHandler>().owner = PlayerManager.instance.players.Where((p) => p.playerID == playerid).ToArray()[0];
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
        private Rigidbody2D rigid;
        private DamageBox damagebox;

        public void Start()
        {
            rigid = gameObject.GetComponent<Rigidbody2D>();
            damagebox = gameObject.GetComponentInChildren<DamageBox>();
        }
        public void Update()
        {
            var poli = gameObject.GetComponentInChildren<PolygonCollider2D>();
            foreach (var colli in owner.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(poli, colli);
            }
            var size = 0.6f;
            var stun = false;
            var knock = false;
            foreach (var card in owner.data.currentCards)
            {
                switch(card.cardName.ToLower())
                {
                    case "bigger sword": size += 0.2f; break;
                    case "shortsword": size -= 0.4f; break;
                    case "longsword": size += 0.4f; break;
                    case "electro sword": stun = true; break;
                    case "knockback sword": knock = true; break;
                }
            }
            foreach(var card in owner.data.currentCards)
            {
                switch(card.cardName.ToLower())
                {
                    case "dagger": size /= 2; break;
                    case "greatsword": size *= 2; break;
                }
            }
            rigid.transform.localScale = new Vector3(size, size, size);
            rigid.position = owner.data.hand.position;
            rigid.transform.up = -owner.data.aimDirection;
            damagebox.damage = owner.data.weaponHandler.gun.damage * 55f;
            damagebox.cd = owner.data.weaponHandler.gun.attackSpeed*2;
            if (!knock) damagebox.force = 0;
            if (!stun) damagebox.setFlyingFor = 0;
            if (knock) damagebox.force = 5000;
            if (stun) damagebox.setFlyingFor = 1;
            if(owner.data.health <= 0)
            {
                rigid.position = new Vector3(1000, 1000, 0);
            }
        }
    }
}