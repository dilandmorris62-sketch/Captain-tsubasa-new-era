# 📱 OPTIMIZACIÓN PARA ANDROID

## Configuración de Calidad (Quality Settings)

### Niveles de Calidad Recomendados:
1. **Low (Teléfonos viejos)**
   - Texture Quality: Half Res
   - Anisotropic Filtering: Disabled
   - Anti Aliasing: Disabled
   - Shadow Resolution: Low
   - Shadow Distance: 30

2. **Medium (Teléfonos promedio)**
   - Texture Quality: Full Res
   - Anisotropic Filtering: Per Texture
   - Anti Aliasing: 2x Multi Sampling
   - Shadow Resolution: Medium
   - Shadow Distance: 50

3. **High (Teléfonos nuevos)**
   - Texture Quality: Full Res
   - Anisotropic Filtering: Forced On
   - Anti Aliasing: 4x Multi Sampling
   - Shadow Resolution: High
   - Shadow Distance: 70

## Optimización de Texturas

### Resoluciones Recomendadas:
- Sprites de jugadores: 128x128 px
- Sprites del campo: 1024x1024 px (tilable)
- UI Elements: según densidad de pantalla
- Efectos de partículas: 64x64 px

### Formatos de Textura:
- PNG para sprites con transparencia
- JPG para fondos sin transparencia
- Usar Sprite Atlases para reducir draw calls

## Optimización de Scripts

### Buenas Prácticas:
1. **Update() Optimization:**
   - Usar coroutines para tareas no críticas
   - Agrupar cálculos pesados
   - Usar InvokeRepeating en vez de Update cuando sea posible

2. **Física:**
   - Usar FixedUpdate para física
   - Configurar Layer Collision Matrix
   - Usar Trigger colliders cuando sea posible

3. **Pooling de Objetos:**
   - Pool para balones
   - Pool para efectos de partículas
   - Pool para textos flotantes

## Configuración de Build

### Player Settings:
