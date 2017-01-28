using ZzukBot.Engines.CustomClass;

namespace StarsAlign4
{
    public class StarsAlign4 : CustomClass
    {
        public override byte DesignedForClass
        {
            get
            {
                return PlayerClass.Druid;
            }
        }
        public override string CustomClassName
        {
            get
            {
                return "StarsAlign 4.2.4";
            }
        }
        public int Tracking = 0;
        public bool HideTracker;
        public void SetTracker()
        {
            while (Tracking < 1)
            {
                this.Player.DoString("StartXP = UnitXP('player');  StartTime = GetTime()");
                Tracking ++;
            }
        }
        public void DisplayTracker()
        {
            while (HideTracker == false)
            {
                this.Player.DoString("GainedXP = UnitXP('player') - StartXP; ElapsedTime = GetTime() - StartTime");
                this.Player.DoString("DEFAULT_CHAT_FRAME:AddMessage(GainedXP / ElapsedTime * 3600)");
                HideTracker = true;
            }
        }
        public string[] drinkNames = {"Refreshing Spring Water", "Ice Cold Milk",
            "Melon Juice", "Moonberry Juice",
            "Sweet Nectar", "Morning Glory Dew"};
        public void SelectDrink()
        {
            if (this.Player.ItemCount("Morning Glory Dew") != 0)
                this.Player.Drink(drinkNames[5]);
            else if (this.Player.ItemCount("Sweet Nectar") != 0)
                this.Player.Drink(drinkNames[4]);
            else if (this.Player.ItemCount("Moonberry Juice") != 0)
                this.Player.Drink(drinkNames[3]);
            else if (this.Player.ItemCount("Melon Juice") != 0)
                this.Player.Drink(drinkNames[2]);
            else if (this.Player.ItemCount("Ice Cold Milk") != 0)
                this.Player.Drink(drinkNames[1]);
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
        public void MoonkinShift()
        {
            if (!this.Player.GotBuff("Moonkin Form") && this.Player.GetSpellRank("Moonkin Form") != 0 && this.Player.IsCasting != "Healing Touch" && this.Player.ManaPercent >= 50)
                this.Player.TryCast("Moonkin Form");
        }
        public void BuffMark()
        {
            if (!this.Player.GotBuff("Mark of the Wild") && this.Player.GetSpellRank("Mark of the Wild") != 0)
                this.Player.TryCast("Mark of the Wild");
        }
        public void BuffThorns()
        {
            if (!this.Player.GotBuff("Thorns") && this.Player.GetSpellRank("Thorns") != 0)
                this.Player.TryCast("Thorns");
        }
        public void BuffGrasp()
        {
            if (!this.Player.GotBuff("Nature's Grasp") && this.Player.GetSpellRank("Nature's Grasp") != 0 && this.Player.CanUse("Nature's Grasp") && this.Player.ManaPercent >= 50)
                this.Player.TryCast("Nature's Grasp");
        }
        public void RangePull()
        {
            this.SetCombatDistance(29);
            if (this.Player.GetSpellRank("Moonfire") != 0 && this.Player.CanUse("Moonfire"))
                this.Player.TryCast("Moonfire");
            else if (this.Player.CanUse("Wrath"))
                    this.Player.Cast("Wrath");
        }
        public void CheckRoot()
        {
            if (this.Target.GotDebuff("Entangling Roots") && this.Target.DistanceToPlayer <= 10)
                this.Player.ForceBackup(8);
            else
                this.Player.StopForceBackup();
        }
        public void FightHeal()
        {
            DrinkPotion();
            if (this.Player.HealthPercent < 50 && this.Player.CanUse("Healing Touch"))
            {
                if (this.Player.IsCasting == "Wrath" || this.Player.IsCasting == "Starfire")
                    this.Player.StopCasting();
                this.Player.CancelShapeShift();
                this.Player.TryCast("War Stomp");
                this.Player.CastWait("Healing Touch", 1000);
            }
        }
        public void FightRegen()
        {
            if (this.Player.ManaPercent < 50 && this.Player.CanUse("Innervate"))
            {
                this.Player.CancelShapeShift();
                this.Player.TryCast("Innervate");
            }
            if (this.Player.ManaPercent < 50 && !this.Player.CanUse("Innervate"))
                this.Player.Attack();
        }
        public void RestHeal()
        {
            if (this.Player.HealthPercent <= 60 && this.Player.ManaPercent >= 15 && !this.Player.GotBuff("Drink") && this.Player.CanUse("Healing Touch"))
            {
                this.Player.CancelShapeShift();
                this.Player.CastWait("Healing Touch", 1000);
            }
            else if (this.Player.HealthPercent >= 61 && this.Player.HealthPercent <= 89 && this.Player.ManaPercent >= 15 && !this.Player.GotBuff("Rejuvenation") && !this.Player.GotBuff("Drink") && this.Player.CanUse("Rejuvenation"))
            {
                this.Player.CancelShapeShift();
                this.Player.CastWait("Rejuvenation", 1000);
            }
        }
        public void RestRegen()
        {
            if (!this.Player.GotBuff("Drink") && this.Player.ManaPercent <= 50)
                SelectDrink();
            if (!this.Player.GotBuff("Shadowmeld") && this.Player.GotBuff("Drink"))
                this.Player.TryCast("Shadowmeld");
        }
        public void Nuke()
        {
            if (this.Player.HealthPercent >= 50 && this.Player.ManaPercent >= 25 && this.Player.GetSpellRank("Moonfire") != 0 && !this.Target.GotDebuff("Moonfire") && this.Player.CanUse("Moonfire"))
                this.Player.TryCast("Moonfire");
            if (this.Player.HealthPercent >= 50 && this.Player.ManaPercent >= 25 && this.Player.GetSpellRank("Starfire") != 0 && this.Player.CanUse("Starfire"))
                this.Player.TryCast("Starfire");
            else if (this.Player.HealthPercent >= 50 && this.Player.ManaPercent >= 25 && this.Player.CanUse("Wrath"))
                this.Player.Cast("Wrath");
        }
        public override bool Buff()
        {
            MoonkinShift();
            BuffMark();
            BuffThorns();
            BuffGrasp();
            SetTracker();
            HideTracker = false;
            return true;
        }
        public override void PreFight()
        {
            RangePull();
        }
        public override void Fight()
        {
            Buff();
            CheckRoot();
            FightHeal();
            FightRegen();
            Nuke();
        }
        public override void Rest()
        {
            DisplayTracker();
            RestHeal();
            RestRegen();
        }
    }
}