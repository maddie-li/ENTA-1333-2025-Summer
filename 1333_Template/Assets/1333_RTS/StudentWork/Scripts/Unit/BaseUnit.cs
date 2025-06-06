using UnityEngine;

namespace RTS_1333
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] protected UnitType unitType;

        public string Name { get; private set; }
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public GridNode CurrentNode { get; private set; }

        public virtual int Width => unitType != null ? unitType.Width : 1;
        public virtual int Length => unitType != null ? unitType.Length : 1;

        public void Initialize(GridNode node)
        {
            Name = gameObject.name;
            GridPosition = node.GridPosition;
            WorldPosition = node.WorldPosition;
            CurrentNode = node;

        }

        public virtual void SetNodePos(GridNode newNode)
        {
            UpdateCurrentNode(newNode);

            WorldPosition = newNode.WorldPosition;
            transform.position = WorldPosition;
        }

        public virtual void UpdateCurrentNode(GridNode newNode)
        {
            if (newNode.CurrentUnit != null) return;

            if (CurrentNode != null)
                CurrentNode.CurrentUnit = null;

            CurrentNode = newNode;
            newNode.CurrentUnit = this;
        }
    }
}
