#include "Common.hlsli"

Texture2D<float4> Input : register(t0);
RWTexture2D<float4> Output : register(u0);

[numthreads(16, 16, 1)]
void CSMain(uint3 tid : SV_DispatchThreadID) {
    float2 uv = tid.xy / gResolution;
    float wave = sin(uv.x * 10 + gTime) * 0.05;
    Output[tid.xy] = Input.SampleLevel(gSampler, float2(uv.x, uv.y + wave), 0);
}