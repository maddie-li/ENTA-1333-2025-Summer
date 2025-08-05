using UnityEngine;

namespace RTS_1333
{
    public abstract class Unit : MonoBehaviour
    {
        [Header("Unit")]
        [SerializeField] public UnitType unitType;
        [SerializeField] public string Name;
        protected Damageable dmg;
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public GridNode CurrentNode { get; protected set; }

        public virtual int Width => unitType != null ? unitType.Width : 1;
        public virtual int Length => unitType != null ? unitType.Length : 1;

        public virtual int Cost => unitType != null ? unitType.Cost : 1;

        public virtual int MaxHP => unitType != null ? unitType.MaxHP : 10;

        private int currentHP;

        public int CurrentHP => currentHP;

        public Army Army;

        public void Initialize(GridNode node)
        {
            Name = gameObject.name;
            GridPosition = node.GridPosition;
            WorldPosition = node.WorldPosition;
            CurrentNode = node;
            
        }
        protected void InitDamage()
        {
            if (TryGetComponent<Damageable>(out dmg))
            {

                if (TryGetComponent<Animator>(out Animator animator))
                {
                    dmg.Initialize(unitType.MaxHP, animator);
                }
                else
                {
                    dmg.Initialize(unitType.MaxHP);
                }
                    
            }
            
        }

        public void TakeDamage(int _dmg)
        {
            if (dmg == null)  return;

            dmg.TakeDamage(_dmg);

            if(this.GetType() == typeof(Combatant))
            {
                FXManager.Instance.DoFX(FXType.CombatantDamage, this.transform.position);
            }
            else if (this.GetType() == typeof(Building))
            {
                FXManager.Instance.DoFX(FXType.BuildingDamage, this.transform.position);
            }


        }

        public bool IsFootprintOccupied()
        {
            if (CurrentNode == null) return false;
            return GridManager.Instance.IsFootprintOccupied(CurrentNode, Width, Length);
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
