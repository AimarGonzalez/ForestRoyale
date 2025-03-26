using UnityEngine;
using Raven.Attributes;

namespace ForestRoyale.Gameplay.Cards
{
    public abstract class CardData : ScriptableObject
    {
        [BoxGroup("Basic Info")]
        [Tooltip("The name of the card")]
        [SerializeField] protected string _cardName;
        
        [BoxGroup("Basic Info")]
        [Tooltip("Description of the card")]
        [SerializeField] [TextArea(3, 6)] protected string _description;
        
        [BoxGroup("Basic Info")]
        [Tooltip("Card portrait image")]
        [SerializeField] protected Sprite _portrait;
        
        [BoxGroup("Battle Stats")]
        [Tooltip("Cost to deploy this card")]
        [SerializeField] protected int _elixirCost;
        
        [BoxGroup("Battle Stats")]
        [Tooltip("Card rarity")]
        [SerializeField] protected CardRarity _rarity;
        
        [BoxGroup("Battle Stats")]
        [Tooltip("Arena where this card is first available")]
        [SerializeField] protected int _arenaUnlock;
        
        [BoxGroup("Development")]
        [Tooltip("Current development status")]
        [SerializeField] protected DevelopmentStatus _status = DevelopmentStatus.InDevelopment;

        // Public getters for properties
        public string CardName => _cardName;
        public string Description => _description;
        public Sprite Portrait => _portrait;
        public int ElixirCost => _elixirCost;
        public CardRarity Rarity => _rarity;
        public int ArenaUnlock => _arenaUnlock;
        public DevelopmentStatus Status => _status;
    }
    
    public enum CardRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Champion
    }
    
    public enum DevelopmentStatus
    {
        Concept,
        InDevelopment,
        Testing,
        Ready,
        Released
    }
} 