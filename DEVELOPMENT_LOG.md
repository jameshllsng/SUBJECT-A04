# DEVELOPMENT_LOG

## 2026-06-09

### Concluido

- Revisados `PROJECT_RULES.md` e `GAME_DESIGN.md` como referencias principais.
- Criado `Assets/Scripts/AI/BasicEnemyController.cs`.
- Implementados deteccao, perseguicao direta, rotacao, ataque, dano e cooldown.
- Convertido `TargetDummy` da `MainScene` em `BasicEnemy`.
- Configurado o Player como alvo e marcado com a tag `Player`.
- Mantida a integracao modular por `IDamageable`.
- Confirmado que o inimigo continua com `Health` e pode morrer apos quatro tiros.
- A morte do jogador usa o fluxo existente de `Health.Die()`.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Fase 1 validada em Play Mode: perseguicao, ataque, dano e morte funcionam.

### Proxima prioridade

- Validar o consumivel de cura da Fase 2 em Play Mode.
- Iniciar a Fase 3 com interacao e loot simples.

## 2026-06-10

### Concluido

- Criada a pasta `Assets/Scripts/Items`.
- Criado `HealingConsumable.cs`.
- Adicionados tres usos de cura ao Player.
- Configurada cura de 30 pontos pela tecla `H`.
- O consumivel nao e gasto quando a vida esta cheia.
- Exposto o metodo publico `Use()` para integracao futura com inventario.
- Corrigido `Health.Heal()` para registrar somente a vida realmente recuperada.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Cura validada em Play Mode: dano, tecla `H`, limite maximo e consumo funcionam.

### Proxima prioridade

- Validar o HUD de vida em Play Mode.
- Iniciar a Fase 3 com um sistema minimo de interacao e coleta de loot.

### HUD de vida

- Criado `Assets/Scripts/UI/HealthHUD.cs`.
- Adicionada barra de vida do Player no canto inferior esquerdo.
- Adicionada barra de vida do inimigo no topo central.
- As barras exibem preenchimento e valores numericos atuais.
- A barra do inimigo desaparece quando o inimigo e destruido.
- O HUD usa Unity UI e e construido em runtime sem alterar a logica de gameplay.

### Validacao do HUD

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- HUD inicial validado em Play Mode.

### Indicador temporario do inimigo

- Removida a barra de vida fixa do inimigo no topo da tela.
- Criado `Assets/Scripts/UI/EnemyHealthBar.cs`.
- A barra agora aparece em World Space acima do inimigo e acompanha seu movimento.
- O indicador permanece voltado para a camera e desaparece junto com o inimigo.
- `HealthHUD.cs` agora exibe somente a vida do Player.
- Esta barra e uma ferramenta temporaria de prototipagem e devera ser removida na fase de imersao/polimento.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Teste visual em Play Mode ainda necessario para ajustar altura e escala.

### Fase 3 - Interacao e loot

- Criada a pasta `Assets/Scripts/Interactions`.
- Criada a interface `IInteractable`.
- Criado `PlayerInteractor.cs` com Raycast central, alcance configuravel e tecla `E`.
- Adicionado prompt contextual temporario na tela.
- Criado `HealingPickup.cs`.
- Adicionado `HealingConsumable.AddUses()` para receber itens coletados.
- Adicionado um `MedkitPickup` prototipo na `MainScene`.
- Ao coletar o kit medico, o Player recebe um uso adicional de cura e o objeto e destruido.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Fase 3 validada em Play Mode: prompt, interacao e coleta funcionam.

### Proxima prioridade

- Validar o inventario basico em Play Mode.
- Iniciar a Fase 5 com equipamentos simples.

### Fase 4 - Inventario basico

- Criado `Assets/Scripts/Inventory/PlayerInventory.cs`.
- Implementado armazenamento generico por ID, nome e quantidade.
- Implementadas operacoes de adicionar, remover e consultar itens.
- Criado `Assets/Scripts/Inventory/InventoryUI.cs`.
- O painel do inventario pode ser aberto e fechado pela tecla `Tab`.
- O consumivel de cura agora consulta e remove kits medicos do inventario.
- O pickup de cura agora adiciona um item ao inventario.
- O Player inicia com tres kits medicos.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Fase 4 validada em Play Mode: painel, coleta e consumo atualizam as quantidades.

### Proxima prioridade

- Validar quantidades do inventario durante coleta e consumo.
- Iniciar a Fase 5 com um primeiro sistema simples de equipamentos.

### Refinamento do inventario e feedback

- Alterada a tecla do inventario de `I` para `Tab`.
- Criado `Assets/Scripts/UI/HUDNotification.cs`.
- Adicionada notificacao quando o Player tenta usar cura com vida cheia.
- Adicionada notificacao quando nao existem kits medicos no inventario.
- Adicionada notificacao ao coletar um item.
- As mensagens desaparecem automaticamente apos dois segundos.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- `Tab` e as notificacoes foram validados em Play Mode.

### Padronizacao de idioma

- Todos os textos exibidos durante o gameplay foram padronizados em ingles.
- As notificacoes de vida cheia, falta de kits e coleta de item agora usam ingles.
- A documentacao interna permanece em portugues.

### Fase 5 - Equipamentos

- Criada a pasta `Assets/Scripts/Equipment`.
- Criada a interface `IDamageModifier`.
- Criado `EquipmentManager.cs` com um slot inicial de armadura.
- Criado `ArmorPickup.cs`.
- Adicionado um `FieldVestPickup` prototipo na `MainScene`.
- O colete e equipado automaticamente pela interacao com `E`.
- O Field Vest reduz em 25% o dano recebido pelo Player.
- O inventario exibe a secao `EQUIPPED` com a armadura e sua protecao.
- O calculo de dano do `Health` agora aceita modificadores desacoplados.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Teste em Play Mode ainda necessario: equipar o colete e confirmar dano de 7.5 por ataque.

### Proxima prioridade

- Validar o equipamento e a reducao de dano.
- Iniciar a Fase 6 com uma primeira melhoria de IA.

### Controles do prototipo e ciclo de morte

- A perseguicao do inimigo agora inicia somente apos interagir com `ACTIVATE ENEMY`.
- O ataque por proximidade continua ativo antes da perseguicao ser liberada.
- Criado `PrototypeWorldController.cs` para centralizar resets temporarios da cena.
- Criados controles interativos para resetar o inimigo e os itens coletaveis do chao.
- Pickups coletados agora ficam inativos para poderem ser restaurados.
- O inimigo volta para sua posicao inicial, recupera toda a vida e interrompe a perseguicao ao ser resetado.
- Criado menu de morte com as opcoes `RESPAWN` e `RESET ENEMY`.
- O respawn restaura a vida, posicao, rotacao, controle do jogador e estado do cursor.
- O inventario aberto e fechado automaticamente quando o jogador morre.

### Validacao

- `dotnet build "SUBJECT-A04.sln" --no-restore`: 0 erros e 0 avisos.
- Referencias dos novos componentes e controles adicionadas a `MainScene`.
- Ciclo completo validado em Play Mode: ativacao, dano por contato, perseguicao, morte, respawn e resets funcionam.

### Proxima prioridade

- Prosseguir para a primeira melhoria de IA da Fase 6.
