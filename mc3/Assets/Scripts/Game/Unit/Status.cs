using UnityEngine;

public abstract class Status {
    public GameObject gameObject;

    private Vector2Int _position;
    public virtual Vector2Int position {
        get { return _position; }
        set {
            if(gameObject != null) {
                gameObject.transform.position =
                    new Vector3(value.x, value.y, gameObject.transform.position.z);
            }
            _position = value;
        }
    }

    public virtual string GetName() { return "Something"; }

    public UnitStat stat;
    public abstract UnitStatus status { get; }
    public int hp {
        get { return (int)_hp; }
        set { _hp = value; }
    }
    public float _hp { get; set; }
    public int[] resistances;

    public Status(Vector2Int position, UnitStat stat, int[] resistances) {
        this.position = position;
        this.stat = stat;
        this.resistances = resistances;
        _hp = status.maxHP;
    }

    public Status() : this(new Vector2Int(), new UnitStat(), new int[7]) {
        _hp = status.maxHP;
    }


    public virtual void Heal(int value) {
        if(value < 0) return;
        int pasthp = this.hp;
        this.hp += value;
        var s = status;
        if(hp > s.maxHP) this.hp = s.maxHP;
        DamageScript.Create(position, this.hp - pasthp);
    }

    public virtual void Damage(int value) {
        if(value < 0) return;
        this.hp -= value;
        DamageScript.Create(position, -value);
    }

    public virtual void Dodge() {
        /* Do Nothing */
        DamageScript.Create(position, DamageScript.DODGED);
    }

    public void Damaged(int atk) {
        Damage(status.CalculateAttackedDamage(atk));
    }

    public void DistanceDamaged(int atk, float acc, Status other, string name)
    {

        if (Random.Range(0f, 1f) <= acc - other.status.agi)
        {
            Damage(status.CalculateAttackedDamage(atk));
            Log.Make(name + " hits " + other.GetName() + "!");
        }
        else
            other.Dodge();
    }

    public bool Attack(Status other) {
        if(!status.TryAttackHitText(other.status)) {
            other.Dodge();
            return false;
        }
        other.Damage(status.CalculateAttackDamage(other.status));
        Log.Make(this.GetName() + " attacks " + other.GetName());
        return true;
    }

}