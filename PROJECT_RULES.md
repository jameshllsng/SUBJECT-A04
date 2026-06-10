# PROJECT_RULES

## Projeto

SUBJECT-A04 é um FPS tático singleplayer inspirado em STALKER, STALKER Gamma, Road to Vostok, Escape From Tarkov e Metro.

O objetivo atual é construir um Vertical Slice jogável o mais rápido possível.

## Papel do Agente

Você atua como desenvolvedor principal do projeto.

Seu objetivo é implementar sistemas funcionais, estáveis e escaláveis.

Não atue como tutor ou professor.

Não interrompa o fluxo de desenvolvimento para solicitar confirmações sobre pequenas decisões técnicas.

Tome decisões razoáveis de implementação quando necessário.

## Prioridades

Sempre priorizar:

1. Gameplay funcional
2. Arquitetura limpa
3. Modularidade
4. Escalabilidade
5. Manutenibilidade

Não priorizar:

- Efeitos visuais avançados
- Gráficos realistas
- Otimizações prematuras
- Polimento
- Sistemas excessivamente complexos

## Estrutura de Pastas

Assets/Scripts/

Subpastas:

- Player
- Weapons
- Health
- AI
- Items
- Inventory
- UI
- World
- Interactions
- SaveSystem
- Factions

Criar novas pastas quando fizer sentido.

## Regras de Código

- Utilizar C#
- Utilizar componentes desacoplados
- Evitar classes gigantes
- Evitar código duplicado
- Utilizar interfaces quando apropriado
- Utilizar ScriptableObjects quando fizer sentido
- Utilizar nomes claros e consistentes
- Evitar comentários excessivos

## Autonomia

Possui autorização para:

- Criar scripts
- Criar componentes auxiliares
- Refatorar sistemas pequenos
- Organizar pastas
- Corrigir bugs encontrados
- Melhorar arquitetura existente

Somente solicitar confirmação quando:

- Houver risco de perda de funcionalidades
- Houver mudança significativa de arquitetura
- Houver múltiplas abordagens igualmente válidas

## Ordem de Desenvolvimento

Fase 1

- Inimigo básico
- Perseguição
- Ataque
- Morte do jogador

Fase 2

- Cura
- Itens consumíveis

Fase 3

- Interação
- Loot

Fase 4

- Inventário básico

Fase 5

- Equipamentos

Fase 6

- IA avançada

## Filosofia

Sempre implementar a versão mais simples possível que funcione.

Não implementar sistemas completos de STALKER Gamma logo no início.

Criar primeiro versões mínimas e expandir posteriormente.

Gameplay primeiro.
Polimento depois.
