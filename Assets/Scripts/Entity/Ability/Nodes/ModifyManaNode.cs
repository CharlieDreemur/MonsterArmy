namespace Physalia.Flexi.Samples.CardGame
{
    [NodeCategory("Card Game Sample")]
    public class ModifyManaNode : ProcessNode
    {
        public Inport<Unit> unitPort;
        public Inport<int> amountPort;

        protected override AbilityState DoLogic()
        {
            Unit player = unitPort.GetValue();
            int amount = amountPort.GetValue();
            //player.Mana += amount;
            /*
            EnqueueEvent(new ManaChangeEvent
            {
                modifyValue = amount,
                newAmount = player.Mana,
            });
            */
            return AbilityState.RUNNING;
        }
    }
}
