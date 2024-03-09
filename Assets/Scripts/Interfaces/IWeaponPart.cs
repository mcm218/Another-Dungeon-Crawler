namespace Interfaces {
    public interface IWeaponPart {
        public IWeaponAction Action { get; }
    }

    public interface IWeaponAction {
        public float Damage { get; }
        public float Modifier { get; }

        public IWeaponAction Add(IWeaponAction action);
        
        public void Execute();
    }
}