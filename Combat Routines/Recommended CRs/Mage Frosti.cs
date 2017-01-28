    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ZzukBot.Engines.CustomClass;
    using ZzukBot.Engines.CustomClass.Objects;

    namespace TwinRovaMageCC //0.2.6
    {
        internal static class Constants
        {
            public static readonly string[] TalentStrings =
            {
                    "2200000000000000000000000000000025353203132351351"
                };
        }
       // This is a modified EmuMage for leveling as frost. Credit goes to Emu for the original script.
        public class TwinRovaMage : CustomClass
        {
            //edit to true if you want these spells
            bool useManaShield = false;
            bool useDampenMagic = true;
            bool useIceBlock = false;
            bool useCounterSpell = true;
            bool useBlink = true;
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            //this enum will be used later in the fight loop to improve the class greatly.
            enum fightingStateEnum {Bursting, LowPlayerHealth, LowPlayerMana, LowTargetHealth, FrostNovaToBlink, Polymorphing, AoEPull, RunAway, EnemyFlee};

            fightingStateEnum fightState = fightingStateEnum.Bursting;

            //these timers are used to make it look more human, and make calculations for spell timing.
            int timerFrostNovaBlink = 0;

            //used for conjure food and water
            private string[] WaterName = {"", "Conjured Water", "Conjured Fresh Water",
                                             "Conjured Purified Water", "Conjured Spring Water",
                                             "Conjured Mineral Water", "Conjured Sparkling Water",
                                             "Conjured Crystal Water"};
            private string[] FoodName = {"", "Conjured Muffin", "Conjured Bread",
                                             "Conjured Rye", "Conjured Pumpernickel",
                                             "Conjured Sourdough", "Conjured Sweet Roll",
                                             "Conjured Cinnamon Roll"};

            //mana gems
            private string[] manaGemNames = {"Mana Agate"};

            private int[] manaGemLevels = {28};

            private readonly TalentManager talentManager;

            public TwinRovaMage()
            {
                this.talentManager = new TalentManager(this.Player);
            }

            //mage class
            public override byte DesignedForClass
            {
                get
                {
                    return PlayerClass.Mage;
                }
            }

            //CC name
            public override string CustomClassName
            {
                get
                {
                    return "TwinrovaFrost";
                }
            }

            //pre fight
            public override void PreFight()
            {
                this.Player.Attack();
                this.SetCombatDistance(30);
                if (this.Player.GetSpellRank("Ice Barrier") != 0)
                {
                    if (!this.Player.GotBuff("Ice Barrier"))
                    {
                        if (this.Player.CanUse("Ice Barrier"))
                        {
                            this.Player.Cast("Ice Barrier");
                        }
                    }
                }

                if (this.Player.GetSpellRank("Frostbolt") == 0)
                {
                    this.Player.Cast("Fireball");
                }
                else
                {
                    this.Player.Cast("Frostbolt");
                }

            }

            //FIGHT!
            public override void Fight()
            {

                //Fight States!!
                /*
                switch (fightState)
                {
                    case fightingStateEnum.Bursting:
                    {
                           
                        break;
                    }
                    case fightingStateEnum.LowPlayerHealth:
                    {

                        break;
                    }
                    case fightingStateEnum.LowPlayerMana:
                    {

                        break;
                    }
                    case fightingStateEnum.LowTargetHealth:
                    {

                        break;
                    }
                    case fightingStateEnum.FrostNovaToBlink:
                    {

                        break;
                    }
                    case fightingStateEnum.Polymorphing:
                    {

                        break;
                    }
                    case fightingStateEnum.AoEPull:
                    {

                        break;
                    }
                    case fightingStateEnum.RunAway:
                    {

                        break;
                    }
                    case fightingStateEnum.EnemyFlee:
                    {

                        break;
                    }

                }
                */

                bool canWand = this.Player.IsWandEquipped();

                if (!canWand)
                {
                    Player.Attack();
                }
               
                //mana gem
                if (this.Player.ManaPercent < 10 && this.Player.ItemCount(manaGemNames[0]) == 1)
                {
                    this.Player.UseItem(manaGemNames[0]);
                }

                //wand
                if (this.Player.ManaPercent < 8 || Target.HealthPercent < 5)
                {
                    if (canWand && this.Player.IsCasting != "Shoot" && this.Player.IsChanneling != "Shoot")
                    {
                        this.Player.StartWand();
                        return;
                    }
                }

                //mana shield
                if (this.Player.ManaPercent >= 75 && !this.Player.GotBuff("Mana Shield") && useManaShield)
                {
                    if (this.Player.GetSpellRank("Mana Shield") != 0)
                    {
                        this.Player.Cast("Mana Shield");
                    }
                }

                if (this.Player.HealthPercent <= 5 && this.Player.CanUse("Ice Block") && useIceBlock)
                {
                    this.Player.Cast("Ice Block");
                }
                 
                //ice barrier
                if (this.Player.GetSpellRank("Ice Barrier") != 0 && (this.Attackers.Count > 1 || this.Target.HealthPercent > 10))
                {
                    if (!this.Player.GotBuff("Ice Barrier"))
                    {
                        if (this.Player.CanUse("Ice Barrier"))
                        {
                            this.Player.Cast("Ice Barrier");
                        }
                    }
                }

                //counterspell
                if (this.Target.IsCasting != "" || this.Target.IsChanneling != "" && useCounterSpell)
                {
                    if (this.Player.GetSpellRank("Counterspell") != 0)
                    {
                        if (this.Player.CanUse("Counterspell"))
                        {
                            this.Player.Cast("Counterspell");
                            return;
                        }
                    }
                }

                //cone of cold
                if (this.Player.GetSpellRank("Cone of Cold") != 0 && this.Target.DistanceToPlayer <= 10 && Target.HealthPercent > 22)
                {
                    if (this.Player.CanUse("Cone of Cold"))
                    {
                        this.Player.Cast("Cone of Cold");
                        return;
                    }
                }

                //fireblast
                if (this.Player.GetSpellRank("Fire Blast") != 0 && this.Target.DistanceToPlayer <= 20)
                {
                    if (this.Player.CanUse("Fire Blast"))
                    {
                        this.Player.Cast("Fire Blast");
                        return;
                    }
                }

                // Frost Nova
                if (this.Player.CanUse("Frost Nova") && this.Target.DistanceToPlayer <= 5 && this.Target.HealthPercent >= 28)
                {
                    this.Player.Cast("Frost Nova");
                    return;

                    /*
                    if (this.Player.GetSpellRank("Blink") != 0 && this.Player.CanUse("Blink"))
                    {

                    }
                     */
                } //coldsnap
                else if (this.Player.CanUse("Frost Nova") == false && this.Target.DistanceToPlayer <= 5 && this.Target.HealthPercent >= 28 && this.Player.CanUse("Cold Snap"))
                {
                    this.Player.Cast("Cold Snap");
                    return;
                }

                //blink
                if (Target.GotDebuff("Frost Nova"))
                {
                    if (this.Player.CanUse("Blink") && useBlink)
                    {
                        this.Player.Cast("Blink");
                    }
                    else
                    {
                        bool res = Player.ForceBackup(4);
                    }
                }
                else
                {
                    Player.StopForceBackup();
                }

                //frostbolt, and backup main spells
                if (this.Player.GetSpellRank("Frostbolt") == 0)
                {
                    this.Player.Cast("Fireball");
                }
                else
                {
                    if (this.Player.CanUse("Frostbolt"))
                    {
                        this.Player.Cast("Frostbolt");
                    }
                    else if (this.Player.CanUse("Fireball"))
                    {
                        this.Player.Cast("Fireball");
                    }
                    else
                    {
                        //this.Player.Cast("Arcane Missiles");
                    }
                   
                }

            }

            //rest
            public override void Rest()
            {
                //evocate for mana if we can
                if (this.Player.GetSpellRank("Evocation") != 0)
                {

                    if (this.Player.CanUse("Evocation") && Player.ManaPercent <= 20)
                    {
                        this.Player.CastWait("Evocation", 5000);
                        //return;
                    }
                }

                //dont cancel evocation if we're casting it.
                if (this.Player.IsChanneling != "Evocation" && this.Player.IsCasting != "Evocation")
                {
                    //food
                    if (this.Player.ItemCount(FoodName[this.Player.GetSpellRank("Conjure Food")]) >= 1)
                    {
                        this.Player.Eat(FoodName[this.Player.GetSpellRank("Conjure Food")]);
                    }
                    else
                    {
                        Player.Eat();
                    }
                    //drink
                    if (this.Player.ItemCount(WaterName[this.Player.GetSpellRank("Conjure Water")]) >= 1)
                    {
                        this.Player.Drink(WaterName[this.Player.GetSpellRank("Conjure Water")]);
                    }
                    else
                    {
                        Player.Drink();
                    }
                    //mana gem
                    if (this.Player.ItemCount(manaGemNames[0]) != 1 && this.Player.CanUse("Conjure " + manaGemNames[0]))
                    {
                        this.Player.Cast("Conjure " + manaGemNames[0]);
                    }
                }
               
                try
                {
                    this.talentManager.DoWork();
                }
                catch (Exception)
                {

                }
            }

            //buffs
            public override bool Buff()
            {

                try
                {
                    this.talentManager.DoWork();
                }
                catch (Exception)
                {

                }

                if (this.Player.IsCasting != "")
                    return false;

                if (this.Player.GetSpellRank("Conjure Water") != 0)
                {
                    if (this.Player.ItemCount(WaterName[this.Player.GetSpellRank("Conjure Water")]) <= 5)
                    {
                        this.Player.Cast("Conjure Water");
                        return false;
                    }
                }

                if (this.Player.GetSpellRank("Conjure Food") != 0)
                {
                    if (this.Player.ItemCount(FoodName[this.Player.GetSpellRank("Conjure Food")]) <= 5)
                    {
                        this.Player.Cast("Conjure Food");
                        return false;
                    }
                }

                if (this.Player.GetSpellRank("Ice Armor") != 0)
                {
                    if (!this.Player.GotBuff("Ice Armor"))
                    {
                        this.Player.Cast("Ice Armor");
                        return false;
                    }
                }
                else
                {
                    if (this.Player.GetSpellRank("Frost Armor") != 0)
                    {
                        if (!this.Player.GotBuff("Frost Armor"))
                        {
                            this.Player.Cast("Frost Armor");
                            return false;
                        }
                    }
                }
                if (this.Player.GetSpellRank("Arcane Intellect") != 0)
                {
                    if (!this.Player.GotBuff("Arcane Intellect"))
                    {
                        this.Player.Cast("Arcane Intellect");
                        return false;
                    }
                }
                if (this.Player.GetSpellRank("Dampen Magic") != 0 && useDampenMagic)
                {
                    if (!this.Player.GotBuff("Dampen Magic"))
                    {
                        this.Player.Cast("Dampen Magic");
                        return false;
                    }
                }

                if (this.Player.ItemCount(manaGemNames[0]) != 1 && this.Player.CanUse("Conjure " + manaGemNames[0]))
                {
                    this.Player.Cast("Conjure " + manaGemNames[0]);
                    return false;
                }
                //True means we are done buffing, or cannot buff
                return true;
            }

            //cheked each fight loop for the best fight state
            private fightingStateEnum GetFightingState()
            {
                if (Target.DistanceToPlayer <= 7)
                {
                    return fightingStateEnum.FrostNovaToBlink;
                }
                else if (this.Player.HealthPercent <= 15)
                {
                    return fightingStateEnum.LowPlayerHealth;
                }
                else if (this.Player.ManaPercent <= 10)
                {
                    return fightingStateEnum.LowPlayerMana;
                }
                else
                {
                    return fightingStateEnum.Bursting;
                }
            }

            //runs all Zzukbot checks for a spell cast. Saves time.
            public bool CheckIfUseableFull(string spellName)
            {
                if (this.Player.GetSpellRank(spellName) != 0 && this.Player.CanUse(spellName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public class Talent
            {
                public string Name { get; private set; }
                public int CurrentRank { get; private set; }
                public int MaxRank { get; private set; }
                public int Tab { get; private set; }
                public int Index { get; private set; }

                public Talent(string name, int currentRank, int maxRank, int tab, int index)
                {
                    this.Name = name;
                    this.CurrentRank = currentRank;
                    this.MaxRank = maxRank;
                    this.Tab = tab;
                    this.Index = index;
                }
            }

            public class IndexModel
            {
                public int Index { get; set; }
            }

            public class TalentManager
            {
                private const string KeyUnspentTalents = "unspentTalents";
                private const string KeyTalentDictionary = "talentDictionary";
                private readonly _Player me;

                public TalentManager(_Player me)
                {
                    this.me = me;
                }

                public void DoWork()
                {
                    var unspentTalents = this.GetUnspentTalents();
                    if (unspentTalents == 0)
                    {
                        return;
                    }

                    var talents = this.GetTalents();
                    for (int i = 0; i < Constants.TalentStrings.Length; i++)
                    {
                        var talentString = Constants.TalentStrings[i];
                        for (int j = 0; j < talentString.Length; j++)
                        {
                            var c = talentString.Substring(j, 1);
                            var number = Convert.ToInt32(c);
                            if (number > talents[j].CurrentRank && number <= talents[j].MaxRank)
                            {
                                this.me.DoString(string.Format("LearnTalent({0}, {1});", talents[j].Tab, talents[j].Index));
                                ClearCache();
                                return;
                            }
                        }
                    }
                }

                private int GetUnspentTalents()
                {
                    return Cache.Instance.GetOrStore(KeyUnspentTalents, () =>
                    {
                        this.me.DoString("TM_unspentTalentPoints, TM_learnedProfessions = UnitCharacterPoints(\"player\");");
                        return new IndexModel { Index = Convert.ToInt32(this.me.GetText("TM_unspentTalentPoints")) };
                    }, 120).Index;
                }

                private IList<Talent> GetTalents()
                {
                    return Cache.Instance.GetOrStore(KeyTalentDictionary, () =>
                    {
                        var talents = new List<Talent>();
                        this.me.DoString("TM_numberOfTabs = GetNumTalentTabs()");
                        int tabCount = Convert.ToInt32(this.me.GetText("TM_numberOfTabs"));

                        for (int i = 1; i <= tabCount; i++)
                        {
                            this.me.DoString(string.Format("TM_numberOfTalents = GetNumTalents({0})", i));
                            int talentCount = Convert.ToInt32(this.me.GetText("TM_numberOfTalents"));
                            for (int j = 1; j <= talentCount; j++)
                            {
                                this.me.DoString(
                                    string.Format(
                                        "TM_nameTalent, TM_icon, TM_tier, TM_column, TM_currRank, TM_maxRank = GetTalentInfo({0},{1});",
                                        i, j));
                                var talent = new Talent(this.me.GetText("TM_nameTalent"),
                                    Convert.ToInt32(this.me.GetText("TM_currRank")),
                                    Convert.ToInt32(this.me.GetText("TM_maxRank")), i, j);

                                talents.Add(talent);
                            }
                        }

                        return talents;
                    }, maxDuration: 120);
                }

                private void ClearCache()
                {
                    Cache.Instance.RemoveFromCache(KeyTalentDictionary);
                    Cache.Instance.RemoveFromCache(KeyUnspentTalents);
                }
            }

            internal class CacheItem
            {
                public object StoredObject { get; private set; }
                public DateTime Time { get; private set; }

                public CacheItem(object obj)
                {
                    this.StoredObject = obj;
                    this.Time = DateTime.UtcNow;
                }
            }

            internal class Cache
            {
                private static readonly Lazy<Cache> instance = new Lazy<Cache>(() => new Cache());
                private static object lockert = new object();
                private readonly Hashtable cache;

                private Cache()
                {
                    cache = new Hashtable();
                }

                public static Cache Instance
                {
                    get { return instance.Value; }
                }

                public T GetOrStore<T>(string key, Func<T> action, int maxDuration = -1) where T : class
                {
                    var result = this.cache[key];

                    if (result == null ||
                        (maxDuration > 0 && DateTime.UtcNow > ((CacheItem)result).Time.AddSeconds(maxDuration)))
                    {
                        lock (lockert)
                        {
                            if (result == null ||
                                (maxDuration > 0 && DateTime.UtcNow > ((CacheItem)result).Time.AddSeconds(maxDuration)))
                            {
                                var obj = action();
                                result = obj != null ? new CacheItem(obj) : new CacheItem(default(T));
                                this.cache[key] = result;
                            }
                        }
                    }

                    if (result == null)
                    {
                        return default(T);
                    }

                    return (T)((CacheItem)result).StoredObject;
                }

                public void RemoveFromCache(string key)
                {
                    if (this.cache.ContainsKey(key))
                    {
                        this.cache.Remove(key);
                    }
                }
            }

        }
    }
