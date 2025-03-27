#pragma once
#include "IEffect.h"

class WaveEffect : public IEffect {
public:
    void Apply(ID3D11DeviceContext* context, 
               ID3D11ShaderResourceView* input,
               ID3D11UnorderedAccessView* output) override;
private:
    ID3D11ComputeShader* m_shader = nullptr;
};