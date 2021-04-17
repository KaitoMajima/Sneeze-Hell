using System;
using UnityEngine;

namespace KaitoCo
{
    public interface IDamageable
    {
        Action<int, IActor> OnDamageTaken {get; set;}
        bool TryTakeDamage(int damage, IActor actor);
    }
}