
void main()
{
	gColor = iColor;
	gl_Position = uWorldViewProjection * vec4(iPosition, 1.0);
}
