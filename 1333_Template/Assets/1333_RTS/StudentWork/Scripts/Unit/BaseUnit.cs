using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RTS_1333
{
    public abstract class BaseUnit : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] protected UnitType unitType;

        public string Name;
        public Vector3 WorldPosition;       // position in 3D space
        public Vector2Int GridPosition;     // grid coordinates

        public GridNode CurrentNode;

        [Header("Size")]
        public virtual int Width => unitType != null ? unitType.Width : 1;
        public virtual int Length => unitType != null ? unitType.Length : 1;

        public void Initialize(GridNode node)
        {
            Name = gameObject.name;
            GridPosition = node.GridPosition;
            WorldPosition = node.WorldPosition;
            CurrentNode = node;

            MoveTo(node);
        }

        public virtual void MoveTo(GridNode newNode)
        {
            if (newNode.CurrentUnit != null)
            {
                Debug.Log("Move failed, node is occupied");
                return;
            }

            if (CurrentNode != null)
            {
                CurrentNode.CurrentUnit = null; // remove this from current node
            }

            CurrentNode = newNode;
            CurrentNode.CurrentUnit = this;

            // CHANGE POSITION
            gameObject.transform.position = WorldPosition;
        }
    }

}