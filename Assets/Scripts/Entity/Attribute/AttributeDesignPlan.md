#AttributeDesignPlan
##character属性设计案
每一个characterController从被分配的（由manager执行分配）的data文件里初始化CharacterAttribute/EnemyAttribute的属性，而在战斗过程中所有的数据修改都是修改这个ICharacterAttribute的子类的属性。
注意：ICharacter只修改attribute，data是只读文件