namespace Interfaces {
    public interface IHealth {
        float Health { get; }
        public    void  Damage(float damage);
        public    void  ResetHealth();
    }
}