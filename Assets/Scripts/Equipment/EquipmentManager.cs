using System;
using SubjectA04.HealthSystem;
using UnityEngine;

namespace SubjectA04.EquipmentSystem
{
    public class EquipmentManager : MonoBehaviour, IDamageModifier
    {
        private const float MaximumDamageReduction = 0.9f;

        public string EquippedArmorName { get; private set; }
        public float ArmorDamageReduction { get; private set; }
        public bool HasArmor => !string.IsNullOrWhiteSpace(EquippedArmorName);

        public event Action Changed;

        public void EquipArmor(string armorName, float damageReduction)
        {
            if (string.IsNullOrWhiteSpace(armorName))
            {
                return;
            }

            EquippedArmorName = armorName;
            ArmorDamageReduction = Mathf.Clamp(damageReduction, 0f, MaximumDamageReduction);

            Changed?.Invoke();
            Debug.Log(
                $"{name} equipped {EquippedArmorName} with {ArmorDamageReduction:P0} damage reduction.",
                this);
        }

        public float ModifyDamage(float damage)
        {
            if (!HasArmor || damage <= 0f)
            {
                return damage;
            }

            return damage * (1f - ArmorDamageReduction);
        }
    }
}
