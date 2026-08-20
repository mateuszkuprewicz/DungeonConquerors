This document provides a comprehensive overview of the system architecture, design patterns, and network communication flow for the multiplayer console game project I made during OOP and Design Patterns college course.


The project requires .NET 10 SDK installed. To start the program:  
git clone https://github.com/mateuszkuprewicz/DungeonConquerors  
cd DungeonConquerors  
dotnet run  
The console interface will guide you through the remaining steps and configuration options.  


The architecture is strictly separated into Client, Server, and Shared components.  

A. Model  
The core game logic and state reside entirely on the Server within the Server/Model/ directory making the Server the single source of truth. The model leverages multiple GoF design patterns to handle the complexity of RPG mechanics without tightly coupling the components.

1. Hero and ActionState (State Pattern)
   
   The Hero's [Server/Model/Hero/Hero.cs] behavior changes dynamically based on whether they are exploring the dungeon or engaged in a fight. This is managed via the State pattern located in Server/Model/ActionState/.
   - Context: Server/Model/ActionState/ActionStateContext.cs holds the current state.
   - States: Server/Model/ActionState/States/ExplorationState.cs and Server/Model/ActionState/States/CombatState.cs implement IActionState.cs.
   - Depending on the active state, ModelCommand inputs are processed differently [Server/Controller/GameCommand/]. ExplorationCommands (like moving or picking up items) are valid in ExplorationState, whereas CombatCommands (like hitting) are valid in CombatState.

