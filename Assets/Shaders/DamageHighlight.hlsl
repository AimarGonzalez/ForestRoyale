// Assets/Shaders/DamageHighlight.hlsl
float4 PlayDamageHilight(float4 col, float4 damageColor, float damageIntensity)
{
    // Blend the damage color based on intensity
    return lerp(col, damageColor, damageIntensity);
}