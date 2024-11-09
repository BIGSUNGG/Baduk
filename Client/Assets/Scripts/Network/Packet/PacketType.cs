namespace Network
{
    public enum PacketType
    {
        Unknown,

        C_LogIn,
        S_LogIn,

        C_SignUp,
        S_SignUp,

        S_EnterRoom,

        C_Chat,
        S_Chat,
    }
}