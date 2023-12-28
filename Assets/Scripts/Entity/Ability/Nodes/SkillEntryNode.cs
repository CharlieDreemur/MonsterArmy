using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Physalia.Flexi;
namespace MonsterEnemy.SkillNodeSystem
{
    [NodeCategory("MonsterEnemy")]
    public class SkillEntryNode : EntryNode
    {
        public class Payload : IEventContext
        {
            public MainManager mainManager;
            public Unit owner;
        }

        private enum State
        {
            INITIAL, SELECTION, COMPLETE,
        }

        public Outport<FlowNode> selectionPort;
        public Outport<MainManager> mainManagerPort;
        public Outport<Unit> unitPort;
        private State state = State.INITIAL;


        public override FlowNode Next
        {
            get
            {
                if (state == State.SELECTION)
                {
                    IReadOnlyList<Port> connections = selectionPort.GetConnections();
                    return connections.Count > 0 ? connections[0].Node as FlowNode : null;
                }
                else if (state == State.COMPLETE)
                {
                    return base.Next;
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool CanExecute(IEventContext payloadObj)
        {
            var payload = payloadObj as Payload;
            if (payload == null)
            {
                return false;
            }

            //int mana = payload.player.Mana;
            /*int cost = payload.card.GetStat(StatId.COST).CurrentValue;
            if (mana < cost)
            {
                return false;
            }
            */
            return true;
        }

        protected override AbilityState DoLogic()
        {
            var payload = GetPayload<Payload>();

            if (state == State.INITIAL)
            {
                if (selectionPort.GetConnections().Count > 0)
                {
                    state = State.SELECTION;
                    PushSelf();
                }
                else
                {
                    state = State.COMPLETE;
                    PayCosts(payload);
                }
            }
            else if (state == State.SELECTION)
            {
                state = State.COMPLETE;
                PayCosts(payload);
            }

            mainManagerPort.SetValue(payload.mainManager);
            unitPort.SetValue(payload.owner);
            return AbilityState.RUNNING;
        }

        private void PayCosts(Payload payload)
        {
            return;
            /*
            int cost = payload.card.GetStat(StatId.COST).CurrentValue;
            payload.player.Mana -= cost;

            EnqueueEvent(new ManaChangeEvent
            {
                modifyValue = -cost,
                newAmount = payload.player.Mana,
            });

            EnqueueEvent(new PlayCardEvent
            {
                card = payload.card,
            });
            */
        }

        protected override void Reset()
        {
            state = State.INITIAL;
        }
    }
}