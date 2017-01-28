    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using ZzukBot.Engines.CustomClass;

    namespace something
    {
        public class EmuDurid : CustomClass
        {

            private bool Shifted()
            {
                return (this.Player.GotBuff("Cat Form") || this.Player.GotBuff("Bear Form")
                    || this.Player.GotBuff("Dire Bear Form") || this.Player.GotBuff("Aquatic Form")
                    || this.Player.GotBuff("Travel Form"));
            }



            private void RemoveShift()
            {
                //Lua is bad
                if (this.Player.GotBuff("Cat Form"))
                {
                    this.Player.DoString("CastSpellByName('Cat Form')");
                }
                if (this.Player.GotBuff("Bear Form"))
                {
                    this.Player.DoString("CastSpellByName('Bear Form')");
                }
                if (this.Player.GotBuff("Dire Bear Form"))
                {
                    this.Player.DoString("CastSpellByName('Dire Bear Form')");
                }
                if (this.Player.GotBuff("Aquatic Form"))
                {
                    this.Player.DoString("CastSpellByName('Aquatic Form')");
                }
                if (this.Player.GotBuff("Travel Form"))
                {
                    this.Player.DoString("CastSpellByName('Travel Form')");
                }

            }

            private bool TargetCanBleed()
            {
                return (this.Target.CreatureType != CreatureType.Elemental && this.Target.CreatureType != CreatureType.Mechanical);

            }

            private bool CanShift()
            {
                return (this.Player.GetSpellRank("Cat Form") != 0 || this.Player.GetSpellRank("Bear Form") != 0);
            }

            private void Shift()
            {
                if (this.Player.GetSpellRank("Cat Form") != 0)
                {
                    this.Player.CastWait("Cat Form", 500);
                }
                else if (this.Player.GetSpellRank("Bear Form") != 0)
                {
                    this.Player.CastWait("Bear Form", 500);
                }
            }

            private int[][] BiteDamage = new int[][]
                {
                    //Element at 0 is estimated white swing dmg
                    new int[] {0,0,0,0,0},
                    new int[] {20, 50,86,122,158,194},
                    new int[] {30, 79,138,197,256,315},
                    new int[] {40, 122,214,306,398,490},
                    new int[] {45, 173,301,429,557,685},
                };

            private bool ShouldWeBite()
            {
                //[]Rank []Combo dmg
                int rank = this.Player.GetSpellRank("Ferocious Bite");
                int damage = 0;
                int comboPoints = this.Player.ComboPoints;
                if (rank >= 1 && comboPoints >= 1)
                {               //Estimated combo point dmg + Estimated white swing dmg
                    damage = (BiteDamage[rank][comboPoints] + BiteDamage[rank][0]);
                    if (this.Target.Health <= damage)
                    {
                        return true;
                    }
                }
                return false;
            }


            private int[][] RipDamage = new int[][]
                {
                    //Element at 0 is estimated white swing dmg
                    new int[] {0,0,0,0,0},
                    new int[] {100, 42,66,90,114,138},
                    new int[] {125, 66,108,150,192,234},
                    new int[] {150, 90,144,198,252,306},
                    new int[] {175, 138,222,306,390,474},
                    new int[] {200, 192,312,432,552,672},
                    new int[] {225, 270,438,606,774,942},
                };

            private bool ShouldWeRip()
            {
                if (!TargetCanBleed())
                    return false;
                //[]Rank []Combo dmg
                int rank = this.Player.GetSpellRank("Rip");
                int damage = 0;
                int comboPoints = this.Player.ComboPoints;
                if (rank >= 1 && comboPoints >= 1)
                {               //Estimated combo point dmg + Estimated white swing dmg
                    damage = (RipDamage[rank][comboPoints] + RipDamage[rank][0]);
                    if (comboPoints == 5 && !ShouldWeBite())
                    {
                        return true;
                    }
                    else if (comboPoints == 5)
                    {
                        return false;
                    }
                    int nextDmg = (RipDamage[rank][comboPoints + 1] + RipDamage[rank][0]);
                    if (this.Target.Health >= damage && Target.Health <= (nextDmg * 1.5))
                    {
                        return true;
                    }
                }
                return false;
            }

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
                    return "EmuDurid";
                }
            }

            public override void PreFight()
            {
                this.Player.Attack();
                if (CanShift())
                {
                    this.SetCombatDistance(3);
                    if (!Shifted())
                    {
                        if (this.Player.GetSpellRank("Faerie Fire") == 0 || this.Player.HealthPercent <= 85)
                        {
                            if (this.Player.GetSpellRank("Rejuvenation") != 0 && !this.Player.GotBuff("Rejuvenation"))
                            {
                                this.Player.CastWait("Rejuvenation", 500);
                            }
                            else
                            {
                                Shift();

                            }
                        }
                        else if (this.Player.GetSpellRank("Faerie Fire") != 0 && this.Player.GetSpellRank("Cat Form") != 0 && TargetCanBleed())
                        {
                            if (!this.Target.GotDebuff("Faerie Fire"))
                            {
                                this.Player.Cast("Faerie Fire");

                            }
                            else
                            {
                                Shift();
                            }
                        }
                        else if (this.Player.GetSpellRank("Moonfire") != 0)
                        {
                            this.Player.Cast("Moonfire");
                        }
                    }
                    else
                    {
                        if (this.Target.DistanceToPlayer >= 20 && this.Player.GetSpellRank("Dash") != 0 && this.Player.CanUse("Dash"))
                        {
                            this.Player.Cast("Dash");
                        }
                        //if (this.Player.GetSpellRank("Faerie Fire (Feral)") != 0 && !this.Target.GotDebuff("Faerie Fire (Feral)"))
                        //{
                        //    this.Player.Cast("Faerie Fire (Feral)");
                        //}

                    }
                }
                else
                {
                    this.SetCombatDistance(30);
                    if (this.Player.GetSpellRank("Moonfire") != 0)
                    {
                        this.Player.Cast("Moonfire");
                    }
                    else
                    {
                        this.Player.Cast("Wrath");
                    }
                }



            }


            public override void Fight()
            {
                this.Player.Attack();
                //Shift in/out to heal /dd
                if (Shifted())
                {
                    this.Player.Attack();
                    this.SetCombatDistance(4);

                    if (this.Player.HealthPercent < 30 && this.Target.HealthPercent > 20 && this.Player.ManaPercent >= 25)
                    {
                        RemoveShift();
                        return;
                    }

                    if (this.Player.GetSpellRank("Cat Form") != 0)
                    {
                        this.SetCombatDistance(3);//Weird attack range with cat form
                        //if (this.Player.GetSpellRank("Faerie Fire Feral") != 0 && TargetCanBleed())
                        //{
                        //   if (!this.Target.GotDebuff("Faerie Fire (Feral)"))
                        //    {
                        //        this.Player.Cast("Faerie Fire (Feral)");
                        //        return;
                        //    }
                        //}
                        if (this.Player.Energy >= 30)
                        {
                            if (ShouldWeBite())
                            {
                                this.Player.Cast("Ferocious Bite");
                            }

                            if (TargetCanBleed())
                            {
                                if (!this.Target.GotDebuff("Rip"))
                                {
                                    if (ShouldWeRip())
                                    {
                                        this.Player.Cast("Rip");

                                    }
                                }
                                if (this.Player.GetSpellRank("Rake") != 0)
                                {
                                    if (!this.Target.GotDebuff("Rake"))
                                    {
                                        if (this.Target.HealthPercent >= 15 && this.Player.Energy >= 40)
                                        {
                                            this.Player.Cast("Rake");
                                        }
                                    }
                                }
                            }


                            if (this.Player.Energy >= 40)
                            {
                                if (this.Player.GetSpellRank("Tiger's Fury") != 0)
                                {
                                    if (this.Player.Energy >= 80)
                                    {
                                        this.Player.Cast("Tiger's Fury");
                                        return;
                                    }
                                }
                                this.Player.Cast("Claw");
                                return;
                            }

                        }
                    }
                    else if (this.Player.GetSpellRank("Bear Form") != 0)
                    {
                        if (this.Player.GetSpellRank("Enrage") != 0)
                        {
                            if (this.Player.Rage <= 15 && this.Player.CanUse("Enrage") && Target.HealthPercent >= 50)
                            {
                                this.Player.Cast("Enrage");
                                return;
                            }
                        }
                        if (this.Player.GetSpellRank("Bash") != 0)
                        {
                            if (this.Player.Rage >= 10 && this.Player.CanUse("Bash"))
                            {
                                this.Player.Cast("Bash");
                                return;
                            }
                        }
                        if (this.Player.GetSpellRank("Maul") != 0)
                        {
                            if (this.Player.Rage >= 15 && this.Player.CanUse("Maul"))
                            {
                                this.Player.Cast("Maul");
                                return;
                            }
                        }
                        if (this.Player.GetSpellRank("Demoralizing Roar") != 0)
                        {
                            if (this.Player.Rage >= 10 && this.Player.CanUse("Demoralizing Roar") && Player.IsAoeSafe(12))
                            {
                                this.Player.Cast("Demoralizing Roar");
                                return;
                            }
                        }

                        //Player.Rage dump
                        if (this.Player.GetSpellRank("Swipe") != 0)
                        {
                            if (this.Player.Rage >= 40 && this.Player.CanUse("Swipe"))
                            {
                                this.Player.Cast("Swipe");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (this.Player.GetSpellRank("Innervate") != 0)
                    {
                        if (this.Player.CanUse("Innervate") && this.Player.ManaPercent <= 30)
                        {
                            this.Player.Cast("Innervate");
                        }
                    }

                    if (this.Player.HealthPercent <= 80 && this.Player.ManaPercent >= 15)
                    {
                        if (Player.HealthPercent <= 30 && Player.GetSpellRank("War Stomp") != 0)
                        {
                            Player.TryCast("War Stomp");
                        }
                        this.Player.CastWait("Healing Touch", 500);
                        return;
                        /*
                        if (this.Player.HealthPercent <= 40 && this.Player.ManaPercent >= 50)
                        {
                            if (!this.Player.GotBuff("Regrowth") && this.Player.GetSpellRank("Regrowth") != 0)
                            {
                                this.Player.Cast("Regrowth");
                                return;
                            }

                        }
                        else
                        {
                            if (this.Player.HealthPercent >= 70 && this.Player.GetSpellRank("Rejuvenation") != 0 && !this.Player.GotBuff("Rejuvenation"))
                            {
                                this.Player.Cast("Rejuvenation");
                                return;
                            }
                            else
                            {
                                this.Player.Cast("Healing Touch");
                                return;
                            }

                        }*/
                    }

                    if (this.Player.ManaPercent >= 10)
                    {
                        if (this.Player.GetSpellRank("Cat Form") != 0 || this.Player.GetSpellRank("Bear Form") != 0)
                        {
                            Shift();
                        }
                        else
                        {
                            if (this.Player.ManaPercent >= 35 && this.Player.GetSpellRank("Moonfire") != 0
                                && !this.Target.GotDebuff("Moonfire"))
                            {
                                this.SetCombatDistance(30);
                                this.Player.Cast("Moonfire");
                                return;
                            }
                            if (this.Player.ManaPercent >= 40)
                            {
                                this.SetCombatDistance(30);
                                this.Player.Cast("Wrath");
                            }
                            else
                            {
                                //Melee Them
                                this.SetCombatDistance(4);
                            }
                        }
                    }


                }

            }

            public override void Rest()
            {
                if (Shifted())
                {
                    RemoveShift();
                }
                if (this.Player.ManaPercent >= 50 && this.Player.HealthPercent <= 60)
                {
                    this.Player.Cast("Healing Touch");
                }
                else
                {
                    this.Player.Drink();
                    this.Player.Eat();
                }
            }

            public override bool Buff()
            {
                if (this.Player.GetSpellRank("Thorns") != 0 && !this.Player.GotBuff("Thorns"))
                {
                    if (Shifted())
                    {
                        RemoveShift();
                        return false;
                    }
                    this.Player.Cast("Thorns");
                    return false;
                }
                if (this.Player.GetSpellRank("Mark of the Wild") != 0 && !this.Player.GotBuff("Mark of the Wild"))
                {
                    if (Shifted())
                    {
                        RemoveShift();
                        return false;
                    }
                    this.Player.Cast("Mark of the Wild");
                    return false;
                }
                //True means we are done buffing, or cannot buff
                RemoveShift();
                return true;
            }

        }
    }
