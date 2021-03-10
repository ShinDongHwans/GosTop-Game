public class UnitStat {
    public int power;
    public int wisdom;
    public int dex;
    public int health;

    public UnitStat(int power, int wisdom, int dex, int health) {
        this.power = power;
        this.wisdom = wisdom;
        this.dex = dex;
        this.health = health;
    }

    public UnitStat(UnitStat stat) : this(stat.power, stat.wisdom, stat.dex, stat.health) {
    }

    public UnitStat() : this(0,0,0,0) {}

    public static UnitStat operator +(UnitStat a, UnitStat b) {
        return new UnitStat(
            a.power + b.power,
            a.wisdom + b.wisdom,
            a.dex + b.dex,
            a.health + b.health
        );
    }
}