using System;

public enum State
{
  DEAD,
  GROUNDED,
  JUMPING
};

public abstract class PlayerState
{
  public State state;
  protected Player Player;

  public PlayerState(Player player)
  {
    Player = player;
  }

  public event Action<Player> OnEnter;
  public event Action<Player> OnExit;

  public virtual void Enter()
  {
    OnEnter?.Invoke(Player);
  }

  public abstract State? CustomUpdate();

  public virtual void Exit()
  {
    OnExit?.Invoke(Player);
  }
}