    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ZzukBot.Engines.CustomClass;
    using ZzukBot.Engines.CustomClass.Objects;



    namespace something
    {
        public class WandTo60 : CustomClass
        {
            public override byte DesignedForClass
            {
                get
                {
                    return PlayerClass.Priest;
                }
            }
            public override string CustomClassName
            {
                get
                {
                    return "WandTo60";
                }
            }
          public override void Rest()
          {
             if (this.Player.HealthPercent <= 20)
             {
                this.Player.Cast("Greater Heal");
             }
             else if (this.Player.HealthPercent <= 85)
             {
                this.Player.Cast("Lesser Heal");
             }
                if (this.Player.ManaPercent < 60)
                {
                    this.Player.Drink();
                }
          }
          public override void PreFight()
          {
             this.SetCombatDistance(30);
             
             if (!this.Target.GotDebuff("Shadow Word: Pain"))
             {
                this.Player.Cast("Shadow Word: Pain");
             }
          }
          public override void Fight()
          {
             if (this.Player.HealthPercent <= 40)
             {
                this.Player.StopWand();
                if (this.Player.CanUse("Flash Heal"))
                {
                   this.Player.Cast("Flash Heal");
                }
                else
                {
                   this.Player.Cast("Lesser Heal");
                }
             }
             if (!this.Player.GotBuff("Power Word: Shield") && !this.Player.GotDebuff("Weakened Soul"))
             {
                this.Player.Cast("Power Word: Shield");
             }
              if (!this.Player.GotBuff("Renew") && this.Player.CanUse("Renew"))
             {
                this.Player.Cast("Renew");
             }
             
             if (!this.Target.GotDebuff("Shadow Word: Pain"))
             {
                this.Player.Cast("Shadow Word: Pain");
             }
             
             this.Player.StartWand();         
          }
          public override bool Buff()
           {
             if (!this.Player.GotBuff("Power Word: Fortitude") && this.Player.CanUse("Power Word: Fortitude"))
             {
                this.Player.Cast("Power Word: Fortitude");
                return false;
             }
             if (!this.Player.GotBuff("Inner Fire") && this.Player.CanUse("Inner Fire"))
             {
                this.Player.Cast("Inner Fire");
                return false;
             }
             return true;
          }
       }
    }
