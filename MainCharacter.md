# Code Pattern
- CharacterClass: 
  - Quản lý Current State, Switch giữa các State
  - Save, Load Character Data
  - Những chức năng share các State Update.
- Character State Base:
  - Base Class cho tất cả state với mục đích Polymophism swith state trong Character Class
- Character Data:
  - Các configure của các State nằm trong này để quản lý phần Init, Configure và Change State Data
- AnimStatesBehavior:
  - Là StateBehavior của Unity để sync giữa Animation State và Character State. 

# Class Diagram

![image](https://user-images.githubusercontent.com/1218572/211292103-e1fed3e9-c5d4-40db-bb88-506b1aad9f22.png)


```
@startuml
class CharacterClass {
  CharacterStateBase currentState;
  CharacterData characterData; 
  GetInputs();
  currState.UpdateState();
  UpdateCamera();
}
class CharacterStateBase {
  EnterState()
  ExitState()
  UpdateState()
}

class CharacterData 
{
 Save()
}
CharacterClass --> CharacterStateBase: has
CharacterClass --> CharacterData : has

CharacterStateBase <|-- JumpState : is
CharacterStateBase <|-- FallingState: is

package AnimStatesBehavior {
 StateMachineBehaviour <|-- IdleStateBehavior: is
 IdleStateBehavior --> CharacterClass: ChangeState
 
}


@enduml
```

