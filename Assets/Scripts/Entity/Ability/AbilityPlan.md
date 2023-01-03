# 关于技能的制作计划案
目前的计划是使用github上的技能插件 GamePlayAbility 进行创作，之后可修改定制，or重搭框架


## 2022.4.2
目前打算自己重新做一个，锻炼技术了属于是
每个hero有个abilityList, 由abilityState AI判断是否可以释放技能
当释放技能时，调用Ability对应的GameEffect，gameEffect是技能的controller，它会创造相应的buff或projectile给对应的单位
创建完后，对应的buff和projectile会触发自身的循环周期和伤害
技能释放结束

## Ability State
---
- Ready - When Cooldown is 0

- Channel - [Optional] When the ability is preparing to release, it 
takes time for an ability from channel state to the release state

- Cast - When the ability is cast, which means all the GameEffects in the List will be activated one by one.

- Active - [Optional] When the ability is active and continously causes some effects

- Cooldown = When the ability is finished and turned into cooldown state, when the timer is ends up, the ability will again turn into the Ready State.

当ability正在遍历activate GameEffectList的时候，是Release State
遍历完毕后，则进入CoolDownState


## 5.29 Ability IsFinshed [已完成]

开始做技能的结束判断，也就是判断每个GameEffect是否都结束了技能的激活，如果都结束了，则将技能进入倒计时
The condition from Release to Cooldown [已完成,待优化]

ps:可以考虑用async 和 await来优化

## 5.30 AbilityState
技能的AI
流程：
1.checkingBool默认为true,一开始检查所有的技能，判断是否能够释放，如果能则return并激活第一个能够释放的
2.重复1步骤，直到确认所有技能都释放完毕，将checkingBool变为false
3.任何一个技能释放完毕时,将checkingBool变为true, 然后再进入第一阶段
问题是：如果是法力值不够呢，该如何优化效率，减少重复的检索？

- AbilityState
释放技能的时候，具体为释放检测到的可释放技能，并且当技能释放完毕（一般是等待一个施法动画）后切换到其他State

- ChannelState
吟唱技能的时候，此时是不能进入别的state的直到技能吟唱结束，或被打断1

- StunState
眩晕，此时无法做任何事

- SilentState
沉默，此时无法释放技能

- Invincible State
无敌状态，此时免疫所有伤害和Debuffz

## 8.4 
重新梳理了下技能AI的流程，目前是这样：
1.在其他的state时（idle,attack, move, chase)时会检查是否有冷却完毕且能够释放（判断法力值，以及范围内是否有目标）的技能，如果有会立即释放，切换到AbilityState
现在我们需要做的就是如何判断技能的范围内是有目标的，我们打算直接update()起手
存在的问题：如果使用update()每帧检测，会造成成吨的卡顿，优化不好，所以这里到时候需要优化

## 8.31
技能的功能拆解开来主要为以下四类：
1. 位移:主要分为相对自己的位移和相对目标的位移
2. Buff：

