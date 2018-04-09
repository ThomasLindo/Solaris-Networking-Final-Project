using Sirenix.OdinInspector;

public class ItemStats : SerializedMonoBehaviour {

    [TableList]
    public OdinTables.WeaponStatsTable itemStats = new OdinTables.WeaponStatsTable();
    //public List<Stat> itemStats = new List<Stat>() { };

}
    
