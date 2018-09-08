
using UnityEngine;

public class DamageInfo
{
    
    public DamageInfo(float damageValue, GameObject attacker)
    {
        this.DamageValue = damageValue;
        this.Attacker = attacker;
    }

    public float DamageValue { get; private set; }
    public GameObject Attacker { get; private set; }
    
}

public interface I_Damageable
{
    bool IsDamageable { get; }
    void ApplyDamage(DamageInfo damageInfo);

}