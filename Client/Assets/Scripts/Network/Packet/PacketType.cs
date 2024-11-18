namespace Network
{
    public enum PacketType
    {
        Unknown,

        C_LogIn,
        S_LogIn,

        C_SignUp,
        S_SignUp,

        C_StartMatch,
        S_EnterRoom,

        C_Chat,
        S_Chat,

        C_PlaceStone,
        S_PlaceStone,

        S_GameFinish,

        C_RequestTopRank,
        S_ResponseTopRank,
    }
}