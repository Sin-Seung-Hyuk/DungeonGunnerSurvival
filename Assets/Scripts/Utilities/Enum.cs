
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
    ReloadSpeed,
    FireRate,
    MoveSpeed,
    CircleRadius,
    ExpGain
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