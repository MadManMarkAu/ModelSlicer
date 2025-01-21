struct VS_IN
{
	float3 vPos : POSITION;
	float3 vNormal : NORMAL;
	float4 vColor : COLOR;
};

struct PS_IN
{
	float4 vPos : SV_POSITION;
	float4 vNormal : NORMAL;
	float4 vColor : POSITION0;
};

cbuffer Matricies : register(b0)
{
	float4x4 mModel; // Model-to-world-space transformation matrix
	float4x4 mView; // World-to-view-space transformation matrix
	float4x4 mProj; // 3D-to-2D projection matrix
};

cbuffer LightParams : register(b1)
{
	float3 vAmbientLightColor;
	float fPadding1;
	float3 vDirectionalLightColor;
	float fPadding2;
	float3 vDirectionalLightDir;
	float fPadding3;
	bool bUseLighting;
	float fPadding4;
	float fPadding5;
	float fPadding6;
}

PS_IN VS(VS_IN input)
{
	float4x4 mModelView = mul(mModel, mView);
	float4x4 mModelViewProj = mul(mModelView, mProj);

	PS_IN output = (PS_IN)0;

	output.vPos = mul(float4(input.vPos, 1.0f), mModelViewProj);
	output.vNormal = normalize(mul(float4(input.vNormal, 0.0f), mModelView));
	output.vColor = input.vColor;

	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	float4 vLightVec;
	float4 vNormal;
	float fDot;

	if (bUseLighting) {
		vNormal = normalize(input.vNormal);

		vLightVec = normalize(float4(vDirectionalLightDir, 0.0f));
		fDot = -min(dot(vLightVec, vNormal), 0.0f);

		return saturate((float4(vDirectionalLightColor, 1.0f) * fDot + float4(vAmbientLightColor, 1.0f)) * input.vColor);
	}
	else {
		return saturate(input.vColor);
	}
}
