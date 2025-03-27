#pragma once
#include <d3d11.h>
#include <dxgi1_2.h>

class DirectXRenderer {
public:
    bool Initialize(HWND hwnd);
    void Shutdown();
    void RenderFrame();
    void LoadTexture(const wchar_t* filePath);
    
private:
    ID3D11Device* m_device = nullptr;
    ID3D11DeviceContext* m_context = nullptr;
    IDXGISwapChain* m_swapChain = nullptr;
};
