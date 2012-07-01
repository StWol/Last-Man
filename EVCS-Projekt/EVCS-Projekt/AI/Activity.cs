using LastMan;

namespace LastMan.AI
{
    public abstract class Activity
    {
        // Tue die Action
        public abstract void CalculateAction(Enemy e);

        // Tue die Action
        public abstract void DoActivity(Enemy e);

        public bool ShouldMove { get; set; }
    }
}
