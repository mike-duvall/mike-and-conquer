#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

Texture2D ShadowTexture;
sampler2D ShadowTextureSampler = sampler_state
{
    Texture = <ShadowTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D UnitMrfTexture;
sampler2D UnitMrfTextureSampler = sampler_state
{
    Texture = <UnitMrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MapPixelToShadowValue(float4 originalColor, sampler2D mrfSampler) 
{
	int numPaletteEntries = 256.0f;	
	float mrfPaletteIndex = (originalColor.r * 256.0f) / numPaletteEntries;
	float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
    float4 mrfColor = tex2D(mrfSampler, mrfPaletteCoordinates);
    return mrfColor;
}

bool HasShadow(float4 shadowColor)
{
	if(shadowColor.r == 0)
	{
		return false;
	}
	else
	{
		return true;
	}
}

// Input:
//      SpriteTexture:  Texture with map tiles rendered to it, where r == palette value (g and b are ignored)
// 		ShadowTexture:  Texture with just unit and terrain shadows for entire map.  Non-zero value in r means there is a shadow for that pixel
//      UnitMrfTexture: A 256x1 texture for mapping terrain pixels to their shadow value (This texture is derived from the tunits.mrf file)
//
// Output:
//      If the pixel is in shadow, returns the appropriate shadow value(as palette value)
//      If the pixel is NOT in shadow, returns the original pixel from SpriteTexture(as palette value)
float4 MainPS(VertexShaderOutput input) : COLOR
{

	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 shadowColor = tex2D(ShadowTextureSampler, input.TextureCoordinates);

	if(HasShadow(shadowColor)) {
	 	return MapPixelToShadowValue(color,UnitMrfTextureSampler);		
	}
	else {
		return color;
	}

}


technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};