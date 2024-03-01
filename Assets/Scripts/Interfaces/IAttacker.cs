namespace Interfaces {
    public interface IAttacker {
        public void    Attack(IHealth target);
        public IWeapon Weapon { get; }
    }
}