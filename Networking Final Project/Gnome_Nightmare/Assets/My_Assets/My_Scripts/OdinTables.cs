namespace OdinTables
{
    //Used to store all the Tables I make using Odin

    using System.Collections.Generic;
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class OdinTables : SerializedMonoBehaviour {
        public static OdinTables instance;
        void Awake() { instance = this; }
    }
    
    public class Table1x5 {
        [TableMatrix(SquareCells = true, HideColumnIndices = true, HideRowIndices = true)]
        public GameObject[,] table1x5 = new GameObject[1, 5];
    }
    public class Table2x5 {
        [TableMatrix(SquareCells = true, HideColumnIndices = true, HideRowIndices = true)]
        public GameObject[,] table2x5 = new GameObject[2, 5];
    }
    public class Table3x5 {
        [TableMatrix(SquareCells = true, HideColumnIndices = true, HideRowIndices = true)]
        public GameObject[,] table3x5 = new GameObject[3, 5];
    }




    public class EnemyDropTable {
        [TabGroup("Enemy", false, 0)]
        [LabelWidth (50)]
        public GameObject Enemy;

        [TabGroup("Items", false, 1)]
        public List<ItemDropTable> Items = new List<ItemDropTable>();
    }
    public class ItemDropTable {
        [LabelWidth(50)]
        public GameObject Item;
        [HorizontalGroup("Group 2"), LabelWidth(80)]
        [MinValue(0), MaxValue(100)]
        public float DropChance = 0;
        [HorizontalGroup("Group 2"), LabelWidth(50)]
        public int Amount = 1;
    }


    public class OnlineTable {
        [TabGroup("Ammo", false, 0)]
        [TableList]
        public List<OnlineItemTable> Ammo = new List<OnlineItemTable>();
        [TabGroup("Parts", false, 1)]
        [TableList]
        public List<OnlineItemTable> Parts = new List<OnlineItemTable>();
        [TabGroup("Misc", false, 2)]
        [TableList]
        public List<OnlineItemTable> Misc = new List<OnlineItemTable>();
    }
    public class OnlineItemTable {
        [LabelWidth(50)]
        public GameObject Item;
        [HorizontalGroup("Summary"), LabelWidth(60)]
        public string Summary;
        [HorizontalGroup("Stats"), LabelWidth(50)]
        [MinValue(0)]
        public int Price = 0;
        [HorizontalGroup("Stats"), LabelWidth(50)]
        [MinValue(0)]
        public int Amount = 1;
    }

    public class WeaponStatsTable {
        public int NumberOfStatsInTable = 7;
        public Stat Damage = new Stat(Stat.StatType.Damage);
        public Stat Range = new Stat(Stat.StatType.Range);
        public Stat ClipSize = new Stat(Stat.StatType.ClipSize);
        public Stat AmountCount = new Stat(Stat.StatType.AmountCount);
        public Stat FireRate = new Stat(Stat.StatType.FireRate);
        public Stat ReloadTime = new Stat(Stat.StatType.ReloadTime);
        public Stat ImpactForce = new Stat(Stat.StatType.ImpactForce);
        //Damage
        //Range
        //ClipSize
        //AmountCount
        //FireRate
        //ReloadTime
        //ImpactForce

        public void SetStats(WeaponStatsTable FromThis, WeaponStatsTable ToThis) {
            //Clears all stats form [ToThis]
            ToThis.Damage.RemoveAllModifiers();
            ToThis.Range.RemoveAllModifiers();
            ToThis.ClipSize.RemoveAllModifiers();
            ToThis.AmountCount.RemoveAllModifiers();
            ToThis.FireRate.RemoveAllModifiers();
            ToThis.ReloadTime.RemoveAllModifiers();
            ToThis.ImpactForce.RemoveAllModifiers();
            //Sets all base values from [FromThis] to [ToThis]
            ToThis.Damage.baseValue = FromThis.Damage.baseValue;
            ToThis.Range.baseValue = FromThis.Range.baseValue;
            ToThis.ClipSize.baseValue = FromThis.ClipSize.baseValue;
            ToThis.AmountCount.baseValue = FromThis.AmountCount.baseValue;
            ToThis.FireRate.baseValue = FromThis.FireRate.baseValue;
            ToThis.ReloadTime.baseValue = FromThis.ReloadTime.baseValue;
            ToThis.ImpactForce.baseValue = FromThis.ImpactForce.baseValue;
            //Adds all ItemStats from [FromThis] to [ToThis] stats
            ToThis.Damage.AddModifier(FromThis.Damage.GetModifierValue());
            ToThis.Range.AddModifier(FromThis.Range.GetModifierValue());
            ToThis.ClipSize.AddModifier(FromThis.ClipSize.GetModifierValue());
            ToThis.AmountCount.AddModifier(FromThis.AmountCount.GetModifierValue());
            ToThis.FireRate.AddModifier(FromThis.FireRate.GetModifierValue());
            ToThis.ReloadTime.AddModifier(FromThis.ReloadTime.GetModifierValue());
            ToThis.ImpactForce.AddModifier(FromThis.ImpactForce.GetModifierValue());

        }

        public void AddAllModifiers(WeaponStatsTable FromThis, WeaponStatsTable ToThis) {
            //Adds all ItemStats from [FromThis] to [ToThis] stats
            ToThis.Damage.AddModifier(FromThis.Damage.GetModifierValue());
            ToThis.Range.AddModifier(FromThis.Range.GetModifierValue());
            ToThis.ClipSize.AddModifier(FromThis.ClipSize.GetModifierValue());
            ToThis.AmountCount.AddModifier(FromThis.AmountCount.GetModifierValue());
            ToThis.FireRate.AddModifier(FromThis.FireRate.GetModifierValue());
            ToThis.ReloadTime.AddModifier(FromThis.ReloadTime.GetModifierValue());
            ToThis.ImpactForce.AddModifier(FromThis.ImpactForce.GetModifierValue());

        }
        public void AddAllModifiersByPercent(WeaponStatsTable FromThis, WeaponStatsTable ToThis, float percent) {
            //Adds all ItemStats from [FromThis] to [ToThis] stats by a percent
            ToThis.Damage.AddModifier(FromThis.Damage.GetModifierValue() * percent);
            ToThis.Range.AddModifier(FromThis.Range.GetModifierValue() * percent);
            ToThis.ClipSize.AddModifier(FromThis.ClipSize.GetModifierValue() * percent);
            ToThis.AmountCount.AddModifier(FromThis.AmountCount.GetModifierValue() * percent);
            ToThis.FireRate.AddModifier(FromThis.FireRate.GetModifierValue() * percent);
            ToThis.ReloadTime.AddModifier(FromThis.ReloadTime.GetModifierValue() * percent);
            ToThis.ImpactForce.AddModifier(FromThis.ImpactForce.GetModifierValue() * percent);
        }

        public void SquishAllModifierValues(WeaponStatsTable ToThis) {
            //Turns all Modifier Values into one
            ToThis.Damage.SquishModifierValue();
            ToThis.Range.SquishModifierValue();
            ToThis.ClipSize.SquishModifierValue();
            ToThis.AmountCount.SquishModifierValue();
            ToThis.FireRate.SquishModifierValue();
            ToThis.ReloadTime.SquishModifierValue();
            ToThis.ImpactForce.SquishModifierValue();
        }
        public void SquishAllValuesToBase(WeaponStatsTable ToThis) {
            //Turns all Modifier Values into base
            ToThis.Damage.SquishValueToBase();
            ToThis.Range.SquishValueToBase();
            ToThis.ClipSize.SquishValueToBase();
            ToThis.AmountCount.SquishValueToBase();
            ToThis.FireRate.SquishValueToBase();
            ToThis.ReloadTime.SquishValueToBase();
            ToThis.ImpactForce.SquishValueToBase();
        }
    }

}