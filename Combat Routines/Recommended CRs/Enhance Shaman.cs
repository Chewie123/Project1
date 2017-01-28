using ZzukBot.Engines.CustomClass;

namespace CasinoFury2
{
    public class CasinoFury2 : CustomClass
    {
        public override byte DesignedForClass
        {
            get
            {
                return PlayerClass.Shaman;
            }
        }
        public override string CustomClassName
        {
            get
            {
                return "CasinoFury 2.2.0";
            }
        }
        public string[] drinkNames =
        {
            "Refreshing Spring Water", "Ice Cold Milk",
            "Melon Juice", "Moonberry Juice",
            "Sweet Nectar", "Morning Glory Dew"
        };
        private string[] conjuredNames = 
        {
            "Conjured Water", "Conjured Fresh Water",
            "Conjured Purified Water", "Conjured Spring Water",
            "Conjured Mineral Water", "Conjured Sparkling Water",
            "Conjured Crystal Water"
        };
        public void SelectDrink()
        {
            if (this.Player.ItemCount("Conjured Crystal Water") != 0)
                this.Player.Drink(conjuredNames[6]);
            else if (this.Player.ItemCount("Conjured Sparkling Water") != 0)
                this.Player.Drink(conjuredNames[5]);
            else if (this.Player.ItemCount("Morning Glory Dew") != 0)
                this.Player.Drink(drinkNames[5]);
            else if (this.Player.ItemCount("Conjured Mineral Water") != 0)
                this.Player.Drink(conjuredNames[4]);
            else if (this.Player.ItemCount("Sweet Nectar") != 0)
                this.Player.Drink(drinkNames[4]);
            else if (this.Player.ItemCount("Conjured Spring Water") != 0)
                this.Player.Drink(conjuredNames[3]);
            else if (this.Player.ItemCount("Moonberry Juice") != 0)
                this.Player.Drink(drinkNames[3]);
            else if (this.Player.ItemCount("Conjured Purified Water") != 0)
                this.Player.Drink(conjuredNames[2]);
            else if (this.Player.ItemCount("Melon Juice") != 0)
                this.Player.Drink(drinkNames[2]);
            else if (this.Player.ItemCount("Conjured Fresh Water") != 0)
                this.Player.Drink(conjuredNames[1]);
            else if (this.Player.ItemCount("Ice Cold Milk") != 0)
                this.Player.Drink(drinkNames[1]);
            else if (this.Player.ItemCount("Conjured Water") != 0)
                this.Player.Drink(conjuredNames[0]);
            else if (this.Player.ItemCount("Refreshing Spring Water") != 0)
                this.Player.Drink(drinkNames[0]);
        }
        public void DrinkPotion()
        {
            if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Major Healing Potion") != 0)
                this.Player.UseItem("Major Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Superior Healing Potion") != 0)
                this.Player.UseItem("Superior Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Greater Healing Potion") != 0)
                this.Player.UseItem("Greater Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Healing Potion") != 0)
                this.Player.UseItem("Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Discolored Healing Potion") != 0)
                this.Player.UseItem("Discolored Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Lesser Healing Potion") != 0)
                this.Player.UseItem("Lesser Healing Potion");
            else if (this.Player.HealthPercent <= 20 && this.Player.ItemCount("Minor Healing Potion") != 0)
                this.Player.UseItem("Minor Healing Potion");
        }
        public void EnchantWeapon()
        {
            if (this.Player.GetSpellRank("Windfury Weapon") != 0)
            {
                if (!this.Player.IsMainhandEnchanted())
                    this.Player.Cast("Windfury Weapon");
            }
            else if (this.Player.GetSpellRank("Flametongue Weapon") != 0)
            {
                if (!this.Player.IsMainhandEnchanted())
                    this.Player.Cast("Flametongue Weapon");
            }
            else if (this.Player.GetSpellRank("Rockbiter Weapon") != 0)
            {
                if (!this.Player.IsMainhandEnchanted())
                    this.Player.Cast("Rockbiter Weapon");
            }
        }
        public void Shield()
        {
            if (this.Player.GetSpellRank("Lightning Shield") != 0 && !this.Player.GotBuff("Lightning Shield") && this.Player.ManaPercent >= 50 && this.Player.CanUse("Lightning Shield") && this.Player.GetSpellRank("Searing Totem") == 0)
                this.Player.Cast("Lightning Shield");
        }
        public void RangePull()
        {
            this.SetCombatDistance(29);
            if (this.Player.ManaPercent >= 33)
                this.Player.CastWait("Lightning Bolt", 5000);
        }
        public void Shock()
        {
            if (this.Player.GetSpellRank("Stormstrike") == 0)
            {
                if (this.Player.GetSpellRank("Flame Shock") != 0 && !this.Target.GotDebuff("Flame Shock") && this.Target.HealthPercent >= 40 && this.Player.ManaPercent >= 45 && this.Player.CanUse("Flame Shock"))
                    this.Player.Cast("Flame Shock");
                if (this.Player.GetSpellRank("Earth Shock") != 0 && this.Target.HealthPercent >= 15 && this.Player.ManaPercent >= 45 && this.Player.CanUse("Earth Shock"))
                    this.Player.Cast("Earth Shock");
            }                
            else if (this.Player.GetSpellRank("Stormstrike") != 0)
            {
                if (this.Player.CanUse("Stormstrike") && this.Target.HealthPercent >= 35 && this.Player.ManaPercent >= 35)
                    this.Player.Cast("Stormstrike");
                if (this.Player.GetSpellRank("Earth Shock") != 0 && this.Target.HealthPercent >= 15 && this.Player.ManaPercent >= 50 && this.Player.CanUse("Earth Shock") && this.Target.GotDebuff("Stormstrike"))
                    this.Player.Cast("Earth Shock");
            }
        }
        public void Totems()
        {
            if (this.Player.GetSpellRank("Grace of Air Totem") != 0 && this.Player.CanUse("Grace of Air Totem") && this.Player.ManaPercent >= 50 && !this.Player.GotBuff("Grace of Air") && this.Attackers.Count < 2)
                this.Player.Cast("Grace of Air Totem");
            else if (this.Player.GetSpellRank("Strength of Earth Totem") != 0 && this.Player.CanUse("Strength of Earth Totem") && this.Player.ManaPercent >= 50 && !this.Player.GotBuff("Strength of Earth") && this.Player.GetSpellRank("Grace of Air Totem") == 0 && this.Attackers.Count < 2)
            {
                float soeRange = this.Player.IsTotemSpawned("Strength of Earth Totem");
                float stoneclawRange = this.Player.IsTotemSpawned("Stoneclaw Totem");
                if ((soeRange == -1 || soeRange > 18) && (stoneclawRange == -1 || stoneclawRange > 18))
                    this.Player.Cast("Strength of Earth Totem");
            }
            if (this.Player.GetSpellRank("Mana Spring Totem") != 0 && this.Player.CanUse("Mana Spring Totem") && !this.Player.GotBuff("Mana Spring") && this.Player.ManaPercent <= 50 && this.Attackers.Count < 2)
            {
                float manaSpringRange = this.Player.IsTotemSpawned("Mana Spring Totem");
                if ((manaSpringRange == -1 || manaSpringRange > 18))
                    this.Player.Cast("Mana Spring Totem");
            }
            if (this.Player.GetSpellRank("Searing Totem") != 0 && this.Target.HealthPercent >= 40 && this.Player.ManaPercent >= 50 && this.Player.CanUse("Searing Totem") && this.Attackers.Count < 2)
            {
                float searingRange = this.Player.IsTotemSpawned("Searing Totem");
                float fireNovaRange = this.Player.IsTotemSpawned("Fire Nova Totem");
                if ((searingRange == -1 || searingRange > 18) && (fireNovaRange == -1 || fireNovaRange > 18))
                    this.Player.Cast("Searing Totem");
            }
        }
        public void FightHeal()
        {
            if (this.Player.HealthPercent < 25 && this.Player.CanUse("Lesser Healing Wave")&& this.Player.ManaPercent >= 10)
            {
                this.Player.TryCast("War Stomp");
                this.Player.CastWait("Lesser Healing Wave", 1000);
            }
            else if (this.Player.HealthPercent < 50 && this.Player.CanUse("Healing Wave") && this.Player.ManaPercent >= 20)
            {
                this.Player.TryCast("War Stomp");
                this.Player.CastWait("Healing Wave", 1000);
            }
            return;
        }
        public void HandleAdds()
        {
            if (this.Player.GetSpellRank("Stoneclaw Totem") != 0 && this.Attackers.Count >= 2 && this.Player.CanUse("Stoneclaw Totem") && this.Player.ManaPercent >= 30)           
                this.Player.Cast("Stoneclaw Totem");
            if (this.Player.GetSpellRank("Fire Nova Totem") != 0 && this.Attackers.Count >= 2 && this.Player.CanUse("Fire Nova Totem") && this.Player.ManaPercent >= 50)
                this.Player.Cast("Fire Nova Totem");
        }
        public override bool Buff()
        {
            EnchantWeapon();
            Shield();
            return true;
        }
        public override void PreFight()
        {
            RangePull();
        }
        public override void Fight()
        {
            this.Player.Attack();
            Shock();
            DrinkPotion();
            HandleAdds();
            FightHeal();
            Buff();
            Totems();
        }
        public override void Rest()
        {   
            if (this.Player.HealthPercent <= 80 && this.Player.CanUse("Healing Wave") && !this.Player.GotBuff("Drink") && this.Player.ManaPercent >= 25)
                this.Player.CastWait("Healing Wave", 1000);
            else if (this.Player.HealthPercent <= 80 && this.Player.CanUse("Lesser Healing Wave") && !this.Player.GotBuff("Drink") && this.Player.ManaPercent >= 15)
                this.Player.CastWait("Lesser Healing Wave", 1000);
            if (this.Player.ManaPercent <= 25 && !this.Player.GotBuff("Drink"))
            {
                SelectDrink();
                return;
            }
        }
    }
}