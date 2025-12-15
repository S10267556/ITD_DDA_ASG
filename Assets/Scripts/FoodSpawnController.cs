using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Templates.AR;

public class FoodSpawnController : MonoBehaviour
    {
        [SerializeField]
        private ObjectSpawner xrSpawner;

        [SerializeField]
        private DatabaseController databaseController;

        [SerializeField]
        private ARTemplateMenuManager arMenuManager;

        public GameObject spawnedFood;

       public GameObject TrySpawnFood(Vector3 spawnPoint, Vector3 spawnNormal)
        {
            if (databaseController == null)
            {
                Debug.LogWarning("DatabaseController reference is missing!");
                return null;
            }

            if (xrSpawner == null)
            {
                Debug.LogWarning("XRSpawner reference is missing!");
                return null;
            }

            // Prevent multiple foods being placed at once
            if (databaseController.foodPlaced)
                return null;

            // Check if player has enough energy
            if (databaseController.energy <= 0)
            {
                databaseController.noEnergy();
                return null;
            }

            // Check if the selected food is available and player has enough coins
            if (xrSpawner.spawnOptionIndex == 0 && (databaseController.almondAmt <= 0 || databaseController.coins < databaseController.almondCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 1 && (databaseController.broccoliAmt <= 0 || databaseController.coins < databaseController.broccoliCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 2 && (databaseController.caffeineAmt <= 0 || databaseController.coins < databaseController.caffeineCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 3 && (databaseController.carrotAmt <= 0 || databaseController.coins < databaseController.carrotCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 4 && (databaseController.onionAmt <= 0 || databaseController.coins < databaseController.onionCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 5 && (databaseController.strawberryAmt <= 0 || databaseController.coins < databaseController.strawberryCost))
                return null;

            if (xrSpawner.spawnOptionIndex == 6 && (databaseController.sunflowerSeedsAmt <= 0 || databaseController.coins < databaseController.sunflowerSeedsCost))
                return null;

            // Prevent invalid selection
            if (xrSpawner.spawnOptionIndex < 0 || xrSpawner.spawnOptionIndex > 6)
            {
                Debug.LogWarning("Invalid spawnOptionIndex!");
                return null;
            }

            // Spawn the selected food
            GameObject spawnedFood = xrSpawner.TrySpawnObject(spawnPoint, spawnNormal);
            return spawnedFood; // null if spawn failed
        }


    }
