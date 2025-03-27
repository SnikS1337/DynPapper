#include "DirectXRenderer.h"
#include "../Utilities/Profiler.h"

bool DirectXRenderer::Initialize(HWND hwnd) {
    DXGI_SWAP_CHAIN_DESC sd = {};
    sd.BufferCount = 2;
    sd.BufferDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
    // ... остальная инициализация DirectX
    
    return SUCCEEDED(D3D11CreateDeviceAndSwapChain(
        nullptr, D3D_DRIVER_TYPE_HARDWARE, nullptr, 0,
        nullptr, 0, D3D11_SDK_VERSION, &sd,
        &m_swapChain, &m_device, nullptr, &m_context));
}