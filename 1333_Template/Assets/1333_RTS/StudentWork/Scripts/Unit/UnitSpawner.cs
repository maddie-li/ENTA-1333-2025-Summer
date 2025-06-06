using UnityEngine;

namespace RTS_1333
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2Int[] nodePosition;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TestSpawn();
        }

        private void TestSpawn()
        {
            foreach (Vector2Int pos in nodePosition)
            {
                GridNode node = gridManager.GetNode(pos);
                if (node == null || node.CurrentUnit != null) return;

                GameObject unitObject = Instantiate(prefab);
                UnitInstance unit = unitObject.GetComponent<UnitInstance>();

                if (unit != null)
                {
                    unit.Initialize(pathfinder, gridManager);
                    unit.SetNodePos(node);
                }
            }

            
        }
    }
}
