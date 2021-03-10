using UnityEngine;

public class AttackRange {
    public int from, to;
    
    public AttackRange(int from, int to) {
        this.from = from;
        this.to = to;
    }

    public bool IsInRange(Vector2Int v) {
        var d = Util.MaxNorm(v);
        return from <= d && d <= to;
    }

    public bool IsInRange(Vector2Int point, Vector2Int origin) {
        var d = Util.MaxDistance(point, origin);
        return from <= d && d <= to;
    }
}