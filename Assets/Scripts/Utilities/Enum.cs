
public enum PlayerCharacter
{
    TheGeneral,
    TheThief,
    TheScientist
}

public enum PlayerStatType
{
    MaxHP,
    BaseDamage,
    BaseArmor,
    Dodge,
    CriticChance,
    CriticDamage,
    MoveSpeed,
    CircleRadius,
    ExpGain,
    ReloadSpeed,
    FireRate
}


public enum GameState
{
    InEntrance,
    Paused,
    InDungeon,
    DungeonRoomClear,
    DungeonCompleted,
    GameCompleted,
    GameLost,
    RestartGame
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}

public enum SlotType
{
    NonEquipmentSlot,
    EquipmentSlot,
}