2. Items and Weapon Decorators

   Items, specifically weapons, receive dynamic statistics and attributes using the Decorator pattern. This allows adding properties (like increasing Hero's strength) without creating a massive inheritance tree.
   - Interfaces/Base: Server/Model/Items/Weapon/IWeaponDecorated.cs and Server/Model/Items/Weapon/AbstractWeapon.cs.
   - Decorators: The base decorator is Server/Model/Items/Weapon/Decorator/AbstractWeaponDecorator.cs, and specific implementations are found in Server/Model/Items/Weapon/Decorator/Concrete Weapon Decorators.cs.

3. Attack Type and Damage Calculation (Visitor Pattern) 

   Calculating damage based on weapon types and attack types (e.g., normal, magic, stealth) is separated from the weapon objects themselves using the Visitor pattern cooperating with IWeaponDecorated interface and AbstractWeaponDecorator.
   - Visitor Interface: Server/Model/AttackTypeVisitor/AttackTypesVisitor/IAttackVisitor.cs
   - Concrete Visitors: MagicAttack.cs, NormalAttack.cs, StealthAttack.cs.
   - Bonus Calculation: Server/Model/AttackTypeVisitor/CalculateBonusDamageVisitor/BonusDamageVisitor.cs.
   - The weapon accepts the visitor, allowing the visitor to apply the correct damage multiplier or mechanic based on the specific type of weapon passed.

4. Sound Propagation (Mediator Pattern) 

   Actions in the dungeon (like combat or dropping items) create noise that alerts nearby enemies. This is handled centrally by a mediator to prevent items and enemies from keeping direct references to each other.
   - Mediator: Server/Model/SoundPropagation/SoundMediation/DungeonSoundManager.cs
   - Components: ISoundMaker.cs, ISoundHearer.cs, and NoiseEvent.cs
   - When a weapon is fired or an item drops, it acts as an ISoundPublisher and sends a NoiseEvent to the DungeonSoundManager. The manager then performs a graph-based search (BFS algorithm) across the dungeon map to find all subscribers (ISoundHearers which is in my case: enemies [Server/Model/Enemies]) within the event's reach radius, notifying them of the player's position.

5. Enemy Mowing AI

   Enemies utilize the State pattern to determine their movement logic depending on external stimuli (like hearing a NoiseEvent).
   - Interfaces: Server/Model/Enemies/MovingAI/IMovingEnemy.cs
   - States: Server/Model/Enemies/MovingAI/MovingStates/AbstractMovingState.cs
   - Concrete States: RandomMoving.cs (default wandering) and TargetedMoving.cs (moving towards the source of a noise).
   - Enemies transition from RandomMoving to TargetedMoving once they are notified by the DungeonSoundManager.

6. Map Generation (Builder & Strategy Patterns)

   The dungeon layout and aesthetic are generated through a combination of the Builder and Strategy patterns, allowing the creation of varied environments.
   - Director & Builder: Server/Model/Map/MapGenerator/MapDirector.cs orchestrates the construction steps defined in MapBuilder.cs.
   - Themes (Strategy): Server/Model/Map/Dungeon Themes/IDungeonTheme.cs acts as the strategy interface. Concrete strategies include ColonyTheme.cs, FrozenLand.cs, and TaxOfficeTheme.cs. The director applies these themes to determine the specific tiles, obstacles, and aesthetics of the generated GameMap.cs.

B. Network and Architecture

The project guarantees deterministic execution via routing everything through specialized Thread-Safe queues and factories. The key observation wa the fact is that the state of the game is fully determined by the sequence of detailed atomic commands like: single move of the enemy to neighboring square, such single move of a Player, Players single hit, etc. The time interval between those actions doesn't affect the final state of the game. Hence the usage of TCP protocol. 

1. Server Architecture

   The Server reads input, processes it safely against the model, and broadcasts views back to clients.

   ![UML Server Simplified.png](UML%20Server%20Simplified.png)

- Receiving Input: Server/NetworkInfrastructure/ClientReader.cs continuously listens for incoming client byte streams, utilizing ClientLifeManager.cs to handle connection states.
- Translating to Actions: The raw requests are processed by the Factory pattern in Server/Controller/NetworkController/ModelCommandFactory.cs. This factory evaluates the request and creates the appropriate concrete IModelCommand (e.g., ExplorationCommands/MoveHeroCommand.cs or CombatCommands/HitCommand.cs).
- Deterministic Queues (_modelCommands and _viewCommands):
  - To maintain strict determinism, network threads do not directly mutate the game state. Instead, ModelCommandFactory pushes commands into the _modelCommands concurrent queue.
  - The main Server/Controller/GameLoop.cs sequentially dequeues _modelCommands, applies them to the Model, and subsequently generates view updates pushed into the _viewCommands queue.
- Broadcasting: Server/View/RenderDispatcher.cs drains the _viewCommands queue. It utilizes command classes implementing Server/View/ViewCommand/IViewCommand.cs (like MapDeltaCommand.cs, PlayerCreationCommand.cs, SendMapViewCommand.cs) to format and push data via Server/View/ClientView.cs to the clients.

2. Client Architecture

   The Client is a thin architecture designed strictly to capture input, render received states, and avoid local game logic prediction.
![UML CLlient Simplified.png](UML%20CLlient%20Simplified.png)

- Reading Server State: Client/NetworkInfrastructure/NetworkReader.cs pulls serialized view commands from the network stream.
- Command Recognition (Factory): Client/Controller/NetworkController/DeserialisingDtoFactory.cs parses the JSON payloads and matches them to the correct handler executing Client/Controller/NetworkController/MessageHandlers/IMessageHandler.cs (such as MapDeltaHandler.cs, InitHandler.cs, or LogMessageHandler.cs).
- Input Handling (Chain of Responsibility): Client keystrokes are processed through a Chain of Responsibility located in Client/Controller/KeyController/KeyNodes/.
  - Base: AbstractKeyNode.cs
  - Exploration Chain: ChainOfExploration/MoveNode.cs, PickDropNode.cs, EquipmentScrollNode.cs, Sentinel.cs.
  - Combat Chain: ChainOfFight/ChainOfKeyOperations/HitNode.cs, LeaveNode.cs.
- If a key corresponds to an exploration action, the MoveNode handles it and breaks the chain; if not, it passes the key to the next node.

C. DTOs and JSON Serialization

   To communicate seamlessly, the project uses Data Transfer Objects (DTOs) serialized into JSON.
- Location: Shared/ClientServerCommunication/
- Client Requests: ClientRequests/ClientRequests.cs and ClientRequestsTypes.cs map directly to the JSON structure sent by the client.
- Server Broadcasts: ServerRequests/ClientGameInit/GameInitPacket.cs and ServerRequests/GameChangedBroadcast/AnwsersToClients.cs are used to package the delta of the game state into a JSON string before dispatching it over the TCP socket.

D. Logger

The logging system is robust, tracking game events and server diagnostics while being safely accessible across threads.

1. Singleton Pattern: Shared/Logger/EventLog.cs implements the Singleton pattern, ensuring that all subsystems (combat, movement, network) write to a unified, thread-safe logging instance without file lock conflicts.
2. Strategy Pattern: The formatting and storage of logs use the Strategy pattern via Shared/Logger/ISavingLogsStrategy.cs. This would allow the server to dynamically switch between logging to the console,.txt journal file or a database, without changing the EventLog class.
3. Client-Side Rendering: The server broadcasts specific game logs (like damage taken or items found) to the clients. On the client side, Client/View/LogRenderer.cs takes these strings and formats them into the dedicated log window on the console UI, keeping the player informed of game events asynchronously.