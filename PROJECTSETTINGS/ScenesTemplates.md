# 📋 PLANTILLAS DE ESCENAS

## Escena 1: MainMenu.unity
### Objetos en escena:
- Canvas_MainMenu
  - Text_Title (Captain Tsubasa)
  - Button_StartGame
  - Button_TeamSelect
  - Button_Options
  - Button_Exit
  
## Escena 2: TeamSelection.unity
- Canvas_TeamSelect
  - Panel_TeamHome (Japón)
  - Panel_TeamAway (Brasil/Argentina/Alemania)
  - Button_Confirm
  - Button_Back

## Escena 3: MatchScene.unity
### Capas (Layers):
1. Default
2. Players
3. Ball
4. Goals
5. UI

### Objetos principales:
- GameManager (vacío)
- Field (Sprite campo de fútbol)
- Goal_Home (Portería izquierda)
- Goal_Away (Portería derega)
- Player_Home_1 a Player_Home_11
- Player_Away_1 a Player_Away_11
- Ball (centro del campo)

## Escena 4: Results.unity
- Canvas_Results
  - Text_Winner
  - Text_Score
  - Button_Rematch
  - Button_MainMenu
  - Button_NextMatch
