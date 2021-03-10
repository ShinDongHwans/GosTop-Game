public class Consumable : Item {
    public int count;

    public Consumable(int kind, int count) {
        this.kind = kind;
        this.count = count;
    }

    public Consumable(int kind) : this(kind, 1) {}

    public override string GetName() {
        if(count == 0) return "a " + info.name;
        else return count + " " + info.name;
    }
}