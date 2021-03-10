public class Item {
    public int kind;

    public ItemInfo info {
        get {
            return ItemInfo.info[kind];
        }
    }

    public virtual string GetName() {
        return "Something";
    }

    public virtual void OnTurnEnd() { }
}