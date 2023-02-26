using ClassesManagerReborn;
using System.Collections;

namespace SMC.Cards
{
    class SwordClass : ClassHandler
    {
        internal static string name = "Swordsman";

        public override IEnumerator Init()
        {
            while (!(SwordClassCard.card && BigerSword.card && ElectroSword.card && KnockSword.card && LongerSword.card && ShortSword.card && GreatSword.card && Dagger.card)) yield return null;
            ClassesRegistry.Register(SwordClassCard.card, CardType.Entry);
            ClassesRegistry.Register(BigerSword.card, CardType.Card, SwordClassCard.card, 5);
            ClassesRegistry.Register(ElectroSword.card, CardType.Card, new[] { SwordClassCard.card, BigerSword.card }, 1);
            ClassesRegistry.Register(KnockSword.card, CardType.Card, new[] { SwordClassCard.card, BigerSword.card}, 1);
            ClassesRegistry.Register(LongerSword.card, CardType.SubClass, new[] { SwordClassCard.card, BigerSword.card, BigerSword.card, BigerSword.card }, 1);
            ClassesRegistry.Register(ShortSword.card, CardType.SubClass, new[] { SwordClassCard.card, BigerSword.card, BigerSword.card, BigerSword.card }, 1);
            ClassesRegistry.Register(GreatSword.card, CardType.Card, LongerSword.card, 1);
            ClassesRegistry.Register(Dagger.card, CardType.Card, ShortSword.card, 1);
        }
    }
}