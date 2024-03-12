namespace Weapons {
    public class WeaponModel {
        public float damage      = 10f;
        public float range       = 3f;
        public float attackRate  = 1f;
        public float impactForce = 30f;
        public float angle       = 90f;
        
        // + operator
        public static WeaponModel operator +(WeaponModel a, WeaponModel b) {
            return new WeaponModel {
                damage = a.damage + b.damage,
                range = a.range + b.range,
                attackRate = a.attackRate + b.attackRate,
                impactForce = a.impactForce + b.impactForce
            };
        }

        public static WeaponModel operator +(WeaponModel a, SwordPartModel b) {
            return new WeaponModel {
                damage = a.damage + b.damage,
                range = a.range + b.range,
                attackRate = a.attackRate + b.attackRate,
                impactForce = a.impactForce + b.impactForce
            };
        }
    }
}