using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Magic {
    public abstract string name { get; }
    public abstract string description { get; }
    public string test;
    public void Invoke(Status invoker) {
        Invoke(invoker, invoker.position);
    }
    public abstract void Invoke(Status invoker, Vector2Int destination);



    public class ThrowStone : Magic {
        public override string name { get { return "Throw Stone"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var ev = new PhysicThrowEvent("Stone",
                Util.LoadSprite("DawnLike/Objects/Effect0", 100),
                invoker.position,
                destination,
                25f,
                5 + invoker.status.atk, invoker.status.acc);
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class Arrow1 : Magic {
        public override string name { get { return "Arrow1"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var ev = new PhysicThrowEvent("Arrow",
                Util.LoadSprite("DawnLike/Items/Ammo", 14),
                invoker.position,
                destination,
                25f,
<<<<<<< HEAD
                (invoker.status.atk + invoker.stat.dex/10), invoker.status.acc,  -135f);
=======
                invoker.status.atk, -135f);
>>>>>>> dc46092fcf43b00674485ef50d9b5fbe383e1011
            GameManager.instance.ReserveEvent(ev);
        }
    }
    public class Arrow2 : Magic {
        public override string name { get { return "Arrow2"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var ev = new PhysicThrowEvent("Arrow",
                Util.LoadSprite("DawnLike/Items/Ammo", 15),
                invoker.position,
                destination,
                25f,
<<<<<<< HEAD
                (invoker.status.atk + invoker.stat.dex / 7), invoker.status.acc ,- 45f);
=======
                5 + (int)(invoker.status.atk * 1.2f), -45f);
>>>>>>> dc46092fcf43b00674485ef50d9b5fbe383e1011
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class Dart : Magic {
        public override string name { get { return "Dart"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var ev = new PhysicThrowEvent("Dart",
                Util.LoadSprite("DawnLike/Items/Ammo", 15),
                invoker.position,
                destination,
                25f,
                (int)(invoker.stat.dex * Mathf.Log10(invoker.stat.dex)), invoker.status.acc, - 135f);
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class IronClaw : Magic
    {
        public override string name { get { return "Iron Claw"; } }
        public override string description { get { return "Scratch"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new PhysicThrowEvent("IronClaw",
                Util.LoadSprite("DawnLike/Objects/Decor0", 70),
                invoker.position,
                destination,
                25f,
                10 + invoker.stat.power, invoker.status.acc, 90f);
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class Smite : Magic {
        public override string name { get { return "Smite"; } }
        public override string description { get { return "Damage the target ignoring its DEF"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var target = GameManager.instance.GetUnitAt(destination);
            if (target != null) {
                target.Damage(20);
                Log.Make("Something smites " + target.GetName());
            }
        }
    }

    public class MagicBolt : Magic
    {
        public override string name { get { return "Magic Bolt"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new MagicThrowEvent("Magic Bolt",
                Util.LoadSprite("DawnLike/Objects/Effect0", 100),
                invoker.position,
                destination,
                25f,
                15 + invoker.stat.wisdom);
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class FireBolt : Magic
    {
        public override string name { get { return "Fire Bolt"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new MagicThrowEvent("Fire Bolt",
                Util.LoadSprite("DawnLike/Objects/Effect0", 72),
                invoker.position,
                destination,
                25f,
                15 + invoker.stat.wisdom + (int)(Mathf.Log10(invoker.stat.wisdom)));
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class IceBolt : Magic
    {
        public override string name { get { return "Ice Bolt"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new MagicThrowEvent("Ice Bolt",
                Util.LoadSprite("DawnLike/Objects/Effect0", 80),
                invoker.position,
                destination,
                25f,
                15 + invoker.stat.wisdom + (int)(Mathf.Log10(invoker.stat.wisdom)));
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class ThunderBolt : Magic
    {
        public override string name { get { return "ThunderBolt"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new MagicThrowEvent("ThunderBolt",
                Util.LoadSprite("DawnLike/Objects/Effect0", 134),
                invoker.position,
                destination,
                25f,
                10 + invoker.stat.wisdom);
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class LightningBolt : Magic
    {
        public override string name { get { return "Lightning Bolt"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var ev = new MagicThrowEvent("Lightning Bolt",
                Util.LoadSprite("DawnLike/Objects/Effect0", 88),
                invoker.position,
                destination,
                25f,
                15 + invoker.stat.wisdom + (int)(Mathf.Log10(invoker.stat.wisdom)));
            GameManager.instance.ReserveEvent(ev);
        }
    }

    public class Fireball : Magic {
        public override string name { get { return "Fireball"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            GameManager.instance.ReserveEvent(new ThrowFireballEvent(invoker.position, destination, invoker));
            Log.Make(invoker.GetName() + " shoots a fireball!");
        }

        class ThrowFireballEvent : MagicThrowEvent {
            Status invoker;
            public ThrowFireballEvent(Vector2Int startPosition, Vector2Int endPosition, Status invoker)
                : base("Fireball", Util.LoadSprite("DawnLike/Objects/Effect0", 96), startPosition, endPosition, 20f, 0) {
                this.invoker = invoker;
            }
            public override void OnHit() {
                GameManager.instance.ReserveEvent(new ExplosionEvent(0, endPosition, 15 + (int)(invoker.stat.wisdom * 1.5)));
                Log.Make("Fireball was exploded!!");
            }
        }
    }

    public class Tornado : Magic
    {
        public override string name { get { return "Tornado"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.ReserveEvent(new CentralMoveEvent(destination, 10 + (int)(invoker.stat.wisdom * 1.5), invoker));
            Log.Make(invoker.GetName() + "Invokes a tornado");
        }
    }

    public class ChainLightning : Magic
    {
        public override string name { get { return "ChainLightning"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.ReserveEvent(new ChainLightningEvent(destination, 10 + (int)(invoker.stat.wisdom * 1.5), invoker));
            Log.Make(invoker.GetName() + "Invokes a chainlightning");
        }
    }


    public class HydroPump : Magic
    {
        public override string name { get { return "HydroPump"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.ReserveEvent(new JetPumpEvent(destination, 10 + (int)(invoker.stat.wisdom * 1.5), invoker));
            Log.Make(invoker.GetName() + "Invokes a HydroPump");
        }
    }

    public class SmallHeal : Magic
    {
        public override string name { get { return "Small Heal"; } }
        public override string description { get { return "Heal the target"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var target = GameManager.instance.GetUnitAt(destination);
            if (target != null)
            {
                target.Heal(10 + invoker.stat.wisdom / 5);
                Log.Make(target.GetName() + " restored some HP");
            }
        }
    }

    public class HealPotion : Magic
    {
        public override string name { get { return "Heal Potion"; } }
        public override string description { get { return "Heal self"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var target = GameManager.instance.GetUnitAt(destination);
            if (target != null)
            {
                target.Heal(10);
                Log.Make(target.GetName() + " restored some HP");
            }
        }
    }

    public class BigHealPotion : Magic
    {
        public override string name { get { return "Big Heal Potion"; } }
        public override string description { get { return "Heal self"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var target = GameManager.instance.GetUnitAt(destination);
            if (target != null)
            {
                target.Heal(25);
                Log.Make(target.GetName() + " restored some HP");
            }
        }
    }

    public class MonsterMeat : Magic
    {
        public override string name { get { return "MonsterMeat"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            if (Random.Range(0, 10) < 7)
            {
            info[FindInfoByName("Reduce Hunger1")].Invoke(invoker, destination);
            }
            else
            {
                GameManager.instance.player.Damaged(1);
                Log.Make("Suck!! Rotten Meat!!");
            }
        }
    }

    public class BossMeat : Magic
    {
        public override string name { get { return "BossMeat"; } }
        public override string description { get { return "Be full and status up!!"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.player.Eat(50f);
            invoker.stat.power++;
            invoker.stat.wisdom++;
            invoker.stat.dex++;
            invoker.stat.health++;
        }
    }

    public class ReduceHunger1 : Magic {
        public override string name { get { return "Reduce Hunger1"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.player.Eat(10f);
            Log.Make("Yummy!");
        }
    }

    public class ReduceHunger2 : Magic {
        public override string name { get { return "Reduce Hunger2"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.player.Eat(20f);
            Log.Make("Yummy!");
        }
    }

    public class BothRecover1 : Magic
    {
        public override string name { get { return "BothRecover1"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            invoker.Heal(15);
            GameManager.instance.player.Eat(15f);
            Log.Make("You feel becoming healthy");
        }
    }

    public class BothRecover2 : Magic
    {
        public override string name { get { return "BothRecover2"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            invoker.Heal(2);
            GameManager.instance.player.Eat(30f);
            Log.Make("You feel becoming healthy!");
        }
    }

    public class BothRecover3 : Magic
    {
        public override string name { get { return "BothRecover3"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            invoker.Heal(25);
            GameManager.instance.player.Eat(30f);
            Log.Make("You feel becoming healthy!!");
        }
    }

    public class IncreaseInven : Magic
    {
        public override string name { get { return "IncreaseInven"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.player.bag++;
        }
    }

    public class WisUpgrade : Magic
    {
        public override string name { get { return "WisUpgrade"; } }
        public override string description { get { return "Your wisdom is upgraded"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            GameManager.instance.player.stat.wisdom += 10;
            Log.Make("Your magic be stroger!!");
        }
    }

    public class TeleportShortDistance : Magic {
        public override string name { get { return "Teleport Short Distance"; } }
        public override string description { get { return "Transport target to random position"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var target = GameManager.instance.GetUnitAt(destination);
            int maxDistance = 2 + invoker.stat.wisdom / 20;
            if (target != null) {
                var gameManager = GameManager.instance;
                var levelManager = gameManager.levelManager;
                for (int i = 0; i < 15; i++) {
                    int dx = Random.Range(-maxDistance, maxDistance + 1);
                    int dy = Random.Range(-maxDistance, maxDistance + 1);
                    var v = new Vector2Int(dx, dy) + target.position;
                    if (levelManager.currentLevel.IsVectorInMap(v) &&
                        !levelManager.currentLevel.tiles[v.x][v.y].blocksMove &&
                        gameManager.GetUnitAt(v) == null) {
                        if (target is PlayerStatus) {
                            gameManager.MovePlayerTo(v);
                        } else {
                            target.position = v;
                        }
                        Log.Make(target.GetName() + " teleports");
                        return;
                    }
                }
                Log.Make(target.GetName() + " fails to teleport");
            }
        }
    }

    public class Warp : Magic
    {
        public override string name { get { return "Warp"; } }
        public override string description { get { return "Transport target to random position"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            var target = GameManager.instance.GetUnitAt(destination);
            int maxDistance = 25;
            if (target != null)
            {
                var gameManager = GameManager.instance;
                var levelManager = gameManager.levelManager;
                for (int i = 0; i < 15; i++)
                {
                    int dx = Random.Range(-maxDistance, maxDistance + 1);
                    int dy = Random.Range(-maxDistance, maxDistance + 1);
                    var v = new Vector2Int(dx, dy) + target.position;
                    if (levelManager.currentLevel.IsVectorInMap(v) &&
                        !levelManager.currentLevel.tiles[v.x][v.y].blocksMove &&
                        gameManager.GetUnitAt(v) == null)
                    {
                        if (target is PlayerStatus)
                        {
                            gameManager.MovePlayerTo(v);
                        }
                        else
                        {
                            target.position = v;
                        }
                        Log.Make(target.GetName() + " teleports");
                        return;
                    }
                }
                Log.Make(target.GetName() + " fails to teleport");
            }
        }
    }

    public class ShowMap : Magic {
        public override string name { get { return "Show Map"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            GameManager.instance.levelManager.MarkAllVisited();
            Log.Make("You noticed the structure of this floor!");
        }
    }

    public class BreakWalls : Magic {
        public override string name { get { return "Break Walls"; } }
        public override string description { get { return "NO DESC"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            var lm = GameManager.instance.levelManager;
            lm.currentLevel.BreakWalls(destination, invoker.position);
            var p = GameManager.instance.player.position;
            GameManager.instance.levelManager.UpdateShadow(p, p);
            Log.Make("Some walls are destroyed");
        }
    }

    public class Fear : Magic {
        public override string name { get { return "Fear"; } }
        public override string description { get { return "Fear all NPCs in your sight"; } }
        public override void Invoke(Status invoker, Vector2Int destination) {
            List<NPCStatus> list = GameManager.instance.GetNPCsInSight();
            foreach (var npc in list) {
                npc.actionType = NPCStatus.EAction.Fearing;
            }
            Log.Make("You fear everyone!");
        }
    }

    public class RandomMagic : Magic
    {
        public override string name { get { return "Random Magic"; } }
        public override string description { get { return "Random magic is used"; } }
        public override void Invoke(Status invoker, Vector2Int destination)
        {
            info[Random.Range(0, info.Length)].Invoke(invoker, destination);
        }
    }


    /* Magics */
    public static Magic[] info = new Magic[]{
        /* Physical */
        new ThrowStone(),
        new Arrow1(),
        new Arrow2(),
        new IronClaw(),
        new IncreaseInven(),
        new HealPotion(),
        new BigHealPotion(),
        new Dart(),

        /* Real Magic */
        new MonsterMeat(),
        new BossMeat(),
        new SmallHeal(),
        new ThunderBolt(),
        new BothRecover1(),
        new BothRecover2(),
        new BothRecover3(),
        new Smite(),
        new Fireball(),
        new TeleportShortDistance(),
        new Warp(),
        new ReduceHunger1(),
        new ReduceHunger2(),
        new WisUpgrade(),
        new Tornado(),
        new ChainLightning(),
        new HydroPump(),
        new MagicBolt(),
        new FireBolt(),
        new IceBolt(),
        new LightningBolt(),
        new ShowMap(),
        new BreakWalls(),
        new Fear(),
        new RandomMagic(),
    };

    public static int infoPhysicalMaxIndex = 8;

    public static int FindInfoByName(string name) {
        for(int i = 0; i < info.Length; i++) {
            if(info[i].name.Equals(name)) return i;
        }
        return -1;
    }
}
