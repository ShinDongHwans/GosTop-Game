using UnityEngine;

public class Util {

    /* Norm Calculator */
    public static int TaxiNorm(Vector2Int d) {
        return Mathf.Abs(d.x) + Mathf.Abs(d.y);
    }
    public static int SquareNorm(Vector2Int d) {
        return d.x * d.x + d.y + d.y;
    }
    public static float EuclideanNorm(Vector2Int d) {
        return Mathf.Sqrt(SquareNorm(d));
    }
    public static int MaxNorm(Vector2Int d) {
        return Mathf.Max(Mathf.Abs(d.x), Mathf.Abs(d.y));
    }

    /* Distance Calculator */
    public static int TaxiDistance(Vector2Int a, Vector2Int b) {
        return TaxiNorm(b - a);
    }
    public static int SquareDistance(Vector2Int a, Vector2Int b) {
        return SquareNorm(b - a);
    }
    public static float EuclideanDistance(Vector2Int a, Vector2Int b) {
        return EuclideanNorm(b - a);
    }
    public static int MaxDistance(Vector2Int a, Vector2Int b) {
        return MaxNorm(b - a);
    }


    /* Spirte */
    public static Sprite LoadSprite(string name, int index) {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + name);
        if(sprites == null || index < 0 || index >= sprites.Length) return null;
        return sprites[index];
    }
    public static Sprite LoadSprite(string name, int index1, int index2) {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + name + index1);
        if(sprites == null || index2 < 0 || index2 >= sprites.Length) return null;
        return sprites[index2];
    }
}


public class Tuple<F, S> {
    public F first;
    public S second;
    
    public Tuple(F f, S s) {
        first = f;
        second = s;
    }
}