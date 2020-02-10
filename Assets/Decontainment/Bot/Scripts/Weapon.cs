using UnityEngine;

namespace Bot
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
    public class Weapon : ScriptableObject
    {
        public GameObject projectilePrefab;
        /// In seconds
        public float cooldown;
        public Color hardpointColor = Color.white;
        public int numShots = 1;
        ///In Degrees
        public float shotSpacing = 15f;
    }
}