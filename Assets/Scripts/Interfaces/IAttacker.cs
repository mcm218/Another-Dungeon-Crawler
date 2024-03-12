using UnityEngine;
using Weapons;

namespace Interfaces {
    public interface IAttacker {
        // public void    Attack(IHealth target);
        public Sword Sword { get; }
        
        public Gun Gun { get;  }
        
        public GameObject GameObject { get; }
    }
}