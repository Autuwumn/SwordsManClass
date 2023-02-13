using ClassesManagerReborn;
using System.Collections;

namespace SMC.Cards
{
    class SwordClass : ClassHandler
    {
        internal static string name = "Swordsman";

        public override IEnumerator Init()
        {
            while (!(SwordClassCard.card)) yield return null;
            ClassesRegistry.Register(SwordClassCard.card, CardType.Entry);
            ClassesRegistry.Register(BigerSword.card, CardType.Card, SwordClassCard.card, 5);
            ClassesRegistry.Register(ElectroSword.card, CardType.Card, SwordClassCard.card, 1);
            ClassesRegistry.Register(KnockSword.card, CardType.Card, SwordClassCard.card, 1);
        }
    }
}