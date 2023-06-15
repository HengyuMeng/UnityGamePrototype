using Infrastructure;

namespace Infrastructure
{
    public static partial class ActionID
    {
        public static class Stage
        {
            public const string Attack = "Attack";
        }
    }
    
}

namespace Gameplay.Events
{
    /// <summary>
    /// 攻击
    /// </summary>
    public class Attack : IAction
    {
        public Identity.Camps Camp;
        public Identity.Roles Role;
        public string ActionName() => ActionID.Stage.Attack;
    }
}