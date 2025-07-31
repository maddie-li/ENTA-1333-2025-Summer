using UnityEngine;
using UnityEngine.UIElements;

namespace RTS_1333
{
    public enum UnitType
    {
        Combatant,
        Building
    }
    public class Unit : MonoBehaviour, ISelectableObject
    {
        [Header("Unit")]
        [SerializeField] public UnitData UnitData;
        [SerializeField] public string Name;

        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public GridNode CurrentNode { get; private set; }

        public int Width => UnitData != null ? UnitData.Width : 1;
        public int Length => UnitData != null ? UnitData.Length : 1;
        public int Cost => UnitData != null ? UnitData.Cost : 1;
        public int MaxHP => UnitData != null ? UnitData.MaxHP : 10;
        public int CurrentHP => currentHP;
        private int currentHP;

        public Army Army => UnitData.Army;

        [Header("Behaviours")]
        public bool CanCombat => UnitData.CombatEnabled;
        public Combat combat;
        public bool CanMove => UnitData.MovementEnabled;
        public Movement movement;
        public bool CanAttack => UnitData.AttackEnabled;
        public Attack attack;
        public bool CanDamaged => UnitData.DamageEnabled;
        public Damage damage;
        public bool CanPlace => UnitData.PlacementEnabled;
        public Placement placement;
        public bool CanSpawn => UnitData.SpawningEnabled;
        public Spawner spawner;


        private Renderer[] renderers;
        private Material defaultMat;
        private Material validMat;
        private Material invalidMat;
        private Material selectedMat;

        public void Initialize(GridNode node)
        {
            Name = gameObject.name;
            GridPosition = node.GridPosition;
            WorldPosition = node.WorldPosition;
            CurrentNode = node;
        }
        private void OnValidate()
        {
            movement.gameObject.SetActive(CanMove);
            attack.gameObject.SetActive(CanAttack);
            damage.gameObject.SetActive(CanDamaged);
            placement.gameObject.SetActive(CanPlace);
        }

        public void TakeDamage(int amount)
        {
            if (damage == null) return;

            damage.TakeDamage(amount);

            /*if (this is Combatant)
                FXManager.Instance.DoFX(FXType.CombatantDamage, transform.position);
            else if (this is Building)
                FXManager.Instance.DoFX(FXType.BuildingDamage, transform.position);*/
        }

        public bool IsFootprintOccupied()
        {
            return CurrentNode != null &&
                   GridManager.Instance.IsFootprintOccupied(CurrentNode, Width, Length);
        }

        public void SetNodePos(GridNode newNode)
        {
            UpdateCurrentNode(newNode);

            Vector3 centerOffset = new Vector3((Width - 1) * 0.5f, 0f, (Length - 1) * 0.5f);
            Vector3 centeredPosition = newNode.WorldPosition + centerOffset;

            WorldPosition = centeredPosition;
            transform.position = WorldPosition;
        }

        public void UpdateCurrentNode(GridNode newNode)
        {
            if(CanPlace && placement.IsGhost)
                CurrentNode = newNode;

            if (newNode.CurrentUnit != null) return;

            if (CurrentNode != null)
                CurrentNode.CurrentUnit = null;

            CurrentNode = newNode;
            newNode.CurrentUnit = this;
        }



        // VISUALISATION ---------------------------------------------------------------
        public void SetupMat()
        {
            renderers = GetComponentsInChildren<Renderer>();
            if (renderers == null) Debug.Log("Renderers are null");

            switch (UnitData.UnitType)
            {
                case UnitType.Combatant:
                    defaultMat = UnitManager.Instance.combatantMaterials[(int)Army];
                    validMat = UnitManager.Instance.combatantMaterials[3];
                    invalidMat = UnitManager.Instance.combatantMaterials[4];
                    selectedMat = UnitManager.Instance.combatantMaterials[5];
                    break;
                case UnitType.Building:
                    defaultMat = UnitManager.Instance.buildingMaterials[(int)Army];
                    validMat = UnitManager.Instance.combatantMaterials[3];
                    invalidMat = UnitManager.Instance.combatantMaterials[4];
                    selectedMat = UnitManager.Instance.combatantMaterials[5];
                    break;
            }

            foreach (var rend in renderers)
            {
                if (rend.material != null)
                    rend.material = defaultMat;
            }
        }

        public void SetSelected(bool selected)
        {
            Debug.Log("Updating Unit material");
            if (renderers == null) Debug.Log("Renderers are null");

            Material mat = selected ? selectedMat : defaultMat;

            foreach (var rend in renderers)
            {
                if (rend.material != null)
                    rend.material = mat;
            }
        }
    }
}
