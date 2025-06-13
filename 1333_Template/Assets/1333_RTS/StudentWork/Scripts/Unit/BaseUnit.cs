using UnityEngine;

namespace RTS_1333
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] protected UnitType unitType;

        [SerializeField] public string Name;
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public GridNode CurrentNode { get; protected set; }

        public virtual int Width => unitType != null ? unitType.Width : 1;
        public virtual int Length => unitType != null ? unitType.Length : 1;

        public void Initialize(GridNode node)
        {
            Name = gameObject.name;
            GridPosition = node.GridPosition;
            WorldPosition = node.WorldPosition;
            CurrentNode = node;

        }
        public bool IsFootprintOccupied(GridManager gridManager)
        {
            if (CurrentNode == null) return false;
            return gridManager.IsFootprintOccupied(CurrentNode, Width, Length);
        }

        public virtual void SetNodePos(GridNode newNode)
        {
            UpdateCurrentNode(newNode);

            Vector3 centerOffset = new Vector3((Width - 1) * 0.5f, 0f, (Length - 1) * 0.5f);
            Vector3 centeredPosition = newNode.WorldPosition + centerOffset;

            WorldPosition = centeredPosition;
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
