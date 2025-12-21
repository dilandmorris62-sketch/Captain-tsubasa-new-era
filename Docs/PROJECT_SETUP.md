# Configuración del Proyecto Captain Tsubasa Android

## Requisitos
- Unity 2021.3.11f1 o superior
- Android SDK instalado
- JDK 11 o superior

## Pasos de Configuración

### 1. Configuración de Unity
1. Abrir proyecto en Unity
2. File → Build Settings → Android
3. Switch Platform

### 2. Player Settings (Android)
- Company Name: [Tu compañía]
- Product Name: Captain Tsubasa Simple
- Version: 0.1.0
- Bundle Identifier: com.tunombre.captaintsubasa
- Minimum API Level: Android 8.0 (API 26)
- Target API Level: Latest
- Orientation: Landscape Left

### 3. Configuración de Gráficos
- Graphics API: OpenGLES3
- Multithreaded Rendering: Activado
- Strip Engine Code: Sí

### 4. Estructura de Escenas
1. 0_SplashScreen
2. 1_MainMenu
3. 2_TeamSelection
4. 3_MatchScene
5. 4_Results

### 5. Controles Móviles
- Joystick virtual izquierdo: Movimiento
- Botones derechos: Acciones
- Gestos: Swipe para técnicas especiales
