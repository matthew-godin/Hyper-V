using XNAProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace HyperV
{
    public class GamePadManager : Microsoft.Xna.Framework.GameComponent
    {
        GamePadCapabilities Capacities { get; set; }
        GamePadState PreviousGamePadState { get; set; }
        GamePadState CurrentGamePadState { get; set; }

        public GamePadManager(Game game)
         : base(game)
        { }

        public override void Initialize()
        {
            Capacities = GamePad.GetCapabilities(PlayerIndex.One);
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            PreviousGamePadState = CurrentGamePadState;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            PreviousGamePadState = CurrentGamePadState;
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsGamePadActivated//update capacities?
        {
            get { return Capacities.IsConnected; }
        }

        public Vector2 TriggerPositions
        {
            get { return new Vector2(CurrentGamePadState.Triggers.Left, CurrentGamePadState.Triggers.Right); }
        }

        public Vector2 PositionThumbStickLeft
        {
            get { return new Vector2(CurrentGamePadState.ThumbSticks.Left.X, CurrentGamePadState.ThumbSticks.Left.Y); }
        }

        public Vector2 PositionThumbStickRight
        {
            get { return new Vector2(CurrentGamePadState.ThumbSticks.Right.X, CurrentGamePadState.ThumbSticks.Right.Y); }
        }

        public bool IsPressed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button);
        }

        public bool IsNewButton(Buttons button)
        {
            return PreviousGamePadState.PacketNumber != CurrentGamePadState.PacketNumber &&
                   CurrentGamePadState.IsButtonDown(button);
        }
    }
}