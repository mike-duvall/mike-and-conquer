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
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    

};


Texture2D PaletteTexture;
sampler2D PaletteTextureSampler = sampler_state
{
    Texture = <PaletteTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D MapTileVisibilityTexture;
sampler2D MapTileVisibilityTextureSampler = sampler_state
{
    Texture = <MapTileVisibilityTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D Value13MrfTexture;
sampler2D Value13MrfTextureSampler = sampler_state
{
    Texture = <Value13MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};


Texture2D Value14MrfTexture;
sampler2D Value14MrfTextureSampler = sampler_state
{
    Texture = <Value14MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};

Texture2D Value15MrfTexture;
sampler2D Value15MrfTextureSampler = sampler_state
{
    Texture = <Value15MrfTexture>;
	addressU = Clamp;
	addressV = Clamp;
	mipfilter = NONE;
	minfilter = POINT;
	magfilter = POINT;    
};

Texture2D Value16MrfTexture;
sampler2D Value16MrfTextureSampler = sampler_state
{
    Texture = <Value16MrfTexture>;
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

bool DrawShroud;


float4 RenderPaletteValue(float4 color)
{
	int numPaletteEntries = 256.0f;
	if(color.a) {
		float paletteIndex = (color.r * 256.0f) / numPaletteEntries;
		float2 paletteCoordinates = float2(paletteIndex, 0.5f);
	    float4 paletteColor = tex2D(PaletteTextureSampler, paletteCoordinates);
	    return paletteColor;
	}
	return color;

}


float4 MapPixelToShadowValue(float4 originalColor, sampler2D mrfSampler) 
{
	int numPaletteEntries = 256.0f;	
	float mrfPaletteIndex = (originalColor.r * 256.0f) / numPaletteEntries;
	float2 mrfPaletteCoordinates = float2(mrfPaletteIndex, 0.5f);
    float4 mrfColor = tex2D(mrfSampler, mrfPaletteCoordinates);
    return mrfColor;
}



float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 mapTileVisibilityColor = tex2D(MapTileVisibilityTextureSampler, input.TextureCoordinates);	

	float rValueFor12 = 12.0f / 255.0f;
	float rValueFor13 = 13.0f / 255.0f;
	float rValueFor14 = 14.0f / 255.0f;
	float rValueFor15 = 15.0f / 255.0f;
	float rValueFor16 = 16.0f / 255.0f;

	int numPaletteEntries = 256.0f;

	if(!DrawShroud) {
		return RenderPaletteValue(color);
	}

	if(mapTileVisibilityColor.a == 0)  {
		return RenderPaletteValue(color);		    
	}
	else if(mapTileVisibilityColor.r == rValueFor13) {
	    float4 mrfColor = MapPixelToShadowValue(color, Value13MrfTextureSampler);
	    return RenderPaletteValue(mrfColor);		    
	}
	else if(mapTileVisibilityColor.r == rValueFor14) {
	    float4 mrfColor = MapPixelToShadowValue(color, Value14MrfTextureSampler);	    
	    return RenderPaletteValue(mrfColor);		    
	
	}
	else if(mapTileVisibilityColor.r == rValueFor15) {
	    float4 mrfColor = MapPixelToShadowValue(color, Value15MrfTextureSampler);	    	    
	    return RenderPaletteValue(mrfColor);		    		    
	}
	else if(mapTileVisibilityColor.r == rValueFor16) {
	    float4 mrfColor = MapPixelToShadowValue(color, Value16MrfTextureSampler);	    	    
	    return RenderPaletteValue(mrfColor);		    		    
	}
	else
	{
		// If it's not part of visible map
		// and it's not part of the shroud
		// then return Black
		return float4(0,0,0,1);			
	}

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};